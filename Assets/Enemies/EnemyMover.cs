using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemies{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] List<Waypoint> path = new List<Waypoint>();
        [SerializeField] [Range(0.1f, 10.0f)] float speed = 1.0f;
        [SerializeField] float waypointYOffset = 5.0f;

        Enemy enemy;

        void Start(){
            this.enemy = GetComponent<Enemy>();
        }

        void OnEnable(){
            FindPath();
            ResetToStart();
            StartCoroutine(FollowPath());
        }

        void FindPath(){
            path.Clear();
            var pathParent = GameObject.FindGameObjectWithTag("Path");
            foreach (Transform child in pathParent.transform){
                var waypoint = child.GetComponent<Waypoint>();
                if (waypoint != null){
                    path.Add(waypoint);
                }
            }
        }
        void DisableEnemy(){
            gameObject.SetActive(false);
        }

        void ResetToStart(){
            var firstWaypoint = path.First().transform.position;
            var startPosition = new Vector3(firstWaypoint.x, waypointYOffset, firstWaypoint.z);
            this.transform.position = startPosition; 
        }

        void FinishPath(){
            enemy.StealGold();
            DisableEnemy();
        }
        IEnumerator FollowPath(){
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
            FinishPath();
        }
    }
}
