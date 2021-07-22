using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding{
    public class Pathfinder : MonoBehaviour{
        [SerializeField] Vector2Int startCoordinate;
        [SerializeField] Vector2Int endCoordinate;

        Node startNode;
        Node endNode;
        Node currentSearchNode;

        readonly Queue<Node> frontier = new Queue<Node>();
        Dictionary<Vector2Int, Node> exploredNodes = new Dictionary<Vector2Int, Node>();

        readonly Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        GridManager gridManager;
        public Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
        void Awake(){
            gridManager = FindObjectOfType<GridManager>();
            if (gridManager == null) return;
            this.grid = gridManager.Grid;
        }

        void Start(){
            this.startNode = grid[startCoordinate];
            this.endNode = grid[endCoordinate];

            GetNewPath();
        }

        List<Node> GetNewPath(){
            gridManager.ResetNodes();
            BreadthFirstSearch();
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
        void BreadthFirstSearch(){
            frontier.Clear();
            exploredNodes.Clear();
            var isRunning = true;
            
            frontier.Enqueue(startNode);
            exploredNodes.Add(startCoordinate, startNode);

            while (frontier.Count > 0 && isRunning){
                this.currentSearchNode = frontier.Dequeue();
                this.currentSearchNode.isExplored = true;
                ExploreNeighbors();
                if (currentSearchNode.coordinates == endCoordinate){
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
            if (!grid.ContainsKey(coordinates)) return false;
            var previousState = grid[coordinates].isWalkable; 
            grid[coordinates].isWalkable = false;
            var newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            return newPath.Count <= 1;
        }
    }
}
