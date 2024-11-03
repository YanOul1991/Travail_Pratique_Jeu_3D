using UnityEngine;

public class CameraSuivit : MonoBehaviour
{
    public GameObject objCamera; // Reference a l'objet camera
    public Vector3 posCamera; // Distance de la camera par rapport au personnage
    public float VitesseTourne;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        objCamera.transform.RotateAround(transform.position,  Vector3.up, Input.GetAxis("Mouse X") * VitesseTourne);

        // Position la camera par rapport au personnage
        // objCamera.transform.position = transform.TransformPoint(posCamera);

        // Oriente la camera pur qu'elle regarde toujours le personnage
        objCamera.transform.LookAt(transform.position);
    }
}
