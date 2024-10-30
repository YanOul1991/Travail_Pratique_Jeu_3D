using UnityEngine;
public class GestionPaladin : MonoBehaviour
{
    public float vitesseMarche; // Vitesse de marche du personnage
    public float forceSaut; // Force de saut du personnage
    public float forceGravite;

    // Start is called before the first frame update
    void Start()
    {
        // Verouille le curseur au centre de l'ecran
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetBool("marche", GetComponent<Rigidbody>().velocity.magnitude > 1f ? true : false);
    }

    void FixedUpdate()
    {
        // Applique les force de deplacement avant et arriere

        GetComponent<Rigidbody>().velocity = transform.TransformDirection(Input.GetAxisRaw("Horizontal"), forceSaut, Input.GetAxisRaw("Vertical")).normalized * vitesseMarche;
    }
}
