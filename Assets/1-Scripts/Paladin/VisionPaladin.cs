using UnityEngine;

/* 
    Scripte de gestion des visions des paladin : 
        - Gestion champ de vision (FOV)
    Par Yanis Oulmane
    Derniere modification : 02-12-2024
 */

public class VisionPaladin : MonoBehaviour
{
    [SerializeField] float distanceMax; // Distance maximal a laquel le paladin peut reperer une entite, qu'elle soit cache ou non
    [SerializeField] float champVision; // Angle de detection maximal du paladin
    private Transform cible; // Reference au component Transform de la cible
    private int lesLayers; // Les layers que le Raycast analysera pour la detection;

    void Start()
    {
        cible = GameObject.FindWithTag("Player").transform;
        
        // Assigniation des layers qui serons pris en compte pour le Raycast de la vision
        lesLayers = 1 << LayerMask.NameToLayer("terrain") | 1 << LayerMask.NameToLayer("entite");
    }

    /// <summary>
    ///     Methode qui verifie si le joueur est dans la vision du joueur et donc il se fait repere a laide d'un Raycast.
    /// </summary>
    /// 
    /// <returns>
    ///     Retourne true si le collider de la collision est celui du joueur. Sinon retourne false
    /// </returns>
    public bool GetJoueurRepere()
    {   
        // Si le joueur est trop loin ou qu'il n'est pas dans le 
        // champ de vision, retourne tout de suite false.

        if (Vector3.Distance(transform.position, cible.transform.position) > distanceMax) return false;
        if (Vector3.Angle(cible.transform.position - transform.position, transform.forward) > champVision/2) return false;

        Ray leRay = new (transform.position + Vector3.up, cible.position - transform.position);

        if (Physics.Raycast(leRay, out RaycastHit hitInfo, distanceMax, lesLayers))
        {
            if (hitInfo.transform == cible && !GameManager.joueurDansOmbre) return true;
        }

        return false;
    }
}