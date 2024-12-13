using UnityEngine;
using UnityEngine.AI;
/* 
    Classe qui possede les methodes permettant de deplacer le paladin qui se deplace grace
    a un NavMeshAgent component. Classe gere les fonctionalites suivantes: 

        - Vitesse de deplacement;
        - Modification des area du NavMesh ou le paladin se deplace;
        - Destination du paladin avec SetDestination();

    Par Yanis Oulmane
    Derniere modification : 30-11-2024
 */
 
public class DeplacementPaladin : MonoBehaviour
{
    /* ================================ VARIABLES ================================ */
    [SerializeField] private float vitesseMarche; // Variable memorisant la vitesse de marche du paladin;
    [SerializeField] private float vitesseCourse; // Variable memorisant la vitesse de course du paladin;\
    private GameObject joueur; // Reference au joueur;
    private PaladinManager paladinManager; // Reference a la classe PaladinManager
    private NavMeshAgent navAgent; // References au component NavMeshAgent du paladin;

    /* =========================================================================== */

    void Start()
    {
        joueur = GameObject.FindWithTag("Player");
        // Assigniation des references de components et scripts 
        paladinManager = GetComponent<PaladinManager>();
        navAgent = GetComponent<NavMeshAgent>();

        InvokeRepeating(nameof(SetSonsDeplacement), 0, 0.15f);
    }

    /* ============================== METHODES ============================== */

    /// <summary>
    ///     Met a jour les proprietes du NavMeshAgent du paladin en fonction de l'etat dans lequel il se trouve.
    /// </summary>
    public void SetContraintes()
    {
        // Proprietes du NavMeshAgent a mettre a jour :
        //      - areaMAsk;
        //      - speed;
        //      - stoppingDistance;

        switch (paladinManager.GetEtatPaldin())
        {
            case PaladinManager.EtatsPaladin.PATROUILLE:
                navAgent.areaMask = 1 << NavMesh.GetAreaFromName("Route");
                navAgent.speed = vitesseMarche;
                navAgent.stoppingDistance = 0;
                break;

            case PaladinManager.EtatsPaladin.INSPECTION:
                navAgent.areaMask = NavMesh.AllAreas;
                navAgent.speed = vitesseMarche;
                navAgent.stoppingDistance = 0;
                break;
                
            case PaladinManager.EtatsPaladin.CHASSE:
                navAgent.areaMask = NavMesh.AllAreas;
                navAgent.speed = vitesseCourse;
                navAgent.stoppingDistance = 1f;
                break;
        }
    }

    void SetSonsDeplacement()
    {
        if (navAgent.speed < 1)
        {
            GetComponent<AudioSource>().volume = 0;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0.2f;
            GetComponent<AudioSource>().pitch = navAgent.speed == vitesseMarche ? 1 : 1.75f;
        }
    }

    /// <summary>
    ///     Set la destination du NavMashAgent a la position du joueur
    /// </summary>
    public void SetChasserJoueur()
    {
        navAgent.SetDestination(joueur.transform.position);
    }

    /// <summary>
    ///     Set la destination du NavMeshAgent a la position du point de patrouille.
    /// </summary>
    /// <param name="position"> Position du point de patrouille </param>
    public void SetPosCible(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    /// <summary>
    ///    Retourne la distance entre le paladin et la destination du NavMeshAgent.
    /// </summary>
    /// <returns>Distance restante</returns>
    public float GetDistanceCible()
    {
        return navAgent.remainingDistance;
    }

    /// <summary>
    ///     Verfie si le path du nav mesh agent est pret, en verifiant les parametres pathPending et pathStatus du
    ///     NavMeshAgent.
    /// </summary>
    /// <returns>Si le path est pret retourne true. Sinon retourne false</returns>
    public bool GetPathPret()
    {   
        // Methode s'assure que tout les parametres du navMash et de son deplacement soient pret
        // pour eviter d'avoir des valeurs qui ne fonctionnnent pas
        // comme le parametre remainingDistance du NavMeshAgent qui prend quelques ms de plus a etre calcule
        // et qui parfois retourne 0 alors que la destination se trouve plus loin.
        return !navAgent.pathPending && navAgent.pathStatus == NavMeshPathStatus.PathComplete;
    }
}

