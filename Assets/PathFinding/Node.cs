using System;
using UnityEngine;

namespace PathFinding{
    [Serializable]
    public class Node{
        public Vector2Int coordinates;
        public bool isWalkable;
        public bool isExplored;
        public bool isPath;
        public bool isWall;
        public Node connectedTo;

        public Node(Vector2Int coordinates, bool isWalkable){
            this.coordinates = coordinates;
            this.isWalkable = isWalkable;
        }
    }
}
