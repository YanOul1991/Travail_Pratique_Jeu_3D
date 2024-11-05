using UnityEngine;

/* 
    Script de gestion de la camera du jeu, gerant les fonctionalites suivantes : 
        - Bla bla bla

    Par Yanis Oulmane
    Derniere modification : 02/11/2024
 */
public class GestionCamera : MonoBehaviour
{
    [field: SerializeField] Transform cible; // Reference au component Transform de la cible a suivre
    [field: SerializeField] Transform pivotCam; // Reference au component Transform du parent de la camera qui sera la pivot
    [field: SerializeField] float distanceCamera; // float memorisant la distance souhaite de la camera par rapport a la cible

    // Update is called once per frame
    void Update()
    {
        // Memorise les vecteurs en X et en Y des mouvement de la souris
        float sourisX = Input.GetAxis("Mouse X");
        float sourisY = Input.GetAxis("Mouse Y");

        // Applique les mouvements de rotation de la souris au pivot
        pivotCam.Rotate(-sourisY, sourisX, 0f);

        pivotCam.localEulerAngles = new Vector3(pivotCam.localEulerAngles.x, pivotCam.localEulerAngles.y, 0f);

        // Positionnement du pivot et de la camera
        pivotCam.position = cible.position + Vector3.up;

        transform.position = pivotCam.position - transform.forward * distanceCamera;

        transform.LookAt(pivotCam);
    }
}
