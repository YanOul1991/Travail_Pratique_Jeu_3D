using UnityEngine;
using System.Collections;
using TMPro;

/* 
    Class de gestion du UI dans le jeu :
        - Affiche l'etat cache/visible du joueur en temps reel;
        - Gerer avec un coroutine infinie;
        - Framerate de la coroutine plus lent -> optimisation;

    Par: Yanis Oulmane;
    Derniere modification: 12/12/2024;
*/

public class gmGestionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI etatCacheUI; // Reference au UI affichant l'etat du joueur;
    [SerializeField] int framerateUI; // Framerate avec lequel le UI se fera mettre a jour a chaque seconde (1/framerateUI);

    void Start()
    {
        // Frame rate du UI est automatiquement de 5 si un valeur non autorise est donne.
        if(framerateUI <= 0) framerateUI = 5;

        // Demmarage de la coroutine de Gestion du UI
        StartCoroutine(GestionUI());
    }
    
    public IEnumerator GestionUI()
    {
        // Verifie les etats du joueur pour le UI a un certain framerate
        while (true)
        {
            // Si le joueur est cache
            if (GameManager.joueurCache)
            {
                // Affiche le le joueur n'est pas visible
                etatCacheUI.text = "CACHÉ";
                etatCacheUI.color = Color.white;
            }
            else
            {
                etatCacheUI.text = "VISIBLE";
                etatCacheUI.color = Color.red;
            }
            yield return new WaitForSeconds(1 / framerateUI);
        }
    }
}
