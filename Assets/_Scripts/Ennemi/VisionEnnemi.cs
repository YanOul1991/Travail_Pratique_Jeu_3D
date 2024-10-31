using UnityEngine;

public class VisionEnnemi : MonoBehaviour
{
    public GameObject cible; // Cible que le personnage regardera
    public float distanceVision; // Distance maximal ou l'ennemi peut voir le joueur
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pointOrigine = transform.position;
        Vector3 pointCible = cible.transform.position;

        pointOrigine.y += 2;
        pointCible.y += 2; 

        Vector3 direction = pointCible - pointOrigine;
        
        Ray leRay = new Ray(transform.position, direction);
        RaycastHit hit;
        
        if (Physics.Raycast(leRay.origin, leRay.direction, out hit, 5000))
        {
            Debug.DrawRay(leRay.origin, leRay.direction * 100, Color.yellow);
        }
    }
}
