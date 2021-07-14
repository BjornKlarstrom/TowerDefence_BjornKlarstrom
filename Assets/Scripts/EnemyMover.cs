using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0.1f, 10.0f)] float speed = 1.0f;
    [SerializeField] float waypointYOffset = 5.0f;

    void OnEnable(){
        FindPath();
        MoveToStart();
        StartCoroutine(MoveToNextWaypoint());
    }

    void FindPath(){
        path.Clear();
        var waypoints = GameObject.FindGameObjectsWithTag("Path");
        foreach (var point in waypoints){
            path.Add(point.GetComponent<Waypoint>());
        }
    }
    void DisableEnemy(){
        gameObject.SetActive(false);
    }

    void MoveToStart(){
        var firstWaypoint = path.First().transform.position;
        var startPosition = new Vector3(firstWaypoint.x, waypointYOffset, firstWaypoint.z);
        this.transform.position = startPosition; 
    }
    IEnumerator MoveToNextWaypoint(){
        
        foreach (var waypoint in path){
            var startPosition = this.transform.position;
            var endPosition = waypoint.transform.position;
            endPosition = new Vector3(endPosition.x, waypointYOffset, endPosition.z);
            var movedPercent = 0.0f;
            
            this.transform.LookAt(endPosition);

            while (movedPercent < 1.0f){
                movedPercent += Time.deltaTime * speed;
                this.transform.position = Vector3.Lerp(startPosition, endPosition, movedPercent);
                yield return new WaitForEndOfFrame();
            }
        }
        DisableEnemy();
    }
}
