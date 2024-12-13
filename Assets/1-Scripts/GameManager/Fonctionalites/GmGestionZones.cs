using System.Collections;
using UnityEngine;

/* 
    Classe du Gamemanager qui servira a la gestion de l'environnement et des ennemies, en
    desactivant et reactivant les objets si le joueur et loin, pour aider a optimiser le jeu.

    Par: Yanis Oulmane
    Derniere modification: 12/12/2024
*/

public class GmGestionZones : MonoBehaviour
{   
    [SerializeField] private Transform joueur; // Reference au transform du joueur pour obtenir;
    [SerializeField] private float distance; // Distance a laquel le joueur devrait se trouver pour que la zone et ses elements soient actifs;

    /* ****************** IMPORTANT ****************** */
    // Pour chaque array l'index du point, et des zones devra coresspondre pour que cela fonctionne;
    [SerializeField] private Transform[] pointsZone; // position reference des zones;
    [SerializeField] private GameObject[] zonesPaladin; // Liste des GameObjects Zones qui parentent les paladins;
    [SerializeField] private GameObject[] zonesEnvironnement; // Liste de toutes les zones d'environnement;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VerificationZone());
    }

    /// <summary>
    ///     Verifie si les ennemies / elements de l'environnement sont a une certaine distance du joueur. 
    ///     Active/desactive les GameObject en fonction de la distance.
    /// </summary>
    /// <returns></returns>
    IEnumerator VerificationZone()
    {
        while (true)
        {   
            for (int i = 0; i < pointsZone.Length; i++)
            {   
                // Si les zones sont dans la bonne distance 
                // Active les GameObject
                // Sinon les desactive
                if (Vector3.Distance(pointsZone[i].position, joueur.position) < distance)
                {
                    zonesPaladin[i].SetActive(true);
                    zonesEnvironnement[i].SetActive(true);
                }
                else
                {
                    zonesPaladin[i].SetActive(false);
                    zonesEnvironnement[i].SetActive(false);
                }
            }
            // Verification 1 fois par seconde
            yield return new WaitForSeconds(1);
        }
    }

}