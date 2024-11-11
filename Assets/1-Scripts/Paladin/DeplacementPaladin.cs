using UnityEngine;
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
    [SerializeField] Transform posCible; // Reference au componenent Transform d'une cible que le Paladin regardera;
    [SerializeField] float vitesseMarche; // Variable memorisant la vitesse de marche du paladin
    [SerializeField] float vitesseCourse; // Variable memorisant la vitesse de course du paladin

    /* ----------------- VAR ETATS ----------------- */
    bool enChasse; // Variable memorisant si le paladin est entrain de chasser le joueur

    /* --------------- REF COMPONENTS --------------- */
    CharacterController controlleurPerso; // Reference au CharacterController component du paladin
    VisionPaladin visionPaladin; // Reference a la classe VisionPaladin
    PatrouillePaladin patrouillePaladin; // Reference a la classe PatrouillePaladin

    /* =========================================================================== */

    // Start is called before the first frame update
    void Start()
    {
        // assigniation des references de components et scripts 
        controlleurPerso = GetComponent<CharacterController>();

        visionPaladin = GetComponent<VisionPaladin>();

        patrouillePaladin = GetComponent<PatrouillePaladin>();
    }

    void FixedUpdate()
    {
        // Par defaut le paladin ne se deplace pas
        Vector3 mouvement = Vector3.zero;

        // Applique un force de gravite au paladin a chaque seconde
        float velociteY = 0;
        velociteY += -10 * Time.deltaTime;

        // Remet la velocite a -10 apres un certain treshold
        if (velociteY < -50)
        {
            velociteY = -10;
        }

        // Appel de la fonction ConditionsChase pour determiner si le paladin est en etat de chase
        enChasse = ConditionsChasse(GameManager.joueurVisible,GameManager.joueurVivant, visionPaladin.JoueurDansVision());

        if (enChasse)
        {
            // vitesseReel = vitesseCourse;
            transform.LookAt(posCible);
            mouvement = transform.forward * vitesseCourse;   
            // Fonction de gestion des animations du paladin
            GetComponent<AnimationsPaladin>().GestionAnimations(mouvement, posCible, transform);
        }
        
        if (!enChasse)
        {
            transform.LookAt(patrouillePaladin.DestinationDeplacement().position);
        }

        // Arrange les rotation pour pas qu'il tourne sur lui meme
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);

        // Applique la force de gravite au mouvement global du paladin
        // Applique la somme des mouvements au CharacterController
        mouvement.y = velociteY;
        controlleurPerso.Move(mouvement * Time.deltaTime);

    }

    /* 
        Fonction qui verifie si toute les conditions qui permettent de mettre le personnage en etat de chasse
        sont respectes.
        
        Passe comme parametre des bool; 
    */
    bool ConditionsChasse(params bool[] lesConditions)
    {
        foreach(bool cond in lesConditions)
        {
            if (!cond)
            {
                return false;
            }
        }

        return true;
    }
}