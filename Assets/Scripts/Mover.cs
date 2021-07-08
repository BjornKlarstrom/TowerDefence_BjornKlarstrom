using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] float moveDelay = 1f;

    void Start(){
        StartCoroutine(PrintWaypoints());
    }

    IEnumerator PrintWaypoints(){
        foreach (var waypoint in path){
            this.transform.position = waypoint.transform.position;
            yield return new WaitForSeconds(moveDelay);
        }
    }
}
