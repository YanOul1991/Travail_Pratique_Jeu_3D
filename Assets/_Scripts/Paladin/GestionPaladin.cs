using UnityEngine;

/* 


 */

public class GestionPaladin : MonoBehaviour
{
    /* =================================================================================== */
    /* ==================================== VARIABLES ==================================== */
    /* =================================================================================== */

    public float vDeplac; // Variable public float memorisant la vitesse de deplacement du personnage

    public GameObject leRig; 

    // Start is called before the first frame update
    void Start()
    {
        // Verouille le curseur au centre de l'ecran
        // Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {   
        // Applique les vitesses de deplacement du personnage
        leRig.GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxisRaw("Horizontal"), leRig.GetComponent<Rigidbody>().velocity.y, Input.GetAxisRaw("Vertical")).normalized * vDeplac;

        print(Input.GetAxisRaw("Horizontal"));
        print(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("attaque");
        }

        gameObject.transform.position = leRig.transform.position;
    }
}
