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
        //  > Descr :
        //      Methode qui set la destination du NavMeshAgent du paladin a la position du joueur;

        navAgent.SetDestination(joueur.transform.position);
    }

    public void SetPatrouillePos(Vector3 posPointPatrouille)
    {
        //  > Description :
        //      Methode qui set la destionation du NavMeshAgent 
        //      du paladin au point de patrouille approprie;
        // 
        //  > Param(1) : 
        //      Vector3 -> Position du point de patrouille vers lequel le paladin se dirigera

        navAgent.SetDestination(posPointPatrouille);
    }

    public bool GetArrivePosPatrouille()
    {   
        //  > Descr :
        //      Methode qui verifie si le paladin est arrive a son point de patrouille;
        // 
        //  Retourne : bool;

        if (navAgent.remainingDistance < 0.2f)
        {
            return true;
        }

        return false;
    }

    public Vector3 GetPositionAlerte()
    {
        /* 
            > Descr : 
                Methode qui permet de recupere la position du paladin au moment d'une alerte;

            > Retourne : Vector3; 
        */
        return transform.position;
    }

    public void SetRetournPosAlerte(Vector3 posAlerte)
    {
        navAgent.SetDestination(posAlerte);
    }

    public float GetDistanceCible()
    {
        return navAgent.remainingDistance;
    }
}

