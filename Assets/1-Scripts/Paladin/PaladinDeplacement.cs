using UnityEngine;
/* 
    Scripte de gestion des deplacements d'un Paladin :
        - Gestion des vitesses et des animations
        - Gestin des rotation selon cible que le paladin va suivre

    Par Yanis Oulmane
    Derniere modification : 04-11-2024
 */
public class PaladinDeplacement : MonoBehaviour
{
    /* =========================================================================== */
    /* ================================ VARIABLES ================================ */
    [SerializeField] Transform posCible; // Reference au componenent Transform d'une cible que le Paladin regardera;
    [SerializeField] float vitesseCourse; // Variable memorisant la vitesse de course du Paladin
    public bool joueurDansVision;
    private float vitesseReel; // Vitesse qui sera applique au mouvemen du personnage
    CharacterController controlleurPerso;
    /* =========================================================================== */

    // Start is called before the first frame update
    void Start()
    {
        controlleurPerso = GetComponent<CharacterController>();
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

        // Si le joueur est dans la champ de vision du paladin et qu'il nest pas cache
        // Alors le paladin cours vers le joueur
        // Paladin Regarde la cible
        if (!GAMEMANAGER.joueurVisible && joueurDansVision && Vector3.Distance(posCible.position, transform.position) > 2)
        {
            vitesseReel = vitesseCourse;
            transform.LookAt(posCible);
            mouvement = transform.forward * vitesseReel;   
        }

        // Arrange les rotation pour pas qu'il tourne sur lui meme
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);

        // Applique la force de gravite au mouvement global du paladin
        // Applique la somme des mouvements au CharacterController
        mouvement.y = velociteY;
        controlleurPerso.Move(mouvement * Time.deltaTime);

        // Fonction de gestion des animations du paladin
        GetComponent<PaladinAnimations>().GestionAnimations(mouvement, posCible, transform, joueurDansVision, GAMEMANAGER.joueurVisible, GAMEMANAGER.joueurVivant);
    }
}
