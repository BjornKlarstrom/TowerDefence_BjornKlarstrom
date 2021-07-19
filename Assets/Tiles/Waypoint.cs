using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour{

    [SerializeField] Tower towerPrefab;
    [SerializeField] bool isPlaceable;
    public bool IsPlaceable => isPlaceable;

    void OnMouseDown(){
        if (!isPlaceable) return;
        var isPlaced = Tower.CreateTower(towerPrefab, transform.position);
        isPlaceable = !isPlaced;
    }
}
