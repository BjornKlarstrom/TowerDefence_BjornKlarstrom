using System;
using UnityEngine;

namespace UI{
    public class TweenText : MonoBehaviour{
        public float tweenTime;

        void Start(){
            Tween();
        }

        void Tween(){
            LeanTween.cancel(gameObject);
            this.transform.localScale = Vector3.one;
            LeanTween.scale(gameObject, Vector3.one * 1.1f, tweenTime).setEasePunch();
        }
    }
}
