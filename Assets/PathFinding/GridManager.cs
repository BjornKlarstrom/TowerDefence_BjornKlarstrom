using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding{
    public class GridManager : MonoBehaviour{
        [SerializeField] Vector2Int gridSize;
        readonly Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
        public Dictionary<Vector2Int, Node> Grid => grid;
        void Awake(){
            CreateGrid();
        }
        void CreateGrid(){
            for (var x = 0; x < gridSize.x; x++){
                for (var y = 0; y < gridSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    grid.Add(coordinates, new Node(coordinates, true));
                }
            }
        }
        public Node GetNode(Vector2Int coordinates){
            return grid.ContainsKey(coordinates) ? grid[coordinates] : null;
        }
    }
}