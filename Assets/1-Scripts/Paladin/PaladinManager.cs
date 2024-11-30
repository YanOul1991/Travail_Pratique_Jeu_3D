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

        if (visionPaladin.GetJoueurRepere() && etat == EtatsPaladin.PATROUILLE) StartCoroutine(GestionChasse());
    }


    /* ============================= METHODES ============================= */

    /// <summary>
    ///     Coroutine de gestion de l'etat de chasse du paladin.
    /// </summary>
    IEnumerator GestionChasse()
    {

        Debug.Log("chase coroutine started");        
        etat = EtatsPaladin.CHASSE;

        posAlerte = transform.position;
        
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

       
        // Une fois le joueur perdu de vue par le paladin 
        // Demmarage de la coroutine d'inspection et arret de la coroutine en cours
        Debug.Log("lost player from fov");
        StartCoroutine(GestionInspection());
        yield break;
    }

    /// <summary>
    ///     Coroutine de gestion de l'etat d'insepction du paladin.
    /// </summary>
    IEnumerator GestionInspection()
    {
        etat = EtatsPaladin.INSPECTION;
        deplacementPaladin.SetVitesseDeplac();

        // Attendre que le paladin soit proche de la derniere position connue du joueur
        while (deplacementPaladin.GetDistanceCible() > 0.5f)
        {
            // 
            if (visionPaladin.GetJoueurRepere())
            {
                Debug.Log("interupted before point");
                StartCoroutine(GestionChasse());
                yield break;
            }
            yield return new WaitForSeconds(1 / 15);
        }

        Debug.Log("Arrived at last known player location");
        // Weewoo
    }


    /// <summary>
    ///     Methode qui retourne l'etat actuel du paladin
    /// </summary>
    /// <returns>Etat du paladin</returns>
    public EtatsPaladin GetEtatPaldin()
    {
        return etat;
    }
}
