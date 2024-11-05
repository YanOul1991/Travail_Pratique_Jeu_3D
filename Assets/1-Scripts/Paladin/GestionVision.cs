using UnityEngine;

public class GestionVision : MonoBehaviour
{
    [SerializeField] Transform cible;

    // Start is called before the first frame update

    void FixedUpdate()
    {
        RaycastHit collision;
        Ray leRay = new Ray(transform.position + Vector3.up, cible.position - transform.position);

        if (Physics.Raycast(leRay, out collision, 5000))
        {
            Debug.DrawRay(leRay.origin, leRay.direction * Vector3.Distance(transform.position + Vector3.up, collision.transform.position), Color.yellow);

            // Si le le point de collision du ray est la position de la cible
            // Alors le joueur est dans le champ de vision du paladin
            if (collision.transform == cible)
            {   
                GetComponent<PaladinDeplacement>().joueurDansVision = true;
                // Debug.Log("Joueur Repere");
            }
            else
            {
                GetComponent<PaladinDeplacement>().joueurDansVision = false;
                // Debug.Log("Joueur cache");
            }

        }
    }
}
