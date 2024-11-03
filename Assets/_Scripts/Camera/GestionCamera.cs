using UnityEngine;

/* 
    Script de gestion de la camera du jeu, gerant les fonctionalites suivantes : 
        - Bla bla bla

    Par Yanis Oulmane
    Derniere modification : 02/11/2024
 */
public class GestionCamera : MonoBehaviour
{
    public GameObject joueur; // Position du joueur
    public Vector3 posCamera; // Distance de la camera par rapport au joueur

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.position = joueur.transform.TransformPoint(posCamera);

        transform.LookAt(joueur.transform.position);
    }
}
