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
            PlaceEnemyBase();
            return Map;
        }

        void RandomFillMap(){
            if (useRandomSeed){
                this.seed = Time.time.ToString(CultureInfo.InvariantCulture);
            }

            var randomHashCode = new System.Random(seed.GetHashCode());

            for (var x = 0; x < MapSize.x; x++){
                for (var y = 0; y < MapSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    if (IsEdgeNode(coordinates)){
                        this.Map.Add(coordinates, new Node(coordinates, false, true, false));
                    }
                    else{
                        this.Map.Add(coordinates, new Node(coordinates, true, false, false));
                        this.Map[coordinates].isWall = randomHashCode.Next(0, 100) < wallPercent;
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
                        this.Map[coordinates].isWall = true;
                    }
                    else if (blockedNeighbours < 4){
                        //this.Map[coordinates].ResetNode();
                        this.Map[coordinates].isWall = false;
                    }
                }
            }
        }

        void PlaceRocks(){
            var random = new System.Random();
            for (var x = 0; x < MapSize.x; x++){
                for (var y = 0; y < MapSize.y; y++){
                    var coordinates = new Vector2Int(x, y);
                    this.Map[coordinates].isRock = random.Next(0, 100) < rockPercent;
                }
            }
        }

        void PlaceEnemyBase(){
            Vector2Int enemyBaseCoordinates;
            var random = new System.Random();
            var enemyBaseSide = random.Next(0, 4);
            Debug.Log(enemyBaseSide);

            switch (enemyBaseSide){
                case 0:{
                    for (var x = 0; x < MapSize.x; x++){
                        for (var y = 0; y < MapSize.y; y++){
                            var coordinates = new Vector2Int(x, y);
                            if (this.Map[coordinates].isWall) continue;
                            this.Map[coordinates].ResetNode();
                            this.Map[coordinates].isEnemyBase = true;
                            this.Map[coordinates].currentDirection = Node.FaceDirections.Right;
                            return;
                        }
                    }
                    break;
                }
                case 1:{
                    for (var y = MapSize.y - 1; y >= 0; y--){
                        for (var x = 0; x < MapSize.x; x++){
                            var coordinates = new Vector2Int(x, y);
                            if (this.Map[coordinates].isWall) continue;
                            this.Map[coordinates].ResetNode();
                            this.Map[coordinates].isEnemyBase = true;
                            this.Map[coordinates].currentDirection = Node.FaceDirections.Down;
                            return;
                        }
                    }
                    break;
                }
                
                case 2:{
                    for (var x = MapSize.x - 1; x > 0; x--){
                        for (var y = 0; y < MapSize.y; y++){
                            var coordinates = new Vector2Int(x, y);
                            if (this.Map[coordinates].isWall) continue;
                            this.Map[coordinates].ResetNode();
                            this.Map[coordinates].isEnemyBase = true;
                            this.Map[coordinates].currentDirection = Node.FaceDirections.Left;
                            return;
                        }
                    }
                    break;
                }
                
                case 3:{
                    for (var y = 0; y < MapSize.x; y++){
                        for (var x = 0; x < MapSize.y; x++){
                            var coordinates = new Vector2Int(x, y);
                            if (this.Map[coordinates].isWall) continue;
                            this.Map[coordinates].ResetNode();
                            this.Map[coordinates].isEnemyBase = true;
                            this.Map[coordinates].currentDirection = Node.FaceDirections.Up;
                            return;
                        }
                    }
                    break;
                }
            }
        }
        
        int GetWallNeighbours(Vector2Int nodeCoordinates){
            var wallNeighbourCount = 0;
            for (var x = nodeCoordinates.x - 1; x <= nodeCoordinates.x + 1; x++){
                for (var y = nodeCoordinates.y - 1; y <= nodeCoordinates.y + 1; y++){
                    var coordinates = new Vector2Int(x, y);
                    if (IsInsideMap(coordinates)){
                        if (x == nodeCoordinates.x && y == nodeCoordinates.y) continue;
                        if (this.Map[coordinates].isWall){
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