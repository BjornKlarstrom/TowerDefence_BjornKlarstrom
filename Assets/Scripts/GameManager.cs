using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using PathFinding;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    GridManager gridManager;
    EnemyPool enemyPool;
    Pathfinder pathfinder;

    void Awake(){
        gridManager = FindObjectOfType<GridManager>();
        enemyPool = FindObjectOfType<EnemyPool>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    public void StartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreateMap(){

            gridManager.CreateRandomizedMap();
    }

    public void StartEnemies(){
        pathfinder.InitPathfinder();
        enemyPool.StartSpawningEnemies();
    }
    
    bool IsPossibleMap(){
        const int minimumPathLenght = 10;
        Debug.Log(pathfinder.PathLenght);
        return pathfinder.PathLenght > minimumPathLenght;
    }
}
