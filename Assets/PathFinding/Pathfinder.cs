using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding{
    public class Pathfinder : MonoBehaviour{
        
        [SerializeField] Vector2Int startCoordinates;
        public Vector2Int StartCoordinates => startCoordinates;

        [SerializeField] Vector2Int endCoordinates;
        public Vector2Int EndCoordinates => endCoordinates;

        Node startNode;
        Node endNode;
        Node currentSearchNode;

        readonly Queue<Node> frontier = new Queue<Node>();
        readonly Dictionary<Vector2Int, Node> exploredNodes = new Dictionary<Vector2Int, Node>();

        readonly Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        GridManager gridManager;
        public Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
        void Awake(){
            gridManager = FindObjectOfType<GridManager>();
            if (gridManager == null) return;
            this.grid = gridManager.Grid;
            //this.startNode = grid[startCoordinates];
            //this.endNode = grid[endCoordinates];
        }

        void Start(){
            //GetNewPath();
        }

        public List<Node> GetNewPath(){
            return GetNewPath(startCoordinates);
        }
        
        public List<Node> GetNewPath(Vector2Int coordinates){
            gridManager.ResetNodes();
            BreadthFirstSearch(coordinates);
            return BuildPath();
        }

        void ExploreNeighbors(){
            var neighbors = new List<Node>();

            foreach (var direction in directions){
                var neighborCoordinates = currentSearchNode.coordinates + direction;
                if (grid.ContainsKey(neighborCoordinates)){
                    neighbors.Add(grid[neighborCoordinates]);
                }
            }

            foreach (var neighbor in neighbors){
                if (exploredNodes.ContainsKey(neighbor.coordinates) || !neighbor.isWalkable) continue;
                neighbor.connectedTo = currentSearchNode;
                exploredNodes.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
        void BreadthFirstSearch(Vector2Int coordinates){
            this.startNode.isWalkable = true;
            this.endNode.isWalkable = true; 
            frontier.Clear();
            exploredNodes.Clear();
            var isRunning = true;
            
            frontier.Enqueue(grid[coordinates]);
            exploredNodes.Add(coordinates, grid[coordinates]);

            while (frontier.Count > 0 && isRunning){
                this.currentSearchNode = frontier.Dequeue();
                this.currentSearchNode.isExplored = true;
                ExploreNeighbors();
                if (currentSearchNode.coordinates == endCoordinates){
                    isRunning = false;
                }
            }
        }

        List<Node> BuildPath(){
            var path = new List<Node>();
            var currentNode = endNode;
            
            path.Add(currentNode);
            currentNode.isPath = true;

            while (currentNode.connectedTo != null){
                currentNode = currentNode.connectedTo;
                path.Add(currentNode);
                currentNode.isPath = true;
            }
            path.Reverse();
            return path;
        }

        public bool WillBlockPath(Vector2Int coordinates){
            if (grid.ContainsKey(coordinates)){
                var previousState = grid[coordinates].isWalkable;

                grid[coordinates].isWalkable = false;
                var newPath = GetNewPath();
                grid[coordinates].isWalkable = previousState;

                if (newPath.Count <= 1){
                    GetNewPath();
                    return true;
                }
            }
            return false;
        }

        public void NotifyReceivers(){
            BroadcastMessage("RecalculatePath",false , SendMessageOptions.DontRequireReceiver);
        }
    }
}
