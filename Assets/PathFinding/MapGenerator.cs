using System.Globalization;
using UnityEngine;

namespace PathFinding{
    public class MapGenerator : MonoBehaviour{
        [SerializeField] int width;
        [SerializeField] int height;

        [SerializeField] string seed;
        [SerializeField] bool useRandomSeed;

        [Range(0, 100)] [SerializeField] int fillFactor;
        int[,] map;

        [SerializeField] int smoothingIterations = 3;

        void Start(){
            GenerateMap();
        }

        void Update(){
        
        }

        public void GenerateMap(){
            this.map = new int[width, height];
            RandomFillMap();
            for (var i = 0; i < smoothingIterations; i++){
                SmoothMap();
            }
        }

        void RandomFillMap(){
            if (useRandomSeed){
                this.seed = Time.time.ToString(CultureInfo.InvariantCulture);
            }

            var randomHashCode = new System.Random(seed.GetHashCode());

            for (var x = 0; x < width; x++){
                for (var y = 0; y < height; y++){
                    if (IsEdgeNode(x, y)){
                        this.map[x, y] = 1;
                    }
                    else{
                        this.map[x, y] = randomHashCode.Next(0, 100) < fillFactor ? 1 : 0;
                    }
                }
            }
        }

        void OnDrawGizmos(){
            if (map == null) return;
            for (var x = 0; x < width; x++){
                for (var y = 0; y < height; y++){
                    Gizmos.color = this.map[x, y] == 1 ? Color.grey : Color.green;
                    var position = new Vector3(-width / 2 + x + 0.5f, 0.0f, -height / 2 + y + 0.5f);
                    Gizmos.DrawCube(position, Vector3.one);
                }
            }
        }

        bool IsEdgeNode(int x, int y){
            return x == 0 || x == width - 1 || y == 0 || y == height - 1;
        }
    
        void SmoothMap(){
            for (var x = 0; x < width; x++){
                for (var y = 0; y < height; y++){
                    var blockedNeighbours = GetBlockedNeighbours(x, y);
                    if (blockedNeighbours > 4){
                        this.map[x, y] = 1;
                    }
                    else if(blockedNeighbours < 4){
                        this.map[x, y] = 0;
                    }
                }
            }
        }

        int GetBlockedNeighbours(int gridX, int gridY){
            var blockedNodesCount = 0;
            for (var neighbourX  = gridX - 1; neighbourX  <= gridX + 1; neighbourX ++){
                for (var neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++){
                    if (neighbourX >= 0 && neighbourX < this.width && neighbourY >= 0 && neighbourY < this.height){
                        if (neighbourX  != gridX || neighbourY != gridY){
                            blockedNodesCount += this.map[neighbourX , neighbourY];
                        }   
                    }
                    else{
                        blockedNodesCount++;
                    }
                }
            }
            return blockedNodesCount;
        }

        bool IsInsideMap(int x, int y){
            return x >= 0 && x < this.width && y >= 0 && y < this.height;
        }
    }
}
