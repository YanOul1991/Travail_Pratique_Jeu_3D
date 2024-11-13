using UnityEngine;

/* 
    Scripte memorisant des infos globales du jeu en cours.
*/
public class InfosGlobales : MonoBehaviour
{
    [SerializeField] ErikaManager infosErika;


    public bool EtatVieJoueur()
    {
        return infosErika.EnVie();
    }

    public bool EtatCacheJoueur()
    {
        return infosErika.Cache();
    }
}
