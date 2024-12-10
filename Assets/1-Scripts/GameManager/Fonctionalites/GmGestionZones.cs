using System.Collections;
using UnityEngine;

/* 
    Classe du Gamemanager qui servira a la gestion de l'environnement et des ennemies, en
    desactivant et reactivant les objets si le joueur et loin, pour aider a optimiser le jeu.

    Par: Yanis Oulmane
    Derniere modification: 20/12/2024
*/

public class GmGestionZones : MonoBehaviour
{
    [SerializeField] private Transform joueur; // Reference au transform du joueur pour obtenir;
    [SerializeField] private float distance; // Distance a laquel le joueur devrait se trouver pour que la zone et ses elements soient actifs;
    [SerializeField] private Transform[] pointsZone; // position reference des zones;
    [SerializeField] private GameObject[] zonesPaladin; // Liste des GameObjects Zones qui parentent les paladins;
    [SerializeField] private GameObject[] zonesEnvironnement; // Liste de toutes les zones d'environnement;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(pointsZone.Length);
        // Debug.Log(zonesPaladin.Length);
        // Debug.Log(zonesEnvironnement.Length);
    }

    IEnumerator VerificationZone() 
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
        }
    }
    
}
