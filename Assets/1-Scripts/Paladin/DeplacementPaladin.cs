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
    /* =================================== VARIABLES =================================== */

    /* --------------- VAR MOUVEMENTS --------------- */
    [SerializeField] Transform joueurTransform; // Reference au componenent Transform du joueur;
    [SerializeField] float vitesseMarche; // Variable memorisant la vitesse de marche du paladin
    [SerializeField] float vitesseCourse; // Variable memorisant la vitesse de course du paladin

    /* ----------------- VAR ETATS ----------------- */
    bool enChasse; // Variable memorisant si le paladin est entrain de chasser le joueur

    /* --------------- REF COMPONENTS --------------- */
    CharacterController controlleurPerso; // Reference au CharacterController component du paladin
    NavMeshAgent navAgent; // References au component NavMeshAgent du paladin;

    VisionPaladin visionPaladin; // Reference a la classe VisionPaladin
    PatrouillePaladin patrouillePaladin; // Reference a la classe PatrouillePaladin;
    AnimationsPaladin animPaladin; // Reference a la classe AnimationsPaladin;

    /* =========================================================================== */

    // Start is called before the first frame update
    void Start()
    {
        // Assigniation des references de components et scripts 
        controlleurPerso = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();

        visionPaladin = GetComponent<VisionPaladin>();
        patrouillePaladin = GetComponent<PatrouillePaladin>();
        animPaladin = GetComponent<AnimationsPaladin>();
    }

    void FixedUpdate()
    {
        // Par defaut le paladin ne se deplace pas
        // Vector3 mouvement = Vector3.zero;

        // // Applique un force de gravite au paladin a chaque seconde
        // float velociteY = 0;
        // velociteY += -10 * Time.deltaTime;

        // // Remet la velocite a -10 apres un certain treshold
        // if (velociteY < -50)
        // {
        //     velociteY = -10;
        // }

        // vitesseReel = vitesseCourse;
        // transform.LookAt(posCible);
        // mouvement = transform.forward * vitesseCourse;   
        // // Fonction de gestion des animations du paladin
        // GetComponent<AnimationsPaladin>().GestionAnimations(mouvement, posCible, transform);

        // Arrange les rotation pour pas qu'il tourne sur lui meme
        // transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);

        // Applique la force de gravite au mouvement global du paladin
        // Applique la somme des mouvements au CharacterController
        // mouvement.y = velociteY;
        // controlleurPerso.Move(mouvement * Time.deltaTime);

        /* ==================================================================================================================== */
        /* =================================================== NavMeshAgent =================================================== */
        /* ==================================================================================================================== */

        // Appel de la fonction ConditionsChase pour determiner si le paladin est en etat de chase
        // enChasse = ConditionsChasse(GameManager.joueurVisible,GameManager.joueurVivant, visionPaladin.JoueurDansVision());

        if (enChasse)
        {

            navAgent.SetDestination(joueurTransform.position);

            animPaladin.AnimationDeplacement(navAgent.velocity.magnitude);
        }

        if (Vector3.Distance(transform.position, joueurTransform.position) < 3)
        {
            animPaladin.AnimationAttaque();
        }
    }

    /* 
        Fonction qui verifie si toute les conditions qui permettent de mettre le personnage en etat de chasse
        sont respectes.
        
        Passe comme parametre des bool; 
    */
    bool ConditionsChasse(params bool[] lesConditions)
    {
        foreach (bool cond in lesConditions)
        {
            if (!cond)
            {
                return false;
            }
        }

        return true;
    }
}