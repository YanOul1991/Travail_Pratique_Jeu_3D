using UnityEngine;

public class CameraSuivit : MonoBehaviour
{
    public GameObject objCamera; // Reference a l'objet camera
    public Vector3 posCamera; // Distance de la camera par rapport au personnage

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Position la camera par rapport au personnage
        objCamera.transform.position = transform.position + posCamera;

        // Oriente la camera pur qu'elle regarde toujours le personnage
        objCamera.transform.LookAt(transform.position);
    }
}
