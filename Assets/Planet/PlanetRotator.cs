using System;
using UnityEngine;

namespace Planet{
    public class PlanetRotator : MonoBehaviour{
        [SerializeField] float rotateSpeed = 1f;

        void Update(){
            this.transform.Rotate(Vector3.left * (rotateSpeed * Time.deltaTime));
        }
    }
}
