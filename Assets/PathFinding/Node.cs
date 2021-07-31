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
        public bool isRock;
        public bool isEnemyBase;
        public Node connectedTo;
        public FaceDirections currentDirection;

        public enum FaceDirections{
            Left,
            Right,
            Up,
            Down
        }
        
        public Node(Vector2Int position, bool isWalkable, bool isWall, bool isRock){
            this.position = position;
            this.isWalkable = isWalkable;
            this.isWall = isWall;
            this.isRock = isRock;
        }

        public void ResetNode(){
            this.isRock = false;
            this.isWall = false;
            this.isWalkable = true;
        }
    }
}
