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
    /* ============================================================================= */
    /* ================================= VARIABLES ================================= */
    /* ============================================================================= */

    /***************** Mouvements *****************/
    [SerializeField] Transform joueurTransform; // Reference au componenent Transform du joueur;
    [SerializeField] float vitesseMarche; // Variable memorisant la vitesse de marche du paladin
    [SerializeField] float vitesseCourse; // Variable memorisant la vitesse de course du paladin
    Vector3 posAlerte;
    Vector3 posDestination;

    /******************* Etats *******************/
    bool enPatrouille; // Variable memorisant si le paladin est etat de patrouille;
    bool enChasse; // Bool memorisant si le paladin est entrain de chasser le joueur;
    bool enInspection; // Bool memorisant si le paladin est en etat d'inspection

    /********** Ref components/classes **********/
    NavMeshAgent navAgent; // References au component NavMeshAgent du paladin;
    VisionPaladin visionPaladin; // Reference a la classe VisionPaladin
    PatrouillePaladin patrouillePaladin; // Reference a la classe PatrouillePaladin;
    AnimationsPaladin animPaladin; // Reference a la classe AnimationsPaladin;

    /* =========================================================================== */
    /* =========================================================================== */
    /* =========================================================================== */

    // Start is called before the first frame update
    void Start()
    {
        // Assigniation des references de components et scripts 
        // controlleurPerso = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();

        visionPaladin = GetComponent<VisionPaladin>();
        patrouillePaladin = GetComponent<PatrouillePaladin>();
        animPaladin = GetComponent<AnimationsPaladin>();
    }

    void FixedUpdate()
    {
        /* ---------------------------------------------------- */
        /* ----------------------- TEMP ----------------------- */
        enChasse = false;
        /* ----------------------- TEMP ----------------------- */
        /* ---------------------------------------------------- */

        // Deplacements du paladin si il est en train de chasser le joueur;
        if (enChasse)
        {
            // Le paladin peut desormais se deplacer sur tout le navMesh
            // La destination du paladin devient la position du joueur
            navAgent.areaMask = NavMesh.AllAreas;
            navAgent.SetDestination(joueurTransform.position);
        }

        // Deplacements du paladin
        if (!enChasse)
        {
            // Si le personnge n'est pas en mode patrouille
            // Il retourne a la position ou il etait lors de l'alerte
            if (!enPatrouille)
            {   
                posDestination = posAlerte;
            }

            // Si le paladin est a sa pos initiale
            // Il revient en mode patrouille
            // Et ne se deplace que sur la route
            if (navAgent.remainingDistance < 0.3f)
            {
                enPatrouille = true;
                navAgent.areaMask = 1 << NavMesh.GetAreaFromName("route");
            }

            // Si le paldin est en mode patrouille et qu'il arrive a son point de destination
            // Appel de la methode DestinationDeplacement() pour passer au prochain point de patrouille
            if (enPatrouille && navAgent.remainingDistance < 0.5f)
            {
                posDestination = patrouillePaladin.DestinationDeplacement().position;
            }   

            // Assigne la destination
            navAgent.SetDestination(posDestination);
        }

        // Verifie si le joueur est a une certaine distance et que le paladin est en etat de chasse
        // pour appeler la methode AnimationAttaque de la classe animPaladin
        if (Vector3.Distance(transform.position, joueurTransform.position) < 3 && enChasse)
        {
            animPaladin.AnimationAttaque();
        }
    }
    
    // Fonction retournant un bool verifiant si toutes les conditions pour que le paladin 
    // commence a chaser le joueur sont respectes
    bool ConditionsChasse(params bool[] lesConditions)
    {   
        // Boucle retourne false quand une condition n'est pas respecte
        foreach (bool cond in lesConditions)
        {
            if (!cond)
            {
                return false;
            }
        }

        // Si passe a travers la boucle, retourne  true;
        return true;
    }
}