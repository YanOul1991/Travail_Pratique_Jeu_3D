using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GmSceneIntro : MonoBehaviour
{
    [SerializeField] private GameObject affichageMain;
    [SerializeField] private GameObject affichageInstructions;

    [SerializeField] private Transform laCam;
    [SerializeField] private Transform cibleCamera;
    [SerializeField] private Transform posCamMain;
    [SerializeField] private Transform posCamInst;

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Cursor.lockState = CursorLockMode.Confined;
        
        // Etats defaut de la camera
        affichageMain.SetActive(true);
        affichageInstructions.SetActive(false);
        laCam.position = posCamMain.position;
        laCam.transform.LookAt(cibleCamera);
    }

    public void SetToggleAffichage(int instruction)
    {
        StartCoroutine(GestionAffichage(instruction));
    }

    public void SetCommencerJeu()
    {
        // Commence le jeu
        SceneManager.LoadScene(1);
    }

    public void SetQuitter()
    {
        Application.Quit();
    }

    IEnumerator GestionAffichage(int i)
    {
        if (i == 0)
        {
            affichageInstructions.SetActive(false);
            while (Vector3.Distance(laCam.position, posCamMain.position) > 0.1f)
            {
                laCam.position = Vector3.MoveTowards(laCam.position, posCamMain.position, 0.1f);
                laCam.LookAt(cibleCamera);

                yield return new WaitForSeconds(1 / 60);
            }

            // laCam.position = posCamMain.position;
            // laCam.LookAt(cibleCamera);

            affichageMain.SetActive(true);
            yield break;
        }

        if (i == 1)
        {
            affichageMain.SetActive(false);
            while (Vector3.Distance(laCam.position, posCamInst.position) > 0.1f)
            {
                laCam.position = Vector3.MoveTowards(laCam.position, posCamInst.position, 0.1f);
                laCam.LookAt(cibleCamera);

                yield return new WaitForSeconds(1 / 60);
            }

            // laCam.position = posCamInst.position;
            // laCam.LookAt(cibleCamera);

            affichageInstructions.SetActive(true);
            yield break;
        }

        yield break;
    }
}
