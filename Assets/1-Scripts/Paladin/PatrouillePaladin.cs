using UnityEngine;

/* 
    Scripte de gestion des pattern de patrouille d'un paladin avec les etapes suivantes : 
        - Definit un Serialized Transform[] ou tout les points par lesquel le paladin passera seront manuellement place, et seronts
        place comme enfant du paladin.

        - ***** L'ORDRE DES POINTS EST IMPORTANT *****

        - Les points seront place comme enfant du paladin.

        - Un private Transform[] sera tout les elements du premier array dans la methode void Start() pour que les Transform des points
        restent statique dans le monde et ne suivent plus le paladin.

        - La script possede aussi une methode public qui itere l'index du point de deplacement qui sera memorise dans une variable et qui retourne 
        le Transform du point a l'index correspondant. Cet methode sera appele dans la class DeplacementPaladin quand necessaire.

    Par : Yanis Oulmane
    Derniere modification : 11-11-2024
*/

public class PatrouillePaladin : MonoBehaviour
{
    [SerializeField] Transform[] lesPoints; // Arrays de transform qui aura les points par lesquels il passera;
    private Transform[] positionsPoints; // private Transform[] morisant les positions de 
    private int indexPointActuel; // Index du point vers lequel le Paladin se dirigera

    // Start is called before the first frame update
    void Start()
    {
        indexPointActuel = 0;

        positionsPoints = lesPoints;
    }

    public Transform DestinationDeplacement()
    {   
        indexPointActuel = indexPointActuel + 1 % positionsPoints.Length;

        return positionsPoints[indexPointActuel];
    }
}