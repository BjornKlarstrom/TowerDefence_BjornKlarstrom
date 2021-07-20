using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding{
    public class GridManager : MonoBehaviour{
        [SerializeField] Vector2Int gridSize;
        
        [Tooltip("Map Grid Size - Should match Unity Editor Snap Settings")]
        [SerializeField] int unityGridSize = 10;
        public int UnityGridSize => unityGridSize;

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
        
        public void BlockNode(Vector2Int coordinates){
            if (grid.ContainsKey(coordinates)){
                grid[coordinates].isWalkable = false;
            }
        }

        public Vector2Int GetCoordinatesFromPosition(Vector3 position){
            var coordinates = new Vector2Int();
            coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
            coordinates.y = Mathf.RoundToInt(position.z / UnityEditor.EditorSnapSettings.move.z);
            return new Vector2Int();
        }
    }
}