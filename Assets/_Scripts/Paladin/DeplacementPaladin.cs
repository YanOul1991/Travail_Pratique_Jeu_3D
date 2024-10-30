using UnityEngine;

public class DeplacementPaladin : MonoBehaviour
{
    public float VitesseDeplacement;
    public float VitesseTourne;

    /* ================================= VARIABLES REFERENCES COMPONENTS ================================= */
    private CharacterController controleur; // Variable private CharcterController pour faire plus rapidement reference au character controller du personnage

    // Start is called before the first frame update
    void Start()
    {
        // Assignation des Components
        controleur = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Cherche les valeurs de l'axe vertical et horizontal
        Vector3 valeursInput = Vector3.zero;
        valeursInput.x = Input.GetAxis("Horizontal");
        valeursInput.z = Input.GetAxis("Vertical");
        
        // Applique les forces de deplacement aux axes du monde
        Vector3 deplacement = transform.TransformDirection(valeursInput * VitesseDeplacement);

        controleur.SimpleMove(deplacement);

        // Faire tourner le personnage en fonction du deplacement horizontal de la souris
        transform.Rotate(0f, Input.GetAxis("Mouse X") * VitesseTourne, 0f);
    }
}
