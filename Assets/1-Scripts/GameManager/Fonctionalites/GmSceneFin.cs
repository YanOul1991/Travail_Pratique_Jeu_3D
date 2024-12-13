using UnityEngine;
using UnityEngine.SceneManagement;

public class GmSceneFin : MonoBehaviour
{   
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetRejoueur()
    {
        SceneManager.LoadScene(1);
    }
    public void SetRetourMenu()
    {
        SceneManager.LoadScene(0);
    }

    // public void SetQuitter()
    // {
    //     Application.Quit();
    // }
}
