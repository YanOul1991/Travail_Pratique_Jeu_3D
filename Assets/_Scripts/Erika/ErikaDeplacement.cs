using System.Diagnostics;
using UnityEngine;
/* 
    Script des gestion des deplacements du joueur ayant les fonctionalites suivantes : 
        - Etats debout/accroupi du joueur avec la touche 'ctrl' gauche;
        - Variations de vitesse deplacement selon l'etat du joueur + sprint avec 'shift' gauche
        - Animations de deplacement du personnage selon l'etat + deplacement du joueur + sauts avec 'espace'
        
    Par : Yanis Oulmane
    Derniere modification : 02/11/2024
 */
public class ErikaDeplacement : MonoBehaviour
{
    /* =================================================================================================== */
    /* ================================= VARIABLES DEPLACEMENTS ET SAUTS ================================= */
    /* =================================================================================================== */

    /* VARIABLES POUR DEPLACEMENTS AU SOL */
    [SerializeField] float vitesseMarche; // Vitesse du deplacement de type marche
    [SerializeField] float vitesseJogging; // Vitesse du deplacement de type jogging
    [SerializeField] float vitesseSprint; // Vitesse du deplacement de type sprint
    [SerializeField] float vitesseAccroupi; // Vitesse du deplacement lorsque le joueur est en etat accroupi
    float vitesseReel; // Vitesse qui sera applique au personnage selon son etat
    float velociteX; // Variable memorisant la Velocite en X du personnage
    float velociteZ; // Variable memorisant la Velocite en Y du personnage

    /* VARIABLES POUR LES SAUTS */
    [SerializeField] float forceSaut; // Force de saut du joueur
    [SerializeField] float forceGravite; // Force de gravite qui sera applique au joueur
    private float velociteY; // Variable memorisant la velocite Y du joueur en temps reel;
    private bool auSol; // Bool memorisant si le personnage touche au sol

    /* =================================================================================================== */
    /* ==================================== REFERENCES AUX COMPONENTS ==================================== */
    /* =================================================================================================== */
    CharacterController controleur;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        /*=============== Assignations des references des components ===============*/
        controleur = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        /* ======================================================================== */

        // Par defaut verouille le curseur au centre de l'ecran
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Fonction Update() gere les Inputs du joueurs et les etats du personnage  
    void Update()
    {
        // Si le joueur appuit sur la 'espace' et qu'il est au sol
        if (Input.GetKeyDown(KeyCode.Space) && auSol)
        {
            // Applique la force de saut a la velociteY du personnage
            velociteY = forceSaut;

            // Set le Trigger de saut de l'animator
            animator.SetTrigger("saut");
        }

        // Vitesse reel = vitesse de marche par defaut
        vitesseReel = vitesseMarche;

        // 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            vitesseReel = vitesseAccroupi;
        }

        animator.SetBool("accroupi", Input.GetKey(KeyCode.LeftControl) ? true : false);
        
        // Si le joueur appuit sur le shift gauche
        if (Input.GetKey(KeyCode.LeftShift))
        {
            vitesseReel = vitesseSprint;
        }
    }

    // Fonction FixedUpdate gere les deplacements des objets et les animations
    void FixedUpdate()
    {
        /* ======================= DEPLACEMENTS ET DES SAUTS ======================= */
        // Si le personnage est au sol
        if (auSol)
        {
            // Permet la modifications des velocite X et Z du personnage
            velociteX = Input.GetAxisRaw("Horizontal");
            velociteZ = Input.GetAxisRaw("Vertical");
        }

        // Quand le joueur touche le sol avec un velocite en Y negative
        if (auSol && velociteY < 0)
        {
            // Remet la velocite en Y a 0
            velociteY = 0;
        }

        // Applique la force de gravite quand le joueur est dans les airs
        if (!auSol)
        {
            velociteY += forceGravite * Time.deltaTime;
        }

        // Variable locale Vector3 qui memorisera le vecteur de deplacement du personnage
        Vector3 deplacement = transform.TransformDirection(new Vector3(velociteX, 0, velociteZ).normalized * vitesseReel);

        // Verifie si le personnage touche au sol 
        RaycastHit collisionSphereCast;
        auSol = Physics.SphereCast(transform.position + new Vector3(0f, 0.3f, 0f), 0.3f, Vector3.down, out collisionSphereCast, 0.3f);

        deplacement.y = velociteY;

        controleur.Move(deplacement * Time.deltaTime);

        GestionAnimations(deplacement);

        /* ================================= DEBUG ================================= */
    }   

    void GestionAnimations(Vector3 deplacementSol)
    {       
        // Paramtre bool 'auSol' de l'animator sera equivalent a la variable bool auSol de la class
        animator.SetBool("auSol", auSol);

        /* ==================== ANIMATIONS DE DEPLACEMENT AU SOL ==================== */
        // Recupere le parametre deplacementSol et assigne la valeur de 0 a son vecteur Y
        // pour seulement verifier les deplacements en X et en Z
        deplacementSol.y = 0;

        animator.SetFloat("vitesse", Mathf.Floor(deplacementSol.magnitude));
 
        /* ================== DEBUGS ================== */
        // UnityEngine.Debug.Log(Mathf.Floor(deplacementSol.magnitude ));
    }

    /*====================== DEBUGS====================== */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 0.3f, 0f), 0.3f);
    }
}
