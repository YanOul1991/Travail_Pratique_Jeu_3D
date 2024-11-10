using UnityEngine;

public class PatrouillePaladin : MonoBehaviour
{
    [SerializeField] Transform[] pointsPatrouille; // Arrays de transform qui aura les points par lesquels il passera;
    int indexPointActuel; // Index du point vers lequel le Paladin se dirigera

    // Start is called before the first frame update
    void Start()
    {
        indexPointActuel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public Transform DestinationDeplacement()
    {   
        indexPointActuel = indexPointActuel + 1 % pointsPatrouille.Length;

        return pointsPatrouille[indexPointActuel];
    }
}
