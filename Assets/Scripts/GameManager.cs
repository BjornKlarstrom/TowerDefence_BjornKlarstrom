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
    Camera mainCamera;

    [Header("HUD Objects")] 
    [SerializeField] GameObject generateButton;
    [SerializeField] GameObject acceptButton;

    void Awake(){
        gridManager = FindObjectOfType<GridManager>();
        enemyPool = FindObjectOfType<EnemyPool>();
        pathfinder = FindObjectOfType<Pathfinder>();
        mainCamera = FindObjectOfType<Camera>();
    }

    public void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextSceneWithDelay(){
        StartCoroutine(DelayedLoad());
    }

    static IEnumerator DelayedLoad(){
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreateMap(){
        gridManager.CreateRandomizedMap();
    }

    public void StartEnemies(){
        this.generateButton.SetActive(false);
        this.acceptButton.SetActive(false);
        pathfinder.InitPathfinder();
        enemyPool.StartSpawningEnemies();
    }

    bool IsPossibleMap(){
        const int minimumPathLenght = 10;
        Debug.Log(pathfinder.PathLenght);
        return pathfinder.PathLenght > minimumPathLenght;
    }
}
