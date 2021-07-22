using System;
using PathFinding;
using UnityEngine;

namespace Tiles{
    public class Tile : MonoBehaviour{
        [SerializeField] Tower towerPrefab;
        [SerializeField] bool isPlaceable;
        public bool IsPlaceable => isPlaceable;

        GridManager gridManager;
        Pathfinder pathfinder;
        Vector2Int coordinates;

        void Awake(){
            this.gridManager = FindObjectOfType<GridManager>();
            this.pathfinder = FindObjectOfType<Pathfinder>();
        }

        void Start(){
            if (this.gridManager == null) return;
            this.coordinates = gridManager.GetCoordinatesFromPosition(this.transform.position);
            if (!isPlaceable){
                gridManager.BlockNode(coordinates);
            }
        }

        void OnMouseDown(){
            if (!gridManager.GetNode(coordinates).isWalkable || pathfinder.WillBlockPath(coordinates)) return;
            var isPlaced = Tower.CreateTower(towerPrefab, transform.position);
            isPlaceable = !isPlaced; 
            gridManager.BlockNode(coordinates);
        }
    }
}