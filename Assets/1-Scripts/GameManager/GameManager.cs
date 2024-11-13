using UnityEngine;

/* 
    Scripte de gestions des fonctionalites globales du jeu :
        - Memoriser les etats du joueurs
    
    Par Yanis Oulmane
    Derniere modification : 06/11/2024
 */
public class GameManager : MonoBehaviour
{
    [SerializeField] Transform personnageTransform; // Reference au component Transform du personnage;
    static public bool joueurVisible;
    static public bool joueurVivant;

    // Start is called before the first frame update
    void Start()
    {
        // Verouillage de la souris au centre de l'ecran
        Cursor.lockState = CursorLockMode.Locked;

        // Verouillage du framerate du jeu a 60fps (sinon mon ordi tente de faire tourner le jeu a 900 fps)
        Application.targetFrameRate = 60;
    }

    /* FONCTIONS RETOURNANTS INFOS */
    public bool JoueurEnVie()
    {
        return joueurVivant;
    }
    public bool JoueurEstVisible()
    {
        return joueurVisible;
    }
}
