using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour{
    [SerializeField] Transform weapon;
    Transform target;

    void Start(){
        target = FindObjectOfType<Enemy>().transform;
    }

    void Update(){
        AimAtTarget();
    }

    void AimAtTarget(){
        weapon.LookAt(target);
    }
}
