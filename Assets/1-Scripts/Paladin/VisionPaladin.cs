using UnityEngine;

/* 
    Scripte de gestion des visions des paladin : 
        - Distance maximale de vision
        - Regarde si un object se trouve entre la cible et paladin -> donc peut pas voir
    Par Yanis Oulmane
    Derniere modification : 09-11-2024
 */

public class VisionPaladin : MonoBehaviour
{
    [SerializeField] float distanceMax; // Distance maximal a laquel le paladin peut reperer une entite, qu'elle soit cache ou non
    private Transform cible; // Reference au component Transform de la cible
    int lesLayers; // Les layers que le Raycast analysera pour la detection;

    void Start()
    {
        cible = GameObject.FindWithTag("Player").transform;

        // Assigniation des layers qui serons pris en compte pour le Raycast de la vision
        lesLayers = 1 << LayerMask.NameToLayer("terrain") | 1 << LayerMask.NameToLayer("entite");
    }


    /* 
        > GetJoueurRepere() : 
            Methode qui verifie a l'aide d'un raycast partant du paladin 
            vers le joueur, si le joueur se trouve dans le champ de vision 
            du paladin et donc si il se fait reperer;

        > Retourne : bool;
    */
    public bool GetJoueurRepere()
    {
        Ray leRay = new (transform.position + Vector3.up, cible.position - transform.position);

        if (Physics.Raycast(leRay, out RaycastHit hitInfo, distanceMax, lesLayers))
        {
            if (hitInfo.transform == cible && !GameManager.joueurDansOmbre)
            {
                return true;
            }
        }

        return false;
    }
}