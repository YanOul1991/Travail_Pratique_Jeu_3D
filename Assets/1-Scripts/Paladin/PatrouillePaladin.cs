using UnityEngine;

/* 
    Scripte de gestion des pattern de patrouille d'un paladin fonctionnant de la facon suivante : 
        -   Chaque paladin aura un enfant GameObject nomme 'Points';
        -   Cette enfant aura un nombre variable de points par lesquelles le paladins patrouillera;
        -   Recupere la position de chaque point enfant du GameObject 'Points';
        -   Place chaque position dans un Vector3[];
        -   Une methode publique incrementera le point de destination qui sera appele dans DeplacementPaladin quand necessaire;

        ********************************** IMPORTANT ********************************** 
        -   Pour que le paladin se dirige correctement vers un point de patrouille,
            ce point doit se trouver dans l'area 'Route' (en rouge) du NavMesh;

        -   L'ordre de chaque point du GameObject 'Points' dans la hierarchie est important,
            celui tout en haut sera le premier dans l'array; 

        -   Le premier point de la serie sera toujours a la position du paladin;

    Par : Yanis Oulmane
    Derniere modification : 14-11-2024
*/

public class PatrouillePaladin : MonoBehaviour
{
    private Vector3[] positions; // Arrays de Vector3 qui momoriseront toutes les positions de debart du array lesPoints[];
    private int indexPointActuel = 0; // Index du point vers lequel le Paladin se dirigera, commence a 0;

    void Start()
    {
        // Trouve l'enfant nomme 'Points' du paladin et assigne sa reference dans une variable;
        // Initialise positions[] et assigne comme lenght le nb d'enfants de 'Points';
        // Assigne chaque position des enfant dans positions[], a l'index equivalent;
        // Le premier point de la serie sera toujours la positino intiale du paladin
        
        GameObject lesPoints = transform.Find("Points").gameObject;

        positions = new Vector3[lesPoints.transform.childCount];

        for (int i = 0; i < lesPoints.transform.childCount; i++)
        {
            positions[i] = lesPoints.transform.GetChild(i).transform.position;
        }

        // Destroy(lesPoints);

        positions[0] = transform.position;
    }
 
    /// <summary>
    ///     Met a jour le point de patrouille du paladin.
    /// </summary>
    /// <returns>Position du prochain point de patrouille du paladin.</returns>
    public Vector3 GetProchainPos()
    {
        /*  
            Incremente d'abord l'index de position 
            Verifie si rendu au dernier point de la liste, si oui, retourne au premier
            Retourne la position du points
        */  

        indexPointActuel ++;

        if (indexPointActuel == positions.Length)
        {
            indexPointActuel = 0;
        }

        return positions[indexPointActuel];
    }
}