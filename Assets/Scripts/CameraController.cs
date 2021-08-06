using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float tweenTime = 1;
    [SerializeField] Transform zoomTarget;

    public void ZoomIn(){
        LeanTween.move(this.gameObject, zoomTarget, tweenTime);
    }
}
