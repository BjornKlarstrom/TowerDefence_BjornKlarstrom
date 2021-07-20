using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding{
    public class Pathfinder : MonoBehaviour{
        [SerializeField] Node currentSearchNode;
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        GridManager gridManager;
        public Dictionary<Vector2Int, Node> grid;
        void Awake(){
            gridManager = FindObjectOfType<GridManager>();
            if (gridManager != null){
                this.grid = gridManager.Grid;
            }
        }

        void Start(){
            ExploreNeighbors();
        }

        void ExploreNeighbors(){
            var neighbors = new List<Node>();

            foreach (var direction in directions){
                var neighborCoordinates = currentSearchNode.coordinates + direction;
                if (grid.ContainsKey(neighborCoordinates)){
                    neighbors.Add(grid[neighborCoordinates]);
                    
                    // TODO: Remove after testing
                    this.grid[neighborCoordinates].isExplored = true;
                    this.grid[currentSearchNode.coordinates].isPath = true;
                }
            }
        }
    }
}
