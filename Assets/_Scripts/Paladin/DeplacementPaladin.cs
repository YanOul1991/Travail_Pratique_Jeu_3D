using UnityEngine;

public class DeplacementPaladin : MonoBehaviour
{
    /* ================================================================================================= */
    /* ================================== VARIABLES POUR DEPLACEMENTS ================================== */
    /* ================================================================================================= */
    public float vitesseMarche; // Vitesse de deplacement;
    public float vitesseSprint; // Vitesse du sprint du personnage;
    float vitesseReel; // Vitesse qui sera applique au personnage
    public float forceSaut; // Force du saut
    public float forceGravite; // Force de la gravite
    float velociteY; // Velocite en Y 
    bool saut; // Bool memorisant si le personnage a saute
    bool auSol; // Bool memorisant si le personnage est au sol

    /* =================================================================================================== */
    /* ================================= VARIABLES REFERENCES COMPONENTS ================================= */
    /* =================================================================================================== */
    private CharacterController controleur; // Variable private CharcterController pour faire plus rapidement reference au character controller du personnage

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Assignations des references aux components du personnage
        controleur = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            saut = true;
        }

        GetComponent<Animator>().SetBool("saut", !auSol);
        
        // La vitesse reel qui sera applique au personnage dependra de selon si la touche shift est maintenu
        // Si oui la vitesse sera celle du sprint, sinon sera celle de la marche;
        vitesseReel = Input.GetKey(KeyCode.LeftShift) ? vitesseSprint : vitesseMarche;
    }

    void FixedUpdate()
    {
        Vector3 deplacement = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * vitesseReel);

        // Verifie si le personnage est au sol avec un SphereCast
        RaycastHit infoCollision;
        auSol = Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.5f, Vector3.down, out infoCollision, 0.5f);


        // Si le personnage touche au sol et que la velocite est plus petit que 0
        if (auSol && velociteY < 0)
        {
            // Remet la velocite en Y a 0
            velociteY = 0f;
        }

        // Si active le saut et que le personnage touche au saul
        if (saut && auSol)
        {
            // Applique comme velocite en y la force de saut
            velociteY = forceSaut;
            saut = false;
        }

        // Applique la force de gravite a la velociteY du personnage a chaque seconde pour lui permetttre de retomber
        velociteY += forceGravite * Time.deltaTime;

        // Applique la velocite en Y au deplacement du personnage
        deplacement.y = velociteY;

        // Applique lews deplacements du personnage multiplie par Time.deltaTime pour un variation constante
        controleur.Move(deplacement * Time.deltaTime);

        GestionAnimations(deplacement);
    }

    void GestionAnimations(Vector3 valeurDeplacement)
    {
        // Applique un valeur de zero a la valeur du deplacement en y
        // pour seulement verifier les mouvement sur les axes X et Y
        valeurDeplacement.y = 0;

        if (vitesseReel == vitesseMarche)
        {
            GetComponent<Animator>().SetBool("sprint", false);

            // Set le bool soit a true ou false selon si la valeur de deplacement sur les axes x et z est superieur a 1f
            GetComponent<Animator>().SetBool("marche", valeurDeplacement.magnitude > 1f ? true : false);
        }

        if (vitesseReel == vitesseSprint)
        {
            GetComponent<Animator>().SetBool("marche", false);

            // Set le bool soit a true ou false selon si la valeur de deplacement sur les axes x et z est superieur a 1f
            GetComponent<Animator>().SetBool("sprint", valeurDeplacement.magnitude > 1f ? true : false);
        }
    }
}
