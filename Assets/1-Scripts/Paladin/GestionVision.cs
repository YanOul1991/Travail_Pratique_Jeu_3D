using UnityEngine;

public class GestionVision : MonoBehaviour
{
    [SerializeField] Transform cible; // Reference au component Transform de la cible
    [SerializeField] float distanceMax; // Distance maximal a laquel le paladin peut reperer une entite, qu'elle soit cache ou non

    void FixedUpdate()
    {
        // Regarder si le joueur se trouve a une certaine distance du paladin;
        // if true -> creer un Raycast entre paladin et cible.
        // if false -> joueur est automatiquement pas dans le champ de vision
        if (Vector3.Distance(transform.position, cible.position) <= distanceMax)
        {
            RaycastHit collision;
            Ray leRay = new Ray(transform.position + Vector3.up, cible.position - transform.position);

            if (Physics.Raycast(leRay, out collision, distanceMax))
            {
                Debug.DrawRay(leRay.origin, leRay.direction * Vector3.Distance(transform.position + Vector3.up, collision.point), Color.yellow);

                // Si le le point de collision du ray est la position de la cible
                // Alors le joueur est dans le champ de vision du paladin
                GetComponent<PaladinDeplacement>().joueurDansVision = collision.transform == cible ? true : false;
            }
        }
        else
        {
            GetComponent<PaladinDeplacement>().joueurDansVision = false;
        }
    }
}
