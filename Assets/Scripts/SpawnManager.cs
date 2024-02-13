using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoint;

    void Start()
    {
        InvokeRepeating("GenerateEnemies", 1f,2f);
    }
    void GenerateEnemies()
    {
        int spawnPointIndex = Random.Range(0, spawnPoint.Length);
        int enemyPrefabIndex = Random.Range(0, enemyPrefab.Length);

        Instantiate(enemyPrefab[enemyPrefabIndex], spawnPoint[spawnPointIndex].position, Quaternion.identity);
    }
}
