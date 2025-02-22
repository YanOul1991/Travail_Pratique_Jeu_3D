using UnityEngine;
using UnityEngine.SceneManagement;
/* 
    Script des gestion des deplacements du joueur ayant les fonctionalites suivantes : 
        - Gestion des etats du joueur (cache/visible, vivant/mort, etc.) et partage ces information avec le gamemanager;
        - Gestion des etats physiques du personnages tels que debout/accroupi et marche/sprint;
        - Gestion des vitesses de deplacement selon l'etat du physique du personnage;
        - Gestion des sauts;
        - Gestion des animations du personnage selons les etats;
        - Gestion des collisions;

    Par : Yanis Oulmane
    Derniere modification : 06/11/2024
 */
public class DeplacementErika : MonoBehaviour
{
    [SerializeField] float vitesseMarche; // Vitesse du deplacement de type marche
    [SerializeField] float vitesseSprint; // Vitesse du deplacement de type sprint
    [SerializeField] float vitesseAccroupi; // Vitesse du deplacement lorsque le joueur est en etat accroupi
    [SerializeField] float forceSaut; // Force de saut du joueur
    [SerializeField] float forceGravite; // Force de gravite qui sera applique au joueur
    [SerializeField] Transform laCamera; // Reference au component Transform de la camera
    float vitesseReel; // Vitesse qui sera applique au personnage selon son etat
    float mouvHorizontal; // Variable memorisant la Velocite en X du personnage
    float mouvVertical; // Variable memorisant la Velocite en Y du personnage
    float velociteY; // Variable memorisant la velocite Y du joueur en temps reel;
    bool auSol; // Bool memorisant si le personnage touche au sol
    [SerializeField] bool estCache; // Bool memorisant si le personnage est cache
    [SerializeField] bool estVivant = true; // Bool memorisant si le personnag est mort

    [SerializeField] AudioClip sonDeplacement; // Son de bruit de pas du personnage
    [SerializeField] AudioClip sonEpee; // Son de l'epee quand le personnage se fait touche

    /* References aux autres classes du personnage */
    CharacterController controleur;
    Animator animator;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Assigniation des references aux components
        controleur = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Assigniation de valeurs par defaut
        estCache = true;
    }

    // Fonction Update() gere les Inputs du joueurs et les etats du personnage  
    void Update()
    {
        GameManager.joueurCache = estCache;
        GameManager.joueurVivant = estVivant;

        // Ecoute les inputs du joueur seulment si il est en vie;
        if (estVivant)
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
        else
        {
            // Si le personnage est mort la vitesse de deplacement envoyee sera automatiquement de 0
            vitesseReel = 0;
        }
    }

    void FixedUpdate()
    {
        // Verification du contacte avec le sol avec un spherecast place aux pieds du personnage
        RaycastHit collisionSphereCast;
        auSol = Physics.SphereCast(transform.position + new Vector3(0f, 0.3f, 0f), 0.3f, Vector3.down, out collisionSphereCast, 0.3f);

        // Permet les modifications des deplacement en X et Z du personnage seuelemt si il touche au sol
        // Fix la velocite en Y a la force de gravite, pour evite qu'elle continue de trop baisser
        if (auSol && estVivant)
        {
            mouvHorizontal = Input.GetAxisRaw("Horizontal");
            mouvVertical = Input.GetAxisRaw("Vertical");

            if (velociteY < forceGravite)
            {
                velociteY = 0;
            }
        }

        /* 
            APPLICATION DES MOUVEMENTS AUX PERSONNAGE SELON L'ANGLE DE LA CAMERA : 
                - Transforme les vecteurs de la camera en vecteur gloable
                - Change le vecteur en Y a 0 pour que le vecteur sultant soit parfaitement a l'horizontal
                - Et normalize le vecteur pour qu'il pointe dans la bonne direction avec un valeur de 1
        */

        Vector3 mouvRelatif = laCamera.forward * mouvVertical + laCamera.right * mouvHorizontal;
        mouvRelatif.y = 0;
        mouvRelatif = mouvRelatif.normalized * vitesseReel;

        // Le personnage regardera vers la direction approprie 
        // Si il est au sol et qu'il y a un deplacement
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

        // Gestion des bruits de pas du joueur
        float deplacementSol = new Vector3(mouvRelatif.x, 0, mouvRelatif.z).magnitude;

        // Modification des sons de bruit de pas du joueur
        // selon si il est au sol et la magnitude de son deplacement au sol
        if (estVivant)
        {
            if (!auSol || deplacementSol < 1)
            {
                // Si n'est pas au sol, il ne fait aucun bruit de pas
                audioSource.volume = 0;
            }
            else
            {

                if (deplacementSol < vitesseSprint)
                {
                    audioSource.volume = 0.5f;
                    audioSource.pitch = 0.8f;
                }
                else
                {
                    audioSource.volume = 0.9f;
                    audioSource.pitch = 1.85f;
                }
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        // Lorsque le joueur se fait frappe par l'epee d'un paladin 
        // le joueur meurt
        if (collision.gameObject.name == "epee" && estVivant)
        {
            estVivant = false;

            animator.SetTrigger("mortTrigger");

            audioSource.resource = null;
            audioSource.volume = 1;
            audioSource.pitch = 1;
            audioSource.PlayOneShot(sonEpee);

            Invoke(nameof(SetSceneDefaite), 3f);
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene(2);
        }
    }

    void OnTriggerStay(Collider collision)
    {
        // lorsque le personnage reste dans le Trigger d'un object qui possede le tag 'lumiereRevele'
        // Il n'est plus cache et donc le rend suseptible d'etre vu par les ennemies
        if (collision.gameObject.tag == "lumiereRevele")
        {
            estCache = false;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        // Si il quite toutes les zones qui genere de la lumiere
        // Le personnage redevient cache
        if (collision.gameObject.tag == "lumiereRevele")
        {
            estCache = true;
        }
    }

    void GestionAnimations(Vector3 deplacementSol)
    {
        // Paramtre bool 'auSol' de l'animator sera equivalent a la variable bool auSol de la class
        animator.SetBool("auSol", auSol);

        /* ==================== ANIMATIONS DE DEPLACEMENT AU SOL ==================== */
        // Verifie les deplacement en X et Z seuelement
        deplacementSol.y = 0;

        animator.SetFloat("vitesse", deplacementSol.magnitude);
    }

    public bool GetJoueurCache()
    {
        return estCache;
    }

    void SetSceneDefaite()
    {
        SceneManager.LoadScene(3);
    }
}
