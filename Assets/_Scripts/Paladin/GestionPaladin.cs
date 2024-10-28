using UnityEngine;
/* 


 */

public class GestionPaladin : MonoBehaviour
{
    /* =================================================================================== */
    /* ==================================== VARIABLES ==================================== */
    /* =================================================================================== */
    public float vDeplac; // Variable public float memorisant la vitesse de deplacement du personnage

    // Start is called before the first frame update
    void Start()
    {
        // Verouille le curseur au centre de l'ecran
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {   
        // Applique les vitesses de deplacement du personnage
        GetComponent<Rigidbody>().velocity = new Vector3(Input.GetAxisRaw("Horizontal"), GetComponent<Rigidbody>().velocity.y, Input.GetAxisRaw("Vertical")).normalized * vDeplac;

        GetComponent<Animator>().SetBool("marche", GetComponent<Rigidbody>().velocity.magnitude > 5 ? true : false);

        // Clique droit souris
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Set le trigger de l'attaque
            GetComponent<Animator>().SetTrigger("attaque");
        }

    }
}
