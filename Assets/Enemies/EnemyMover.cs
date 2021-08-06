using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinding;
using Tiles;
using UnityEngine;

namespace Enemies{
    [RequireComponent(typeof(Enemy))]
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 10.0f)] float speed = 1.0f;
        [SerializeField] float YOffset = 5.0f;
        
        List<Node> path = new List<Node>();
        GridManager gridManager;
        Pathfinder pathfinder;

        Enemy enemy;

        void Awake(){
            this.enemy = GetComponent<Enemy>();
            this.gridManager = FindObjectOfType<GridManager>();
            this.pathfinder = FindObjectOfType<Pathfinder>();
        }

        void OnEnable(){
            ReturnToStart();
            RecalculatePath(true);
        }

        void ReturnToStart(){
            this.transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
        }
        
        void RecalculatePath(bool resetPath){
            var coordinates = resetPath ? 
                pathfinder.StartCoordinates : 
                gridManager.GetCoordinatesFromPosition(this.transform.position);
            
            StopAllCoroutines();
            path.Clear();
            path = pathfinder.GetNewPath(coordinates);
            StartCoroutine(FollowPath());
        }
        void DisableEnemy(){
            gameObject.SetActive(false);
        }

        void FinishPath(){
            enemy.LoseDominance();
            DisableEnemy();
        }
        IEnumerator FollowPath(){
            for (var i = 1; i < path.Count; i++){
                var startPosition = this.transform.position;
                var nextPosition = gridManager.GetPositionFromCoordinates(path[i].position);
                nextPosition = new Vector3(nextPosition.x, YOffset, nextPosition.z);
                var movedPercent = 0.0f;
            
                this.transform.LookAt(nextPosition);

                while (movedPercent < 1.0f){
                    movedPercent += Time.deltaTime * speed;
                    this.transform.position = Vector3.Lerp(startPosition, nextPosition, movedPercent);
                    yield return new WaitForEndOfFrame();
                }
            }

            FinishPath();
        }

        public int GetPathLenght(){
            return path.Count;
        }
    }
}
