using PathFinding;
using TMPro;
using UnityEngine;

namespace Tiles{
    [ExecuteAlways]
    [RequireComponent(typeof(TextMeshPro))]
    public class CoordinatesDisplay : MonoBehaviour{

        [SerializeField] Color defaultColor = Color.white;
        [SerializeField] Color blockedColor = Color.gray;
        [SerializeField] Color exploredColor = Color.yellow;
        [SerializeField] Color pathColor = new Color(1.0f, 0.5f, 0.0f);
    
        TextMeshPro coordinateText;
        Vector2Int coordinates;
        Vector3 position;

        GridManager gridManager;

        void Awake(){
            gridManager = FindObjectOfType<GridManager>();
            coordinateText = GetComponent<TextMeshPro>();
            coordinateText.enabled = true;
            Display();
        }

        void Update(){
            if (!Application.isPlaying){
                Display();
                DisplayInHierarchy();
            }
            SetTextColor();
            ToggleCoordinateText();
        }

        void ToggleCoordinateText(){
            if (Input.GetKeyDown(KeyCode.C)){
                coordinateText.enabled = !coordinateText.IsActive();
            }
        }

        void SetTextColor(){
            if (gridManager == null) return;
            var node = gridManager.GetNode(coordinates);
            if (node == null) return;

            if (!node.isWalkable){
                this.coordinateText.color = blockedColor;
            }
            else if (node.isPath){
                this.coordinateText.color = pathColor;
            }
            else if (node.isExplored){
                this.coordinateText.color = exploredColor;
            }
            else{
                this.coordinateText.color = defaultColor;
            }
        }

        void Display(){
            position = this.transform.position;
            coordinates.x = Mathf.RoundToInt(position.x / UnityEditor.EditorSnapSettings.move.x);
            coordinates.y = Mathf.RoundToInt(position.z / UnityEditor.EditorSnapSettings.move.z);
            coordinateText.text = coordinates.x + "," + coordinates.y;
        }

        void DisplayInHierarchy(){
            this.transform.parent.name = coordinates.x + "," + coordinates.y;
        }
    }
}
