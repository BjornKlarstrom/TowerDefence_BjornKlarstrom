using System;
using UnityEngine;

namespace PathFinding{
    [Serializable]
    public class Node{
        public Vector2Int position;
        public bool isWalkable;
        public bool isExplored;
        public bool isPath;
        public bool isWall;
        public Node connectedTo;

        public Node(Vector2Int position, bool isWalkable, bool isWall){
            this.position = position;
            this.isWalkable = isWalkable;
            this.isWall = isWall;
        }
    }
}
