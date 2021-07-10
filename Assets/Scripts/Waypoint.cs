using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour{

    [SerializeField] GameObject towerPrefab;
    [SerializeField] bool isPlaceable;
    void OnMouseDown(){
        if (isPlaceable){
            Instantiate(towerPrefab, this.transform);
            isPlaceable = false;
        }
    }
}
