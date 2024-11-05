using UnityEngine;

/* 
    Scripte de gestions des fonctionalites globales du jeu :
        * FONCTIONALITES A VENIR *
    
    Par Yanis Oulmane
    Derniere modification : 03/11/2024
 */
public class GAMEMANAGER : MonoBehaviour
{
    static public bool joueurCache;
    static public bool joueurMort;

    // Start is called before the first frame update
    void Start()
    {
        // Verouillage du framerate du jeu a 60fps (sinon mon ordi tente de faire tourner le jeu a 900 fps)
        Application.targetFrameRate = 60;

        // Verouillage de la souris au centre de l'ecran
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(joueurCache);
    }
}
