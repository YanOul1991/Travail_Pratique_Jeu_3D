using UnityEngine;
using UnityEngine.SceneManagement;

/* 
    Gestion des btn des scenes de fin du jeu. Fonctionne pour scene victoire et defaite.

    Par: Yanis Oulmane;
    Derniere modifiaction: 12/12/2024;
*/
public class GmSceneFin : MonoBehaviour
{   
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    ///     Lance la scene du jeu.
    /// </summary>
    public void SetRejoueur()
    {
        SceneManager.LoadScene(1);
    }


    /// <summary>
    ///     Charge la scene du menu principale.
    /// </summary>
    public void SetRetourMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    /// <summary>
    ///     Ferme le jeu.
    /// </summary>
    public void SetQuitter()
    {
        Application.Quit();
    }
}
