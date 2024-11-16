using UnityEngine;
using UnityEngine.AI;
/* 
    Scripte de gestion des deplacements d'un Paladin :
        - Gestion des vitesses et des animations
        - Gestin des rotation selon cible que le paladin va suivre

    Par Yanis Oulmane
    Derniere modification : 04-11-2024
 */
public class DeplacementPaladin : MonoBehaviour
{
    /* ================================ VARIABLES ================================ */

    /* --------------- Mouvements --------------- */
    [SerializeField] private float vitesseMarche; // Variable memorisant la vitesse de marche du paladin;
    [SerializeField] private float vitesseCourse; // Variable memorisant la vitesse de course du paladin;\

    /* --------------- Positions --------------- */
    private GameObject joueur;
    private Vector3 posAlerte;
    private Vector3 posPatrouille; 

    /* ----------------- Etats ----------------- */
    private int etat; // int memorisant 
    private const int PATROUILLE = 0; //const int memorisant la valeur de PATROUILLE
    private const int INSPECTION = 1; //const int memorisant la valeur de INSPECTION
    private const int CHASSE = 2; //const int memorisant la valeur de CHASSE


    /* ---------- Components / Class ---------- */
    private NavMeshAgent navAgent; // References au component NavMeshAgent du paladin;
    private VisionPaladin visionPaladin; // Reference a la classe VisionPaladin
    private PatrouillePaladin patrouillePaladin; // Reference a la classe PatrouillePaladin;

    /* =========================================================================== */
    /* =========================================================================== */

    // Start is called before the first frame update
    void Start()
    {
        joueur = GameObject.FindWithTag("Player");

        // Assigniation des references de components et scripts 
        navAgent = GetComponent<NavMeshAgent>();
        visionPaladin = GetComponent<VisionPaladin>();
        patrouillePaladin = GetComponent<PatrouillePaladin>();

        etat = PATROUILLE;

        // Appel la methode de patrouille une premiere fois
        posPatrouille = patrouillePaladin.Destination();

    }

    void Update()
    {

        if (Vector3.Distance(transform.position, joueur.transform.position) < 30 && !GameManager.joueurCache && visionPaladin.VisionSpotting())
        {
            EtatPaladin();
        }

        if (etat == CHASSE && GameManager.joueurCache)
        {
            etat = INSPECTION;
            navAgent.SetDestination(posAlerte);
            navAgent.stoppingDistance = 0;
        }

        if (etat == INSPECTION && navAgent.remainingDistance < 0.1f)
        {
            etat = PATROUILLE;
            navAgent.SetDestination(patrouillePaladin.Destination());
        }

        Debug.Log(navAgent.remainingDistance);


        switch (etat)
        {
            case PATROUILLE:
                navAgent.areaMask = 1 << NavMesh.GetAreaFromName("Route");

                navAgent.speed = vitesseMarche;

                // Verifie si le Paladin est arrive a son point de patrouille
                // Si oui, passe au suivant
                if (navAgent.remainingDistance < 0.1f)
                {
                    navAgent.SetDestination(patrouillePaladin.Destination());
                }

                break;

            case INSPECTION:
                navAgent.areaMask = NavMesh.AllAreas;
                break;

            case CHASSE:
                navAgent.areaMask = NavMesh.AllAreas;

                navAgent.speed = vitesseCourse;

                navAgent.SetDestination(joueur.transform.position);
                break;
        }
    }

    void EtatPaladin()
    {
        if (etat == PATROUILLE)
        {
            navAgent.stoppingDistance = 0.5f;
            posAlerte = transform.position;
            etat = CHASSE;
        }
    }
}