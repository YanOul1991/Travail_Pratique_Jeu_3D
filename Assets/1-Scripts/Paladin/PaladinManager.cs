using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/* 
    Classe de gestion du paladin:
        - Gestion des 3 etats du paladin (patrouille, inspection, chasse);
        - Echanges de donnes entres les differents scriptes;
*/

public class PaladinManager : MonoBehaviour
{

    /* Variables globales */
    [SerializeField] private float distanceDetectionMax;
    [SerializeField] private float coroutineFramerate; // Frame rate des coroutine de gestion des etats du paladin
    [SerializeField] private GameObject colliderEpee; // GameObject qui contient le collider de l'epee du paladin

    /* --------------- References GameObject / Classes --------------- */
    private GameObject joueur;

    // References aux autres classes du paladin
    private DeplacementPaladin deplacementPaladin;
    private PatrouillePaladin patrouillePaladin;
    private VisionPaladin visionPaladin;
    private AnimationsPaladin animationsPaladin;
    public enum EtatsPaladin // public enum des differents etats possibles du paladin
    {
        PATROUILLE,
        INSPECTION,
        CHASSE
    }

    private EtatsPaladin etat; // Variable prive memorisant l'etat du paladin
    private Vector3 posPointPatrouille; // Variable privee memorisant le point de patrouille du paladin
    private Vector3 posAlerte; // Var memorisant la position du paladin au moment de l'alerte

    void Start()
    {
        /* ```````````````` Assigniation des references ```````````````` */
        joueur = GameObject.FindWithTag("Player");
        deplacementPaladin = GetComponent<DeplacementPaladin>();
        patrouillePaladin = GetComponent<PatrouillePaladin>();
        visionPaladin = GetComponent<VisionPaladin>();
        animationsPaladin = GetComponent<AnimationsPaladin>();

        /* ``````````````````` Assigne le frame rate ``````````````````` */
        coroutineFramerate = 1 / coroutineFramerate;

        colliderEpee.SetActive(false);

        StartCoroutine(Initialise());
    }

    /// <summary>
    ///     Set des variables intiales lorsque le paladin est active
    /// </summary>
    /// <returns></returns>
    IEnumerator Initialise()
    {
        // Incremente le point de partouille et set la position de celui-ci comme 
        // la destination du paladin
        posPointPatrouille = patrouillePaladin.GetProchainPos();
        deplacementPaladin.SetPosCible(posPointPatrouille);

        // Attendre 
        while (!deplacementPaladin.GetPathPret())
        {
            yield return null;
        }

        StartCoroutine(GestionPatrouille());

        yield break;
    }


    IEnumerator GestionPatrouille()
    {
        // Met a jour l'etat est les contraintes de deplacement
        etat = EtatsPaladin.PATROUILLE;
        deplacementPaladin.SetContraintes();

        while (!deplacementPaladin.GetPathPret())
        {
            yield return null;
        }

        while (true)
        {
            // Lorsque le paladin est arrive a sa position de patrouille
            if (deplacementPaladin.GetDistanceCible() < 0.2f)
            {
                yield return new WaitForSeconds(3);
                posPointPatrouille = patrouillePaladin.GetProchainPos();
                deplacementPaladin.SetPosCible(posPointPatrouille);
            }

            // Si le joueur se fait repere
            if (visionPaladin.GetJoueurRepere())
            {
                posAlerte = transform.position;
                // Memorise la position du paladin au moment ou il apercoit le joueur.
                posAlerte = GetPosAlerte();
                StartCoroutine(GestionChasse());
                yield break;
            }
            yield return new WaitForSeconds(coroutineFramerate);
        }
    }


    /* ============================= METHODES ============================= */

    /// <summary>
    ///     Coroutine de gestion de l'etat de chasse du paladin.
    /// </summary>
    IEnumerator GestionChasse()
    {
        Debug.ClearDeveloperConsole();
        etat = EtatsPaladin.CHASSE;

        deplacementPaladin.SetContraintes();

        /* ================= BOUCLE DE CHASSE DU JOUEUR ================= */
        // Appel le fonction de chasse du joueur tant que celui-ci;
        // sera dans la vision du paladin ;
        while (visionPaladin.GetJoueurRepere())
        {
            deplacementPaladin.SetChasserJoueur();

            // Si le paladin est a moins de 1m du joueur trigger l'animation d'attaque
            if (deplacementPaladin.GetDistanceCible() < 1.5f)
            {
                SetAttaque();
            }

            yield return new WaitForSeconds(coroutineFramerate);
        }

        // Apres que le paladin perd le joueur de vue
        // Attendre que le path soit pret
        while (!deplacementPaladin.GetPathPret())
        {
            yield return null;
        }

        // Le paladin continue vers le derniere endroit ou il a apercu le joueur pour la derniere fois
        while (deplacementPaladin.GetDistanceCible() > 3f)
        {
            // Si le joueur est de nouveau repere 
            // Recommence une nouvelle iteration de la coroutine de chasse
            // Arrete l'instance en cours
            if (visionPaladin.GetJoueurRepere())
            {
                // Debug.Log("spotted again before point");
                StartCoroutine(GestionChasse());
                yield break;
            }
            yield return new WaitForSeconds(coroutineFramerate);
        }

        StartCoroutine(GestionInspection());
        yield break;
    }

    /// <summary>
    ///     Coroutine de gestion de l'etat d'insepction du paladin.
    /// </summary>
    IEnumerator GestionInspection()
    {
        // Mise a jour de l'etat du paladin
        // Mise a jour de ses contraintes de deplacements
        etat = EtatsPaladin.INSPECTION;
        deplacementPaladin.SetContraintes();

        // Attendre pendant un certain temps
        float tempsAttente = 5;
        while (tempsAttente > 1)
        {
            if (visionPaladin.GetJoueurRepere())
            {
                StartCoroutine(GestionChasse());
                yield break;
            }

            tempsAttente -= coroutineFramerate;
            yield return new WaitForSeconds(coroutineFramerate);
        }

        // Paladin retourne a la position ou il se trouvait au moment de l'alerte
        deplacementPaladin.SetPosCible(posAlerte);

        // Attendre le calcule du path
        while (!deplacementPaladin.GetPathPret())
        {
            yield return null;
        }

        while (deplacementPaladin.GetDistanceCible() > 0.1f)
        {
            if (visionPaladin.GetJoueurRepere())
            {
                StartCoroutine(GestionChasse());
                yield break;
            }

            yield return new WaitForSeconds(coroutineFramerate);
        }

        deplacementPaladin.SetPosCible(posPointPatrouille);

        while (!deplacementPaladin.GetPathPret())
        {
            yield return null;
        }

        StartCoroutine(GestionPatrouille());
        yield break;
    }


    /// <summary>
    ///     Methode qui retourne l'etat actuel du paladin
    /// </summary>
    /// <returns>Etat du paladin</returns>
    public EtatsPaladin GetEtatPaldin()
    {
        return etat;
    }

    Vector3 GetPosAlerte()
    {
        return transform.position;
    }

    /// <summary>
    ///     Gere les propreietes du paladin lorsque celui-ci passe en mode attaque
    /// </summary>
    private void SetAttaque()
    {   
        // Le paladin est immobile quand il attaque
        GetComponent<NavMeshAgent>().speed = 0;
        // Active le collider
        colliderEpee.SetActive(true);
        animationsPaladin.SetAnimAttaqueTrigger();

        // Appel la methode qui retourne le paladin a ses proprietes normales apres
        // qu'il termine son attaque
        Invoke(nameof(SetAttaqueFin), animationsPaladin.GetAnimAttaqueDuree());
    }

    /// <summary>
    ///     Retourne les proprietes de paladin a leur etat normales apres la fin de l'attaque de celui-ci
    /// </summary>
    private void SetAttaqueFin()
    {
        colliderEpee.SetActive(false);
        deplacementPaladin.SetContraintes();
    }
}
