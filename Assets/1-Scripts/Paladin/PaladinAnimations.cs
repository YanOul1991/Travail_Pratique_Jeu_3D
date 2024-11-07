using UnityEngine;
/* 

 */
public class PaladinAnimations : MonoBehaviour
{
    [SerializeField] float porteeAttaque;
    Animator animatorComp; // Raccourci au component Animator du GameObject Paladin

    void Start()
    {
        /* ASSIGNATION DE LA REFERENCE AUX COMPONENT ANIMATOR DU GAMEOBJECT */
        animatorComp = GetComponent<Animator>();
    }

    public void GestionAnimations(Vector3 deplacement, Transform posCible, Transform posPaladin, bool dansChampVision, bool persoVisible, bool persoVivant)
    {
        // Assignation de la valeur 0 au deplacement vertical pour ne prendre en compte que les deplacements sur le sol
        // Assigne la magnitude du vecteur resultant au parametre 'vitesse' de l'animator
        deplacement.y = 0;

        animatorComp.SetFloat("vitesse", deplacement.magnitude);

        // Declaration d'un variable bool, pour definir si le personnage est dans la portee d'attaque du Paladin 
        bool persoDansPortee = Vector3.Distance(posCible.position, posPaladin.position) < porteeAttaque ? true : false;
        
        animatorComp.SetBool("attk", VerificationCondition(persoDansPortee, dansChampVision, persoVisible, persoVivant));
    }

    /* 
        FONCTION GESTION ANIM ATTAQUE : 
            - Fonction qui retourne un bool pour permettre ou non au paladin de jouer son animation d'attaque
            - Donne un nombre non definit de parametres bool a analyser
            - Avec un boucle verification de chaque conditions, et si une des condition est false la fonction retourne automatiquement false
            - Si la boucle ne detecte pas de condition false, retourne true
    */
    bool VerificationCondition(params bool[] lesConditions)
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
