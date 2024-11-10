using UnityEngine;

/* 
    Scripte de gestions des fonctionalites globales du jeu :
        - Memoriser les etats du joueurs
    
    Par Yanis Oulmane
    Derniere modification : 06/11/2024
 */
public class GameManager : MonoBehaviour
{
    static public bool joueurVisible;
    static public bool joueurVivant;

    // Start is called before the first frame update
    void Start()
    {
        // Verouillage du framerate du jeu a 60fps (sinon mon ordi tente de faire tourner le jeu a 900 fps)
        Application.targetFrameRate = 60;

        // Verouillage de la souris au centre de l'ecran
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log(" 3 % 5 = " + 3 % 5);
    }
}
