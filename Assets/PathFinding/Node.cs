using System;
using UnityEngine;

namespace PathFinding{
    [Serializable]
    public class Node{
        public Vector2Int position;
        public bool isWalkable;
        public bool isExplored;
        public bool isPath;
        //public bool isWall;
        //public bool isRock;
        public bool isEnemyBase;
        public Node connectedTo;
        public FaceDirections faceDirection;
        public Type type;

        public enum FaceDirections{
            Left,
            Right,
            Up,
            Down
        }
        
        public enum Type{
            Floor,
            Wall,
            Rock,
            EnemyBase
        }
        
        public Node(Vector2Int position, bool isWalkable, Type type){
            this.position = position;
            this.isWalkable = isWalkable;
            this.type = type;
        }

        public void ResetNode(){
            this.type = Type.Floor;
            this.isWalkable = true;
        }
    }
}
