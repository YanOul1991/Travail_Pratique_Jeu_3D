using UnityEngine;
using UnityEngine.AI;

public class AnimationsPaladin : MonoBehaviour
{
    [SerializeField] float porteeAttaque;
    Animator animatorComp; // Raccourci au component Animator du GameObject Paladin
    VisionPaladin visionPaladin; // Reference a la classe VisionPaladin
    NavMeshAgent navAgent; // Reference au componentn NavMeshAgent du paladin

    void Start()
    {
        // Assignitation des references aux components et scriptes
        animatorComp = GetComponent<Animator>();
        visionPaladin = GetComponent<VisionPaladin>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        // La valeur de la magnitude de la velocite du NavMeshAgent sera la assigne au parametre "vitesse"
        animatorComp.SetFloat("vitesse", navAgent.velocity.magnitude);
    }

    // Fonction publique qui gere les animations d'attaque
    public void AnimationAttaque()
    {   
        animatorComp.SetBool("attk", GetAnimAttaque(visionPaladin.GetJoueurRepere(), GameManager.joueurDansOmbre, GameManager.joueurVivant));
    }

    /// <summary>
    /// Methode qui verifie si les conditions pour jouer l'animation d'attaque sont respectes.
    /// </summary>
    /// <param name="lesConditions"></param>
    /// <returns>Bool</returns>
    bool GetAnimAttaque(params bool[] lesConditions)
    {
        foreach (bool condition in lesConditions)
        {
            if (!condition)
            {
                return false;
            }
        }
        return true;
    }
}
