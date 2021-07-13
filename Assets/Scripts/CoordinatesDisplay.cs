using UnityEngine;
using TMPro;
using Unity.VisualScripting;

[ExecuteAlways]
public class CoordinatesDisplay : MonoBehaviour{

    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.grey;
    
    TextMeshPro coordinateText;
    Vector2Int coordinates;
    Vector3 position;
    Waypoint waypoint;

    void Awake(){
        coordinateText = GetComponent<TextMeshPro>();
        coordinateText.enabled = false;
        this.waypoint = GetComponentInParent<Waypoint>();
        Display();
    }

    void Update(){
        if (!Application.isPlaying){
            Display();
            DisplayInHierarchy();  
            ToggleCoordinateText();
        }
        ColorDisplay();
        ToggleCoordinateText();
    }

    void ToggleCoordinateText(){
        if (Input.GetKeyDown(KeyCode.C)){
            coordinateText.enabled = !coordinateText.IsActive();
        }
    }

    void ColorDisplay(){
        coordinateText.color = waypoint.IsPlaceable ? defaultColor : blockedColor;
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
