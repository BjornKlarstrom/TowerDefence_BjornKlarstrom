using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace PathFinding{
    public class MapGenerator : MonoBehaviour{
        public Vector2Int MapSize{ get; set; }

        [SerializeField] string seed;
        [SerializeField] bool useRandomSeed;

        [Range(0, 100)] [SerializeField] int wallPercent;
        [Range(0, 100)] [SerializeField] int rockPercent;

        //int[,] map;
        Dictionary<Vector2Int, Node> Map{ get; set; } = new Dictionary<Vector2Int, Node>();

        [SerializeField] int smoothingIterations = 3;

        void Start(){
            //GenerateMap(new Vector2Int(16,16));
        }

        public Dictionary<Vector2Int, Node> GenerateMap(){
            RandomFillMap();
            for (var i = 0; i < smoothingIterations; i++){
                SmoothMap();
            }
            PlaceRocks();
            PlaceEnemyAndPlayerBases();
            return Map;
        }

        void RandomFillMap(){
            if (useRandomSeed){
                this.seed = Time.time.ToString(CultureInfo.InvariantCulture);
            }

            var random = new System.Random(seed.GetHashCode());

            for (var x = 0; x < MapSize.x; x++){
                for (var y = 0; y < MapSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    if (IsEdgeNode(coordinates)){
                        this.Map.Add(coordinates, new Node(coordinates, false, Node.NodeType.Wall));
                    }
                    else{
                        this.Map.Add(coordinates, new Node(coordinates, true, Node.NodeType.Floor));
                        this.Map[coordinates].nodeType = random.Next(0, 100) < wallPercent ? 
                            Node.NodeType.Wall : Node.NodeType.Floor;
                    }
                }
            }
        }
        
        void PlaceEnemyAndPlayerBases(){
            RandomizeBasePositions();
        }

        /*void OnDrawGizmos(){
            if (Map == null) return;
            for (var x = 0; x < MapSize.x; x++){
                for (var y = 0; y < MapSize.y; y++){
                    var coordinates = new Vector2Int();
                    coordinates.Set(x, y);
                    Gizmos.color = this.Map[coordinates].isWall ? Color.black : Color.green;
                    var position = new Vector3(-MapSize.x / 2 + x + 0.5f, 0.0f, -MapSize.y / 2 + y + 0.5f);
                    Gizmos.DrawCube(position, Vector3.one);
                }
            }
        }*/

        void SmoothMap(){
            for (var x = 0; x < MapSize.x; x++){
                for (var y = 0; y < MapSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    var blockedNeighbours = GetWallNeighbours(coordinates);
                    if (blockedNeighbours > 4){
                        //this.Map[coordinates].ResetNode();
                        this.Map[coordinates].nodeType = Node.NodeType.Wall;
                    }
                    else if (blockedNeighbours < 2){
                        //this.Map[coordinates].ResetNode();
                        this.Map[coordinates].nodeType = Node.NodeType.Floor;
                    }
                }
            }
        }

        void PlaceRocks(){
            var random = new System.Random();
            for (var x = 0; x < MapSize.x; x++){
                for (var y = 0; y < MapSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    if (random.Next(0, 100) >= rockPercent ||
                        this.Map[coordinates].nodeType == Node.NodeType.Wall) continue;
                    this.Map[coordinates].nodeType = Node.NodeType.Rock;
                    this.Map[coordinates].isWalkable = false;
                }
            }
        }

        void RandomizeBasePositions(){

            var values = Enum.GetValues(typeof(BaseSide));
            var random = new System.Random();
            
            var randomEnemySide = (BaseSide)values.GetValue(random.Next(values.Length));
            
            BaseSide randomPlayerSide;
            do{
                randomPlayerSide = (BaseSide) values.GetValue(random.Next(values.Length));
            } while (randomPlayerSide == randomEnemySide);

            FindFirstAvailableTile(randomEnemySide, Node.NodeType.EnemyBase);
            FindFirstAvailableTile(randomPlayerSide, Node.NodeType.PlayerBase);
        }

        void FindFirstAvailableTile(BaseSide randomSide, Node.NodeType type){
            const int cornerThreshold = 2;
            switch (randomSide){
                case BaseSide.Left:{
                    for (var x = 0; x < MapSize.x; x++){
                        for (var y = cornerThreshold; y < MapSize.y - cornerThreshold; y++){
                            if (IsWall(x, y)) continue;
                            SetupBaseNode(new Vector2Int(x, y), randomSide, type);
                            return;
                        }
                    }
                    break;
                }
                case BaseSide.Top:{
                    for (var y = MapSize.y - 1; y >= 0; y--){
                        for (var x = cornerThreshold; x < MapSize.x - cornerThreshold; x++){
                            if (IsWall(x, y)) continue;
                            SetupBaseNode(new Vector2Int(x, y), randomSide, type);
                            return;
                        }
                    }
                    break;
                }

                case BaseSide.Right:{
                    for (var x = MapSize.x - 1; x > 0; x--){
                        for (var y = cornerThreshold; y < MapSize.y - cornerThreshold; y++){
                            if (IsWall(x, y)) continue;
                            SetupBaseNode(new Vector2Int(x, y), randomSide, type);
                            return;
                        }
                    }
                    break;
                }

                case BaseSide.Bottom:{
                    for (var y = 0; y < MapSize.x; y++){
                        for (var x = cornerThreshold; x < MapSize.y - cornerThreshold; x++){
                            if (IsWall(x, y)) continue;
                            SetupBaseNode(new Vector2Int(x, y), randomSide, type);
                            return;
                        }
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        bool IsWall(int x, int y){
            var coordinates = new Vector2Int(x, y);
            return this.Map[coordinates].nodeType == Node.NodeType.Wall;
        }

        void SetupBaseNode(Vector2Int coordinates, BaseSide baseSide, Node.NodeType type){
            this.Map[coordinates].ResetNode();
            this.Map[coordinates].nodeType = type switch{
                Node.NodeType.EnemyBase => Node.NodeType.EnemyBase,
                Node.NodeType.PlayerBase => Node.NodeType.PlayerBase,
                _ => this.Map[coordinates].nodeType
            };

            this.Map[coordinates].faceDirection = baseSide switch{
                BaseSide.Left => Node.FaceDirections.Right,
                BaseSide.Top => Node.FaceDirections.Down,
                BaseSide.Right => Node.FaceDirections.Left,
                BaseSide.Bottom => Node.FaceDirections.Up,
                _ => this.Map[coordinates].faceDirection
            };

            ClearEntranceForPath(coordinates);
            MakeWayForEnemyBaseSides(coordinates);
        }

        enum BaseSide{
            Left,
            Top,
            Right,
            Bottom
        }

        void ClearEntranceForPath(Vector2Int coordinates){
            
            switch (this.Map[coordinates].faceDirection){
                case Node.FaceDirections.Right:{
                    this.Map[new Vector2Int(coordinates.x + 1, coordinates.y)].ResetNode();
                    break;
                }
                case Node.FaceDirections.Down:{
                    this.Map[new Vector2Int(coordinates.x, coordinates.y - 1)].ResetNode();
                    break;
                }
                case Node.FaceDirections.Left:{
                    this.Map[new Vector2Int(coordinates.x - 1, coordinates.y)].ResetNode();
                    break;
                }
                case Node.FaceDirections.Up:{
                    this.Map[new Vector2Int(coordinates.x, coordinates.y + 1)].ResetNode();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        void MakeWayForEnemyBaseSides(Vector2Int coordinates){
            
            var sidesCoordinates = new List<Vector2Int>();
            
            switch (this.Map[coordinates].faceDirection){
                
                case Node.FaceDirections.Right:{
                    SetVerticalBase(coordinates, sidesCoordinates);
                    break;
                }
                case Node.FaceDirections.Down:{
                    SetHorizontalBase(coordinates, sidesCoordinates);
                    break;
                }
                case Node.FaceDirections.Left:{
                    SetVerticalBase(coordinates, sidesCoordinates);
                    break;
                }
                case Node.FaceDirections.Up:{
                    SetHorizontalBase(coordinates, sidesCoordinates);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var side in sidesCoordinates){
                this.Map[side].nodeType = Node.NodeType.BlockedEmpty;
            }
        }

        static void SetHorizontalBase(Vector2Int coordinates, ICollection<Vector2Int> sidesCoordinates){
            sidesCoordinates.Add(new Vector2Int(coordinates.x + 1, coordinates.y));
            sidesCoordinates.Add(new Vector2Int(coordinates.x - 1, coordinates.y));
        }

        static void SetVerticalBase(Vector2Int coordinates, ICollection<Vector2Int> sidesCoordinates){
            sidesCoordinates.Add(new Vector2Int(coordinates.x, coordinates.y + 1));
            sidesCoordinates.Add(new Vector2Int(coordinates.x, coordinates.y - 1));
        }

        int GetWallNeighbours(Vector2Int nodeCoordinates){
            var wallNeighbourCount = 0;
            for (var x = nodeCoordinates.x - 1; x <= nodeCoordinates.x + 1; x++){
                for (var y = nodeCoordinates.y - 1; y <= nodeCoordinates.y + 1; y++){
                    var coordinates = new Vector2Int(x, y);
                    if (IsInsideMap(coordinates)){
                        if (x == nodeCoordinates.x && y == nodeCoordinates.y) continue;
                        if (this.Map[coordinates].nodeType == Node.NodeType.Wall){
                            wallNeighbourCount++;
                        }
                    }
                    else{
                        wallNeighbourCount++;
                    }
                }
            }

            return wallNeighbourCount;
        }
        
        bool IsEdgeNode(Vector2Int nodeCoordinates){
            return nodeCoordinates.x == 0 || nodeCoordinates.x == MapSize.x - 1 ||
                   nodeCoordinates.y == 0 || nodeCoordinates.y == MapSize.y - 1;
        }

        bool IsInsideMap(Vector2Int nodeCoordinates){
            return nodeCoordinates.x >= 0 && nodeCoordinates.x < this.MapSize.x &&
                   nodeCoordinates.y >= 0 && nodeCoordinates.y < this.MapSize.y;
        }
    }
}