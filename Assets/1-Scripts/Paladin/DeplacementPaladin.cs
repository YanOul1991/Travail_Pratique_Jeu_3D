using UnityEngine;
using UnityEngine.AI;
/* 
    Scripte de gestion des deplacements d'un Paladin :
        - Gestion des vitesses et des animations
        - Gestin des rotation selon cible que le paladin va suivre

    Par Yanis Oulmane
    Derniere modification : 04-11-2024
 */
public class DeplacementPaladin : MonoBehaviour
{
    /* ================================ VARIABLES ================================ */

    /* --------------- Mouvements --------------- */
    [SerializeField] private float vitesseMarche; // Variable memorisant la vitesse de marche du paladin;
    [SerializeField] private float vitesseCourse; // Variable memorisant la vitesse de course du paladin;\

    /* --------------- Positions --------------- */
    private GameObject joueur;

    /* ---------- Components / Class ---------- */
    private PaladinManager paladinManager;
    private NavMeshAgent navAgent; // References au component NavMeshAgent du paladin;
    private PatrouillePaladin patrouillePaladin; // Reference a la classe PatrouillePaladin;

    /* =========================================================================== */

    void Start()
    {
        joueur = GameObject.FindWithTag("Player");

        // Assigniation des references de components et scripts 
        paladinManager = GetComponent<PaladinManager>();
        navAgent = GetComponent<NavMeshAgent>();
        patrouillePaladin = GetComponent<PatrouillePaladin>();

        // Assignitations de valeurs par defauts :
        // Vitesse de deplacement du paladin
        navAgent.speed = vitesseMarche;
    }

    /* ============================== METHODES ============================== */

    public void SetNavArea()
    {   
        //  > Descr: 
        //      Methode qui change le nav area du paladin selon son etat;

        navAgent.areaMask = paladinManager.GetEtatPaldin() switch
        {
            PaladinManager.EtatsPaladin.PATROUILLE => 1 << NavMesh.GetAreaFromName("Route"),
            PaladinManager.EtatsPaladin.INSPECTION => NavMesh.AllAreas,
            PaladinManager.EtatsPaladin.CHASSE => NavMesh.AllAreas,
            _ => NavMesh.AllAreas
        };
    }
    public void SetVitesseDeplac()
    {
        //  > Descr : 
        //      Methode qui set la vitesse de deplacement du paladin selon son etat;

        navAgent.speed = paladinManager.GetEtatPaldin() switch
        {
            PaladinManager.EtatsPaladin.PATROUILLE => vitesseMarche,
            PaladinManager.EtatsPaladin.INSPECTION => vitesseMarche,
            PaladinManager.EtatsPaladin.CHASSE => vitesseCourse,
            _ => vitesseMarche
        };
    }

    public void SetChasserJoueur()
    {
        navAgent.SetDestination(joueur.transform.position);
    }

    public void SetPatrouillePos(Vector3 posPointPatrouille)
    {
        navAgent.SetDestination(posPointPatrouille);
    }

    /// <summary>
    ///     Methode qui verifie si le paladin est arrive a la destination de sa patrouille.
    /// </summary>
    /// <returns>Retourne true si le paladin est arrive a destination. Sinon retourne false.</returns>
    public bool GetArrivePosPatrouille()
    {   
        return navAgent.remainingDistance < 0.2f;
    }

    /// <summary>
    ///     Methode qui retourne la distance restante de la destination actuel de son component NavMeshAgent;
    /// </summary>
    /// <returns> float </returns>
    public float GetDistanceCible()
    {
        return navAgent.remainingDistance;
    }
}

