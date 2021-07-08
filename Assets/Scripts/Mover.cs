using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();

    void Start(){
        StartCoroutine(MoveToNextWaypoint());
    }

    IEnumerator MoveToNextWaypoint(){
        
        foreach (var waypoint in path){
            var startPosition = this.transform.position;
            var endPosition = waypoint.transform.position;
            var movedPercent = 0.0f;

            while (movedPercent < 1.0f){
                movedPercent += Time.deltaTime;
                this.transform.position = Vector3.Lerp(startPosition, endPosition, movedPercent);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
