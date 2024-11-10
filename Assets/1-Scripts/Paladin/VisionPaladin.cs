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
    [SerializeField] Transform cible; // Reference au component Transform de la cible
    [SerializeField] float distanceMax; // Distance maximal a laquel le paladin peut reperer une entite, qu'elle soit cache ou non
    [SerializeField] bool joueurVisible; // Bool memorisant si le joueur est visible
    int lesLayers; // Les layers que le Raycast analysera pour la detection (optimisation) 

    void Start()
    {
        // Definition des layers qui serons pris en compte pour le Raycast de la vision
        lesLayers = 1 << LayerMask.NameToLayer("terrain") | 1 << LayerMask.NameToLayer("entite");
    }

    void FixedUpdate()
    {
        /* 
            
        */
        // Regarder si le joueur se trouve a une certaine distance du paladin;
        // if true -> creer un Raycast entre paladin et cible.
        // if false -> joueur est automatiquement pas dans le champ de vision
        if (Vector3.Distance(transform.position, cible.position) <= distanceMax)
        {
            RaycastHit collision;
            Ray leRay = new Ray(transform.position + Vector3.up, cible.position - transform.position);

            if (Physics.Raycast(leRay, out collision, distanceMax, lesLayers))
            {
                // Si le le point de collision du ray est la position de la cible
                // Alors le joueur est dans le champ de vision du paladin
               joueurVisible = collision.transform == cible ? true : false;

                /* ------------------------- Debug du Raycast ------------------------- */
                Debug.DrawRay(leRay.origin, leRay.direction * Vector3.Distance(transform.position + Vector3.up, collision.point), Color.yellow);
            }
        }
        else
        {
            joueurVisible = false;
        }
    }

    /* Fonction retournant la valeur de la variable joueurVisible */
    public bool JoueurDansVision()
    {
        return joueurVisible;
    }
}