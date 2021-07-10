using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0.1f, 10.0f)] float speed = 1.0f;

    void Start(){
        StartCoroutine(MoveToNextWaypoint());
    }

    IEnumerator MoveToNextWaypoint(){
        
        foreach (var waypoint in path){
            var startPosition = this.transform.position;
            var endPosition = waypoint.transform.position;
            var movedPercent = 0.0f;
            
            this.transform.LookAt(endPosition);

            while (movedPercent < 1.0f){
                movedPercent += Time.deltaTime * speed;
                this.transform.position = Vector3.Lerp(startPosition, endPosition, movedPercent);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
