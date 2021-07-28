using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding{
    public class GridManager : MonoBehaviour{
        
        [SerializeField] Vector2Int gridSize;
        
        [Tooltip("Map Grid Size - Should match Unity Editor Snap Settings")]
        [SerializeField] int unityGridSize = 10;
        public int UnityGridSize => unityGridSize;

        public Dictionary<Vector2Int, Node> Grid{ get; private set; } = new Dictionary<Vector2Int, Node>();

        MapGenerator mapGenerator;
        
        void Awake(){
            this.mapGenerator = GetComponent<MapGenerator>();
            mapGenerator.MapSize = gridSize;
            this.Grid = mapGenerator.GenerateMap();
        }

        void CreateGrid(){
            for (var x = 0; x < gridSize.x; x++){
                for (var y = 0; y < gridSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    Grid.Add(coordinates, new Node(coordinates, true));
                }
            }
        }
        
        public Node GetNode(Vector2Int coordinates){
            return Grid.ContainsKey(coordinates) ? Grid[coordinates] : null;
        }
        
        public void BlockNode(Vector2Int coordinates){
            if (Grid.ContainsKey(coordinates)){
                Grid[coordinates].isWalkable = false;
            }
        }

        public void ResetNodes(){
            foreach (var node in Grid){
                node.Value.connectedTo = null;
                node.Value.isExplored = false;
                node.Value.isPath = false;
            }
        }

        public Vector2Int GetCoordinatesFromPosition(Vector3 position){
            var coordinates = new Vector2Int{
                x = Mathf.RoundToInt(position.x / unityGridSize),
                y = Mathf.RoundToInt(position.z / unityGridSize)
            };
            return coordinates;
        }

        public Vector3 GetPositionFromCoordinates(Vector2Int coordinates){
            var position = new Vector3{
                x = coordinates.x * unityGridSize,
                z = coordinates.y * unityGridSize
            };
            return position;
        }
    }
}