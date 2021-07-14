using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour{
    [SerializeField] Transform weapon;
    Transform target;
    
    void Update(){
        AimAtTarget();
    }

    void AimAtTarget(){
        weapon.LookAt(target);
    }

    void FindClosestTarget(){
        var enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        var maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies){
            var targetDistance = Vector3.Distance(this.transform.position, enemy.transform.position);
        }
    }
}
