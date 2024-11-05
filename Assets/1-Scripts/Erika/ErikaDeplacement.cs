using UnityEngine;
/* 
    Script des gestion des deplacements du joueur ayant les fonctionalites suivantes : 
        - Etats debout/accroupi du joueur avec la touche 'ctrl' gauche;
        - Variations de vitesse deplacement selon l'etat du joueur + sprint avec 'shift' gauche
        - Animations de deplacement du personnage selon l'etat + deplacement du joueur + sauts avec 'espace'
        
    Par : Yanis Oulmane
    Derniere modification : 04/11/2024
 */
public class ErikaDeplacement : MonoBehaviour
{
    [SerializeField] bool estCache = true;
    [SerializeField] bool estMort = false;
    /* ================================================================ */
    /* ================= VARIABLES DEPLACEMENTS/SAUTS ================= */
    /* ================================================================ */


    /* VARIABLES POUR DEPLACEMENTS AU SOL */
    [SerializeField] float vitesseMarche; // Vitesse du deplacement de type marche
    [SerializeField] float vitesseJogging; // Vitesse du deplacement de type jogging
    [SerializeField] float vitesseSprint; // Vitesse du deplacement de type sprint
    [SerializeField] float vitesseAccroupi; // Vitesse du deplacement lorsque le joueur est en etat accroupi
    float vitesseReel; // Vitesse qui sera applique au personnage selon son etat
    float horizontal; // Variable memorisant la Velocite en X du personnage
    float vertical; // Variable memorisant la Velocite en Y du personnage
    [SerializeField] Transform laCamera; // Reference au component Transform de la camera

    /* VARIABLES POUR LES SAUTS */
    [SerializeField] float forceSaut; // Force de saut du joueur
    [SerializeField] float forceGravite; // Force de gravite qui sera applique au joueur
    private float velociteY; // Variable memorisant la velocite Y du joueur en temps reel;
    private bool auSol; // Bool memorisant si le personnage touche au sol

    /* ================================================================= */
    /* =================== REFERENCES AUX COMPONENTS =================== */
    /* ================================================================= */
    CharacterController controleur;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Assigniation des references aux components
        controleur = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Fonction Update() gere les Inputs du joueurs et les etats du personnage  
    void Update()
    {
        // Assigne la valeur de estCache a la variable joueurCache du GAMEMANAGER
        GAMEMANAGER.joueurCache = estCache;
        GAMEMANAGER.joueurMort = estMort;

        if (!estMort)
        {
            // Si le joueur appuit sur la 'espace' et qu'il est au sol
            // Applique la force de saut au deplacement en Y et Strigger le parametre saut de l'animator
            if (Input.GetKeyDown(KeyCode.Space) && auSol)
            {
                velociteY = forceSaut;
                animator.SetTrigger("saut");
            }

            // Par defaut la vitesse reel sera la vitesse de marche
            // Si la touche 'ctrl' de gauche est appuiye alors la vitesse accroupi sera applique
            // Si la touche 'shift' de gauche est appuye -> vitesse de sprint est applique et
            // ecrase vitesse accroupi si 'ctrl' et 'shift' sont appuye en meme temps

            vitesseReel = vitesseMarche;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                vitesseReel = vitesseAccroupi;
            }

            animator.SetBool("accroupi", Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift) ? true : false);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                vitesseReel = vitesseSprint;
            }
        }
    }

    // Fonction FixedUpdate gere les deplacements des objets et les animations
    void FixedUpdate()
    {
        // Verification de la collision avec le sol avec un SphereCast place aux pieds du personnage
        // Verifie si ce SphereCast entre en collision
        RaycastHit collisionSphereCast;
        auSol = Physics.SphereCast(transform.position + new Vector3(0f, 0.3f, 0f), 0.3f, Vector3.down, out collisionSphereCast, 0.3f);

        // Permet les modifications des deplacement en X et Z du personnage seuelemt si il touche au sol
        // Fix la velocite en Y a la force de gravite, pour evite qu'elle continue de trop baisser
        if (auSol && !estMort)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            if (velociteY < forceGravite)
            {
                velociteY = 0;
            }
        }

        /* 
            APPLICATION DES MOUVEMENTS AUX PERSONNAGE SELON L'ANGLE DE LA CAMERA

            - Transforme les vecteurs de la camera en vecteur gloable
            - Change le vecteur en Y a 0 pour que le vecteur sultant soit parfaitement a l'horizontal
            - Et normalize le vecteur pour qu'il pointe dans la bonne direction avec un valeur de 1
        */

        Vector3 mouvRelatif = laCamera.forward * vertical + laCamera.right * horizontal;
        mouvRelatif.y = 0;
        mouvRelatif = mouvRelatif.normalized * vitesseReel;

        if (auSol && mouvRelatif != Vector3.zero)
        {
            transform.forward = mouvRelatif.normalized;
        }

        // Applique la velocite en Y au vecteur de mouvementRelatif
        velociteY += forceGravite * Time.deltaTime;
        mouvRelatif.y = velociteY;

        // Applique le mouvement au personnage avec la methode Move() de son compoenent CharacterController
        controleur.Move(mouvRelatif * Time.deltaTime);

        // Envoit le vecteur mouvement relatif a la fonction de gestion des animations
        GestionAnimations(mouvRelatif);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "epee")
        {
            Debug.Log("OUCH !!!");
        }

        estMort = true;
    }

    void GestionAnimations(Vector3 deplacementSol)
    {
        // Paramtre bool 'auSol' de l'animator sera equivalent a la variable bool auSol de la class
        animator.SetBool("auSol", auSol);

        /* ==================== ANIMATIONS DE DEPLACEMENT AU SOL ==================== */
        // Verifie les deplacement en X et Z seuelement
        deplacementSol.y = 0;

        animator.SetFloat("vitesse", deplacementSol.magnitude);

        if (estMort)
        {
            animator.SetBool("mort", true);
        }
    }

    /* DEBUGGAGE DES RAYCASTS */
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0f, 0.3f, 0f), 0.3f);
    }
}
