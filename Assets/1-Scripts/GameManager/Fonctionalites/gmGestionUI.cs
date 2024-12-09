using UnityEngine;
using System.Collections;
using TMPro;

public class gmGestionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI etatCacheUI; // Reference au UI affichant l'etat du joueur;
    [SerializeField] int framerateUI; // Framerate avec lequel le UI se fera mettre a jour a chaque seconde (1/framerateUI);

    void Start()
    {
        // Frame rate du UI est automatiquement de 5 si un valeur non autorise est donne.
        if(framerateUI <= 0) framerateUI = 5;

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
                etatCacheUI.text = "cache";
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
