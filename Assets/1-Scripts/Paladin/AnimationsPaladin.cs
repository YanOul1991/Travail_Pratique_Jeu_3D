using UnityEngine;
/* 

 */
public class AnimationsPaladin : MonoBehaviour
{
    [SerializeField] float porteeAttaque;
    Animator animatorComp; // Raccourci au component Animator du GameObject Paladin
    VisionPaladin visionPaladin; // Reference a la classe VisionPaladin

    void Start()
    {
        /* ASSIGNATION DE LA REFERENCE AUX COMPONENT ANIMATOR DU GAMEOBJECT */
        animatorComp = GetComponent<Animator>();
        visionPaladin = GetComponent<VisionPaladin>();
    }

    public void AnimationDeplacement(float deplacement)
    {
        animatorComp.SetFloat("vitesse", deplacement);
    }

    public void AnimationAttaque()
    {
        // Assignation de la valeur 0 au deplacement vertical pour ne prendre en compte que les deplacements sur le sol
        // Assigne la magnitude du vecteur resultant au parametre 'vitesse' de l'animator
        // deplacement.y = 0;

        // animatorComp.SetFloat("vitesse", deplacement.magnitude);

        // Declaration d'un variable bool, pour definir si le personnage est dans la portee d'attaque du Paladin 
        // bool persoDansPortee = Vector3.Distance(posCible.position,transform.position) < porteeAttaque ? true : false;
        
        animatorComp.SetBool("attk", ConditionsAttaqueAnim(visionPaladin.JoueurDansVision(), GameManager.joueurVisible, GameManager.joueurVivant));
    }

    /* 
        FONCTION GESTION ANIM ATTAQUE : 
            Fonction qui verifie les conditions permettant au paladin de pouvoir activer son animation d'attaque.
            Fera un boucle a l'array ou si une condition n'est pas respectee la fonction retourne imediatement false.
            Si toute les conditions sont respecte alors elle retourne true.

            Parametres : La fonction prend un nombre non definit de bool qui seront place dans un array;

            Retourne : bool;
    */
    bool ConditionsAttaqueAnim(params bool[] lesConditions)
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
