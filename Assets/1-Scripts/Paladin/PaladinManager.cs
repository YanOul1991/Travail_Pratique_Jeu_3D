using UnityEngine;
using System.Collections;

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
    public enum EtatsPaladin // public enum des differents etats possibles du paladin
    {
        PATROUILLE,
        INSPECTION,
        CHASSE
    }

    private string coroutineEnCours; // string memorisant le nom de la coroutine de gestion d'etat en cours

    private bool corChasseEnCours = false;
    private bool coroutineInspecDemar = false;
    private EtatsPaladin etat; // Variable prive memorisant l'etat du paladin
    private Vector3 posPointPatrouille; // Variable privee memorisant le point de patrouille du paladin
    private Vector3 posAlerte; // Var memorisant la position du paladin au moment de l'alerte

    void Start()
    {
        // Assigne references aux autres classes
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

        coroutineEnCours = nameof(GestionChasse);

        /* ------------------- DEBUG ------------------- */
        // StartCoroutine(LocalDebugDisplayInfo(1f));

        Debug.Log("nom de la coroutine en cours : " + coroutineEnCours);
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
        if (visionPaladin.GetJoueurRepere() && !corChasseEnCours)
        {
            StartCoroutine(GestionChasse());
        }
    }


    /* ============================= METHODES ============================= */

    /// <summary>
    ///     Coroutine de gestion de l'etat de chasse du paladin.
    /// </summary>
    IEnumerator GestionChasse()
    {
        /* ================= MODIFICATION DE VARIBALES ================= */
        // Indique le la coroutine est en cours de marche
        // Mise a jours de letat du paladin et de sa vitesse de deplacement
        
        posAlerte = deplacementPaladin.GetPositionAlerte();
        corChasseEnCours = true;
        etat = EtatsPaladin.CHASSE;
        deplacementPaladin.SetVitesseDeplac();
        deplacementPaladin.SetNavArea();

        /* ================= BOUCLE DE CHASSE DU JOUEUR ================= */
        // Appel le fonction de chasse du joueur tant que celui-ci;
        // sera dans la vision du paladin ;
        while (visionPaladin.GetJoueurRepere())
        {
            deplacementPaladin.ChasserJoueur();
            yield return new WaitForSeconds(1 / 15);
        }

        /* ======================  CHANGEMENT ETAT ====================== */
        // Une fois que le joueur n'est plus visible
        //  - Arrete la coroutine de chasse;
        //  - Memorise qu'elle ne roule plus'
        //  - Appel la coroutine de gestion de l'etat d'inspection

        StartCoroutine(GestionInspection());
        corChasseEnCours = false;
        
        /* ======================= FIN COROUTINE ======================= */
        yield break;
    }

    /// <summary>
    ///     Coroutine de gestion de l'etat d'insepction du paladin.
    /// </summary>
    IEnumerator GestionInspection()
    {
        /* =================== UPDATE DES VARIABLES =================== */

        // Met a jour les variable approprie
        //  - Memorise que la coroutine d'inspection est demmare;
        //  - Met a jour l'etat du paladin;\
        //  - Met a jour la vitesse de deplacement du paladin
        coroutineInspecDemar = true;
        etat = EtatsPaladin.INSPECTION;
        deplacementPaladin.SetVitesseDeplac();

        /* =============== BOUCLE DERNIERE POS JOUEUR =============== */

        // Attendre que le Paladin se trouve a quelques metres de la derniere position 
        // connu du joueur avant de continuer
        while (deplacementPaladin.GetDistanceCible() > 0.1f)
        {
            // Si le paladin repere de nouveau le joueur
            // Retourne en mode chasse
            if (visionPaladin.GetJoueurRepere())
            {
                StopCoroutine(GestionInspection());
                coroutineInspecDemar = false;
                StartCoroutine(GestionChasse());
            }

            yield return new WaitForSeconds(1 / 15);
        }

        while (true)
        {
            Debug.Log("we wait at last known pos ...");
            yield return new WaitForSeconds(1);
        }
    }


    /// <summary>
    ///     Methode qui retourne l'etat actuel du paladin
    /// </summary>
    /// <returns>Etat du paladin</returns>
    public EtatsPaladin GetEtatPaldin()
    {
        return etat;
    }


    /* ---------------- DEBUGGAGE ---------------- */
    IEnumerator LocalDebugDisplayInfo(float interval)
    {
        while (true)
        {
            Debug.Log("\nvar : etat | value = " + etat);
            // Debug.Log("\n Coroutine chasse is playing : " + corChasseEnCours);

            yield return new WaitForSeconds(interval);
        }
    }
}
