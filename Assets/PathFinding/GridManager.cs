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

        [SerializeField] Transform tileParent;
        [SerializeField] GameObject floorPrefab;
        [SerializeField] GameObject rockPrefab;
        [SerializeField] GameObject enemyBase;
        [SerializeField] GameObject invisibleBlockedPrefab;
        [SerializeField] GameObject[] wallPrefabs;
        
        MapGenerator mapGenerator;
        
        void Awake(){
            this.mapGenerator = GetComponent<MapGenerator>();
            mapGenerator.MapSize = gridSize;
            //this.Grid = mapGenerator.GenerateMap();
        }

        void Start(){
            //PlaceTilesOnGrid();
        }

        void CreateMap(){
            for (var x = 0; x < gridSize.x; x++){
                for (var y = 0; y < gridSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    Grid.Add(coordinates, new Node(coordinates, true, Node.Type.Floor));
                }
            }
        }

        public void CreateRandomizedMap(){
            this.Grid.Clear();
            this.Grid = mapGenerator.GenerateMap();
            PlaceTilesOnGrid();
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
        
        void PlaceTilesOnGrid(){
            if (Grid == null) return;
            var random = new System.Random();

            foreach (Transform tile in tileParent.transform){
                Destroy(tile.gameObject);
            }

            foreach (var node in Grid){
                var randomWallTile = wallPrefabs[random.Next(0, wallPrefabs.Length)];
                switch (node.Value.type){
                    case Node.Type.Wall:
                        Instantiate(randomWallTile, GetPositionFromCoordinates(node.Value.position),
                            Quaternion.identity, this.tileParent);
                        break;
                    case Node.Type.Rock:
                        Instantiate(rockPrefab, GetPositionFromCoordinates(node.Value.position), Quaternion.identity,
                            this.tileParent);
                        break;
                    case Node.Type.Floor:
                        Instantiate(floorPrefab, GetPositionFromCoordinates(node.Value.position), Quaternion.identity, this.tileParent);
                        break;
                    case Node.Type.EnemyBase:
                        var enemyBaseInstance =
                            Instantiate(enemyBase, GetPositionFromCoordinates(node.Value.position), Quaternion.identity,
                                this.tileParent);

                        enemyBaseInstance.transform.rotation = node.Value.faceDirection switch{
                            Node.FaceDirections.Right => Quaternion.Euler(0, 90, 0),
                            Node.FaceDirections.Down => Quaternion.Euler(0, 180, 0),
                            Node.FaceDirections.Left => Quaternion.Euler(0, -90, 0),
                            Node.FaceDirections.Up => Quaternion.Euler(0, 0, 0),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        break;
                    case Node.Type.BlockedEmpty:
                        Instantiate(invisibleBlockedPrefab, GetPositionFromCoordinates(node.Value.position), Quaternion.identity, this.tileParent);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}