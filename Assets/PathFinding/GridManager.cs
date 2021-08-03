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

        [Header("Tile Prefabs")]
        [SerializeField] Transform tileParent;
        [SerializeField] GameObject floorPrefab;
        [SerializeField] GameObject rockPrefab;
        [SerializeField] GameObject enemyBase;
        [SerializeField] GameObject playerBase;
        [SerializeField] GameObject invisibleBlockedPrefab;
        [SerializeField] GameObject[] wallPrefabs;
        
        MapGenerator mapGenerator;
        Pathfinder pathfinder;
        
        void Awake(){
            this.mapGenerator = GetComponent<MapGenerator>();
            mapGenerator.MapSize = gridSize;
            this.Grid = mapGenerator.GenerateMap();

            this.pathfinder = FindObjectOfType<Pathfinder>();
        }

        public void CreateRandomizedMap(){
            this.Grid.Clear();
            this.Grid = mapGenerator.GenerateMap();
            PlaceTilesOnGrid();
            this.pathfinder.InitPathfinder();
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
                switch (node.Value.nodeType){
                    case Node.NodeType.Wall:
                        Instantiate(randomWallTile, GetPositionFromCoordinates(node.Value.position),
                            Quaternion.identity, this.tileParent);
                        break;
                    case Node.NodeType.Rock:
                        Instantiate(rockPrefab, GetPositionFromCoordinates(node.Value.position), Quaternion.identity,
                            this.tileParent);
                        break;
                    case Node.NodeType.Floor:
                        Instantiate(floorPrefab, GetPositionFromCoordinates(node.Value.position), Quaternion.identity, this.tileParent);
                        break;
                    case Node.NodeType.EnemyBase:
                        pathfinder.StartCoordinates = node.Value.position;
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
                    case Node.NodeType.PlayerBase:
                        pathfinder.EndCoordinates = node.Value.position;
                        var playerBaseInstance =
                            Instantiate(playerBase, GetPositionFromCoordinates(node.Value.position), Quaternion.identity,
                                this.tileParent);

                        playerBaseInstance.transform.rotation = node.Value.faceDirection switch{
                            Node.FaceDirections.Right => Quaternion.Euler(0, 90, 0),
                            Node.FaceDirections.Down => Quaternion.Euler(0, 180, 0),
                            Node.FaceDirections.Left => Quaternion.Euler(0, -90, 0),
                            Node.FaceDirections.Up => Quaternion.Euler(0, 0, 0),
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        break;
                    case Node.NodeType.BlockedEmpty:
                        Instantiate(invisibleBlockedPrefab, GetPositionFromCoordinates(node.Value.position), Quaternion.identity, this.tileParent);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}