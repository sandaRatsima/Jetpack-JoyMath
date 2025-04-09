using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemTube : MonoBehaviour
{
    public int tubeId;  // numéro d'identification pour ce tube

    // appelé lorsque quelque chose entre en collision avec le collider du tube
    void OnTriggerEnter2D (Collider2D col)
    {
        // était-ce le joueur ?
        if(col.CompareTag("Player"))
        {
            // informer le gestionnaire de jeu que le joueur est entré dans ce tube
            GameManager.instance.OnPlayerEnterTube(tubeId);
        }
    }
}
