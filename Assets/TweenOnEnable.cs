using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOnEnable : MonoBehaviour
{
    public float tweenTime = 1;

    void OnEnable(){
        Tween();
    }

    void Tween(){
        LeanTween.cancel(gameObject);
        this.transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 1.45f, tweenTime).setEasePunch();
    }
}
