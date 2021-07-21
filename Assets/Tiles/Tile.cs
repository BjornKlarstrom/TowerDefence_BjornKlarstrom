using System;
using PathFinding;
using UnityEngine;

namespace Tiles{
    public class Tile : MonoBehaviour{
        [SerializeField] Tower towerPrefab;
        [SerializeField] bool isPlaceable;
        public bool IsPlaceable => isPlaceable;

        GridManager gridManager;
        Vector2Int coordinates = new Vector2Int();

        void Awake(){
            this.gridManager = FindObjectOfType<GridManager>();
        }

        void Start(){
            if (this.gridManager != null){
                this.coordinates = gridManager.GetCoordinatesFromPosition(this.transform.position);
                if (!isPlaceable){
                    gridManager.BlockNode(coordinates);
                }
            }
        }

        void OnMouseDown(){
            if (!isPlaceable) return;
            var isPlaced = Tower.CreateTower(towerPrefab, transform.position);
            isPlaceable = !isPlaced;
        }
    }
}