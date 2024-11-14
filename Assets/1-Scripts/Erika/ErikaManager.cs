using UnityEngine;

/* 
    Scripte de Gestion des infos globales du joueur
*/

public class ErikaManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager; // Reference au game manager

    // Variables globales privees
    private bool estVivant = true;
    private bool estCache = true;

    /* FONCTIONS DE MODIFICATIONS D'ETATS */
    public void DansLumiere()
    {
        estCache = false;
    }

    public void SortLumiere()
    {
        estCache = true;
    }

    public void ImpacteEpee()
    {
        estVivant = false;
    }

    /* FONCTIONS DE PARTAGES D'ETATS */
    public bool EnVie()
    {
        return estVivant;
    }
    public bool Cache()
    {
        return estCache;
    }
}
