using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 
    Class de gestion de la scene d'intro du jeu :
        - Alternance affichage des btns principale et de la page d'instructions;
        - Gestion du deplacement de la camera lors du changement d'affichage;
        - Methodes de gestion de scene / application, qui serons appele lors du clique du btn approprie;
    
    Par Yanis Oulmane;
    Derniere modification: 12/12/2024;
*/


public class GmSceneIntro : MonoBehaviour
{
    [SerializeField] private GameObject affichageMain; // Rer GameObject qui parente l'affichage principale
    [SerializeField] private GameObject affichageInstructions; // Ref GameObject qui parente les textes d'instructions;

    [SerializeField] private Transform laCam; // Ref au Transform de la camera;
    [SerializeField] private Transform cibleCamera; // Ref au tranform de la cible de la camera;
    [SerializeField] private Transform posCamMain; // Ref Transform pos camera quand affiche le contenu principale du menu;
    [SerializeField] private Transform posCamInst; // Ref Transform pos camera quand affiche le les instructions;

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

    /// <summary>
    ///     Active/desactivation de l'affichage des l'elements appropries du menu et positionnement de la camera selon ce qui est affiche.
    /// </summary>
    /// <param name="i">Index action</param>
    /// <returns></returns>
    IEnumerator GestionAffichage(int i)
    {
        // Active/desactive l'affichage approprie
        // Dans boucle while, bouge la camera vers l'autre position et arret de la boucle quand cam arrive a position
        
        if (i == 0)
        {
            affichageInstructions.SetActive(false);
            while (Vector3.Distance(laCam.position, posCamMain.position) > 0.1f)
            {
                laCam.position = Vector3.MoveTowards(laCam.position, posCamMain.position, 0.1f);
                laCam.LookAt(cibleCamera);

                yield return new WaitForSeconds(1 / 60);
            }

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

            affichageInstructions.SetActive(true);
            yield break;
        }

        yield break;
    }
}
