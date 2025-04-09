using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstacles;      // tableau de tous les différents types d'obstacles

    public float minSpawnY;             // hauteur minimale à laquelle les objets peuvent apparaître
    public float maxSpawnY;             // hauteur maximale à laquelle les objets peuvent apparaître
    private float leftSpawnX;           // côté gauche de l'écran
    private float rightSpawnX;          // côté droit de l'écran

    public float spawnRate;             // temps en secondes entre chaque apparition
    private float lastSpawn;            // Time.time de la dernière apparition

    void Start ()
    {
        // définir les limites de spawn gauche et droite
        // faites cela en obtenant les bordures horizontales de la caméra
        Camera cam = Camera.main;
        float camWidth = (2.0f * cam.orthographicSize) * cam.aspect;

        leftSpawnX = -camWidth / 2;
        rightSpawnX = camWidth / 2;
    }

    void Update ()
    {
        // toutes les 'spawnRate' secondes, faites apparaître un nouvel obstacle
        if(Time.time - spawnRate >= lastSpawn)
        {
            lastSpawn = Time.time;
            SpawnObstacle();
        }
    }

    // fait apparaître un obstacle aléatoire à un point d'apparition aléatoire
    void SpawnObstacle()
    {
        GameObject prefab = obstacles[Random.Range(0, obstacles.Length)];

        if (prefab == null)
        {
            Debug.LogWarning("Un obstacle du tableau est null !");
            return;
        }

        GameObject obstacle = Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);

        // définir la direction de déplacement de l'obstacle
        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            obstacleScript.moveDir = new Vector3(obstacle.transform.position.x > 0 ? -1 : 1, 0, 0);
        }
        else
        {
            Debug.LogWarning("L'objet instancié n'a pas de script Obstacle !");
        }
    }

    // retourne une position d'apparition aléatoire pour un obstacle
    Vector3 GetSpawnPosition ()
    {
        float x = Random.Range(0, 2) == 1 ? leftSpawnX : rightSpawnX;
        float y = Random.Range(minSpawnY, maxSpawnY);

        return new Vector3(x, y, 0);
    }
}
