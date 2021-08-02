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
            PlaceBases();
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

        bool IsEdgeNode(Vector2Int nodeCoordinates){
            return nodeCoordinates.x == 0 || nodeCoordinates.x == MapSize.x - 1 ||
                   nodeCoordinates.y == 0 || nodeCoordinates.y == MapSize.y - 1;
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
                    if (random.Next(0, 100) < rockPercent && this.Map[coordinates].nodeType != Node.NodeType.Wall){
                        this.Map[coordinates].nodeType = Node.NodeType.Rock;
                    }
                }
            }
        }

        void PlaceBases(){

            var values = Enum.GetValues(typeof(BaseSide));
            var random = new System.Random();
            var randomSide = (BaseSide)values.GetValue(random.Next(values.Length));

            switch (randomSide){
                case BaseSide.Left:{
                    for (var x = 0; x < MapSize.x; x++){
                        for (var y = 0; y < MapSize.y; y++){
                            if (PlaceOnRandomSide(x, y)) continue;
                            return;
                        }
                    }
                    break;
                }
                case BaseSide.Top:{
                    for (var y = MapSize.y - 1; y >= 0; y--){
                        for (var x = 0; x < MapSize.x; x++){
                            if (PlaceOnRandomSide(x, y)) continue;
                            return;
                        }
                    }
                    break;
                }
                
                case BaseSide.Right:{
                    for (var x = MapSize.x - 1; x > 0; x--){
                        for (var y = 0; y < MapSize.y; y++){
                            if (PlaceOnRandomSide(x, y)) continue;
                            return;
                        }
                    }
                    break;
                }
                
                case BaseSide.Bottom:{
                    for (var y = 0; y < MapSize.x; y++){
                        for (var x = 0; x < MapSize.y; x++){
                            if (PlaceOnRandomSide(x, y)) continue;
                            return;
                        }
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        bool PlaceOnRandomSide(int x, int y){
            var coordinates = new Vector2Int(x, y);
            if (this.Map[coordinates].nodeType == Node.NodeType.Wall) return true;
            this.Map[coordinates].ResetNode();
            this.Map[coordinates].nodeType = Node.NodeType.EnemyBase;
            this.Map[coordinates].faceDirection = Node.FaceDirections.Right;
            ClearEntranceForPath(coordinates);
            MakeWayForEnemyBaseSides(coordinates);
            return false;
        }

        enum BaseSide{
            Left = 0,
            Top = 1,
            Right = 2,
            Bottom = 3
        }

        void ClearEntranceForPath(Vector2Int coordinates){
            
            switch (this.Map[coordinates].faceDirection){
                case Node.FaceDirections.Right:{
                    Debug.Log("Right");
                    this.Map[new Vector2Int(coordinates.x + 1, coordinates.y)].ResetNode();
                    break;
                }
                case Node.FaceDirections.Down:{
                    Debug.Log("Down");
                    this.Map[new Vector2Int(coordinates.x, coordinates.y - 1)].ResetNode();
                    break;
                }
                case Node.FaceDirections.Left:{
                    Debug.Log("Left");
                    this.Map[new Vector2Int(coordinates.x - 1, coordinates.y)].ResetNode();
                    break;
                }
                case Node.FaceDirections.Up:{
                    Debug.Log("Up");
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

        bool IsInsideMap(Vector2Int nodeCoordinates){
            return nodeCoordinates.x >= 0 && nodeCoordinates.x < this.MapSize.x &&
                   nodeCoordinates.y >= 0 && nodeCoordinates.y < this.MapSize.y;
        }
    }
}