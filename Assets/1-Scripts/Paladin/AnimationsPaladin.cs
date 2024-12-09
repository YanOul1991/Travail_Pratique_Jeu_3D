using UnityEngine;
using UnityEngine.AI;

public class AnimationsPaladin : MonoBehaviour
{
    [SerializeField] float porteeAttaque;
    Animator animatorComp; // Raccourci au component Animator du GameObject Paladin;
    NavMeshAgent navAgent; // Reference au componentn NavMeshAgent du paladin;
    [SerializeField] AnimationClip animationAttaque; // Reference au clip d'animation d'attaque du paladin;

    void Start()
    {
        // Assignitation des references aux components et scriptes
        animatorComp = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        // La valeur de la magnitude de la velocite du NavMeshAgent sera la assigne au parametre "vitesse"
        animatorComp.SetFloat("vitesse", navAgent.velocity.magnitude);
    }

    
    /// <summary>
    ///     Set le parametre trigger "attaque" du l'animator du paladin pour jouer l'animation d'attaque;
    /// </summary>
    public void SetAnimAttaqueTrigger()
    {   
        animatorComp.SetTrigger("attaque");
    }

    /// <summary>
    ///     Retourne la duree totale de l'animation d'attaque du Paladin en secondes.
    /// </summary>
    /// <returns>Duree de l'animation d'attaque/returns>
    public float GetAnimAttaqueDuree()
    {
        return animationAttaque.length;
    }
}
