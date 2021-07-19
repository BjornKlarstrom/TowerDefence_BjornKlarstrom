using UnityEngine;

namespace PathFinding{
    public class GridManager : MonoBehaviour{
        [SerializeField] Node node;
        void Start(){
            Debug.Log(node.coordinates);
            Debug.Log(node.isWalkable);
        }
    }
}
