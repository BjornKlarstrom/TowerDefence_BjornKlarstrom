using UnityEngine;
using TMPro;
using Unity.VisualScripting;

[ExecuteAlways]
public class CoordinatesDisplay : MonoBehaviour{
    
    TextMeshPro coordinateText;
    Vector2Int coordinates;
    Vector3 position;

    void Awake(){
        coordinateText = GetComponent<TextMeshPro>();
        Display();
    }

    void Update(){
        if (Application.isPlaying) return;
        Display();
        DisplayInHierarchy();
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
