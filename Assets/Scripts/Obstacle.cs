using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 moveDir;         // direction pour se déplacer
    public float moveSpeed;         // vitesse de déplacement le long de moveDir

    private float aliveTime = 8.0f; // temps avant que l'objet soit détruit

    void Start ()
    {
        Destroy(gameObject, aliveTime);
    }

    void Update ()
    {
        // déplacer l'obstacle dans une certaine direction au fil du temps
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // faire pivoter l'obstacle
        transform.Rotate(Vector3.back * moveDir.x * (moveSpeed * 20) * Time.deltaTime);
    }
}
