using System;
using UnityEngine;

namespace PathFinding{
    [Serializable]
    public class Node{
        public Vector2Int position;
        public bool isWalkable;
        public bool isExplored;
        public bool isPath;
        public Node connectedTo;
        public FaceDirections faceDirection;
        public NodeType nodeType;

        public enum FaceDirections{
            Left,
            Right,
            Up,
            Down
        }
        
        public enum NodeType{
            Floor,
            Wall,
            Rock,
            EnemyBase,
            PlayerBase,
            BlockedEmpty
        }
        
        public Node(Vector2Int position, bool isWalkable, NodeType nodeType){
            this.position = position;
            this.isWalkable = isWalkable;
            this.nodeType = nodeType;
        }

        public void ResetNode(){
            this.nodeType = NodeType.Floor;
            this.isWalkable = true;
        }
    }
}
