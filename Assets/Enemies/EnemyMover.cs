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
            RecalculatePath();
            ReturnToStart();
            StartCoroutine(FollowPath());
        }

        void RecalculatePath(){
            path.Clear();
            path = pathfinder.GetNewPath();
        }
        void DisableEnemy(){
            gameObject.SetActive(false);
        }

        void ReturnToStart(){
            this.transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinate);
        }

        void FinishPath(){
            enemy.StealGold();
            DisableEnemy();
        }
        IEnumerator FollowPath(){
            foreach (var node in path){
                var startPosition = this.transform.position;
                var nextPosition = gridManager.GetPositionFromCoordinates(node.coordinates);
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
    }
}
