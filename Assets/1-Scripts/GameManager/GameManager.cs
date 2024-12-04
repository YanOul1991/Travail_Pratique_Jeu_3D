using UnityEngine;
/* 
    Scripte de gestions des fonctionalites globales du jeu :
        - Memoriser les etats du joueurs
    
    Par Yanis Oulmane
    Derniere modification : 06/11/2024
 */
public class GameManager : MonoBehaviour
{
    static public bool joueurDansOmbre = true;
    static public bool joueurVivant = true;

    // Start is called before the first frame update
    void Start()
    {
        // Verouillage de la souris au centre de l'ecran
        // et du framerate du jeu
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }
}
