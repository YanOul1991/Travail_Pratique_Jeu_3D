using UnityEngine;

/* 
    Scripte de gestions des fonctionalites globales du jeu :
        - Memoriser les etats du joueurs
        - Mini optimisation de la performance du GPU

    Par Yanis Oulmane
    Derniere modification : 12/12/2024
*/

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    static public bool joueurCache = true;
    static public bool joueurVivant = true;

    // Start is called before the first frame update
    void Awake()
    {
        // Verouillage de la souris au centre de l'ecran
        // et du framerate du jeu
        Cursor.lockState = CursorLockMode.Locked;

        // Optimisation en limitant le refresh rate a 60fps et changeant la qualite des graphismes
        // pour eviter d'en demander trop au GPU pour rien
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        QualitySettings.SetQualityLevel(3);
    }
}
