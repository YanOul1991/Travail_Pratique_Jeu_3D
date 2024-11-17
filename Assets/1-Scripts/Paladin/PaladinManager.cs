using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/* 
    Scripte de gestion d'un paladin:
        - Gestion des 3 etats du paladin (patrouille, inspection, chasse);
        - Echanges de donnes entres les differents scriptes;
*/

public class PaladinManager : MonoBehaviour
{

    /* Variables globales */
    [SerializeField] private float distanceDetectionMax;

    /* --------------- References GameObject / Class --------------- */
    private GameObject joueur;

    // References aux autres scriptes du paladin
    private DeplacementPaladin deplacementPaladin;
    private PatrouillePaladin patrouillePaladin;
    private VisionPaladin visionPaladin;
    private AnimationsPaladin animationsPaladin;

    // public enum des differents etats possibles du paladin
    public enum EtatsPaladin
    {
        PATROUILLE,
        INSPECTION,
        CHASSE
    }

    private bool corChasseEnCours = false;
    private bool coroutineInspecDemar = false;

    private EtatsPaladin etat; // Variable prive memorisant l'etat du paladin
    private Vector3 posPointPatrouille; // Variable privee memorisant le point de patrouille du paladin
    private Vector3 posAlerte; // Var memorisant la position du paladin au moment de l'alerte

    void Start()
    {
        // Assignitation des references
        deplacementPaladin = GetComponent<DeplacementPaladin>();
        patrouillePaladin = GetComponent<PatrouillePaladin>();
        visionPaladin = GetComponent<VisionPaladin>();
        animationsPaladin = GetComponent<AnimationsPaladin>();

        // Reference au joueur en le trouvant grace a son tag
        joueur = GameObject.FindWithTag("Player");

        // Etat par defaut du paladin = PATROUILLE
        etat = EtatsPaladin.PATROUILLE;

        // Set le premier point de patrouille du paladin 
        // et fait deplacer le paladin vers cette position
        deplacementPaladin.SetNavArea();
        posPointPatrouille = patrouillePaladin.GetProchainPos();
        deplacementPaladin.SetPatrouillePos(posPointPatrouille);

        StartCoroutine(LocalDebugDisplayInfo(1f));
    }

    void Update()
    {
        // Analyse des scenarios possibles de detection et d'etats
        // La detection du joueur se fera seulment si celui-ci est a une certaine distance

        if (etat == EtatsPaladin.PATROUILLE)
        {
            if (deplacementPaladin.GetArrivePosPatrouille())
            {
                posPointPatrouille = patrouillePaladin.GetProchainPos();
                deplacementPaladin.SetPatrouillePos(posPointPatrouille);
            }
        }

        /* 
            Si le joueur se trouve dans la distance de detection permise,
            appel la methode qui indique si le paladin repere le joueur de la class VisionPaladin;
            Si le joueur est repere, appel de la methode qui set l'etat de chasse
        */
        if (Vector3.Distance(transform.position, joueur.transform.position) < distanceDetectionMax)
        {
            if (visionPaladin.GetJoueurRepere() && !corChasseEnCours)
            {
                StartCoroutine(GestionChasse());
            }
        }
    }


    /* ============================= METHODES ============================= */

    /* --------- Coroutines --------- */

    IEnumerator GestionChasse()
    {
        //  > Descr :
        //      Coroutine de Gestion de l'etat de chasse du 
        //      paladin apres qu'il repere le joueur;

        // Indique le la coroutine est en cours de marche
        // Mise a jours de letat du paladin et de sa vitesse de deplacement
        posAlerte = deplacementPaladin.GetPositionAlerte();
        corChasseEnCours = true;
        etat = EtatsPaladin.CHASSE;
        deplacementPaladin.SetVitesseDeplac();
        deplacementPaladin.SetNavArea();

        // Appel le fonction de chasse du joueur tant que celui-ci 
        // sera dans la vision du paladin 
        while (visionPaladin.GetJoueurRepere())
        {
            deplacementPaladin.SetChasserJoueur();
            yield return new WaitForSeconds(1 / 15);
        }

        // Apres que le paladin perd le joueur de vue
        // Attendre que celui-ci se trouve dans un certain permietre
        // De ou celui-ci a vue le joueur pour la derniere fois;
        while (GetComponent<NavMeshAgent>().remainingDistance > 5)
        {
            yield return new WaitForSeconds(1 / 15);
        }

        // Paladin inspct la dernier position du joueur pendant quelques secondes
        yield return new WaitForSeconds(10f);


        etat = EtatsPaladin.INSPECTION;
        deplacementPaladin.SetVitesseDeplac();

        deplacementPaladin.SetRetournPosAlerte(posAlerte);
        while (deplacementPaladin.GetDistanceCible() > 0.1f)
        {
            yield return new WaitForSeconds(1 / 15);
        }

        // Paladin attent 5 secondes avant de retourner en patrouille
        yield return new WaitForSeconds(5);

        // Le paladin retourne en mode patrouille
        // Et retourne vers le point de patrouille vers ou il devait se diriger
        // avant qu'il soit interompu
        etat = EtatsPaladin.PATROUILLE;
        deplacementPaladin.SetNavArea();
        deplacementPaladin.SetPatrouillePos(posPointPatrouille);

        // Arret de la coroutine de chasse
        StopCoroutine(GestionChasse());
        corChasseEnCours = false;
    }


    // Methode retournant l'etat du paladin
    public EtatsPaladin GetEtatPaldin()
    {
        //  > Descr : 
        //      Methode qui rotourne l'etat dans lequel le paladin se retrouve;
        // 
        //  > Retourne : EtatsPaladin;
        return etat;
    }


    /* ---------------- DEBUGGAGE ---------------- */
    IEnumerator LocalDebugDisplayInfo(float interval)
    {
        while (true)
        {
            Debug.Log("\nvar : etat | value = " + etat);
            Debug.Log("\n Coroutine chasse is playing : " + corChasseEnCours);

            yield return new WaitForSeconds(interval);
        }
    }
}
