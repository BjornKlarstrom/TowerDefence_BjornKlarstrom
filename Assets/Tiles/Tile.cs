using UnityEngine;

namespace Tiles{
    public class Tile : MonoBehaviour{

        [SerializeField] Tower towerPrefab;
        [SerializeField] bool isPlaceable;
        public bool IsPlaceable => isPlaceable;

        void OnMouseDown(){
            if (!isPlaceable) return;
            var isPlaced = Tower.CreateTower(towerPrefab, transform.position);
            isPlaceable = !isPlaced;
        }
    }
}
