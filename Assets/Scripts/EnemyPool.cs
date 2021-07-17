using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour{
    [SerializeField] [Range(0, 50)]int size = 3;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0.1f, 10.0f)] float spawnTimer = 2.0f;

    GameObject[] pool;

    void Awake(){
        FillPool();
    }
    void Start(){
        StartCoroutine(SpawnEnemy());
    }
    void FillPool(){
        pool = new GameObject[size];

        for (var i = 0; i < pool.Length; i++){
            pool[i] = Instantiate(enemyPrefab, this.transform);
            pool[i].SetActive(false);
        }
    }

    IEnumerator SpawnEnemy(){
        while (true){
            EnableEnemiesInPool();
            yield return new WaitForSeconds(spawnTimer);   
        }
    }

    void EnableEnemiesInPool(){
        for (var i = 0; i < size; i++){
            if (pool[i].activeInHierarchy) continue;
            pool[i].SetActive(true);
            return;
        }
    }
}
