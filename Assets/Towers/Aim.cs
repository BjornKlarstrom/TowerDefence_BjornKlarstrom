using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class Aim : MonoBehaviour{
    [SerializeField] Transform weapon;
    [SerializeField] float shootingRange = 10.0f;
    [SerializeField] ParticleSystem bulletParticle;
    Transform target;
    
    void Update(){
        FindClosestTarget();
        AimAtTarget();
    }

    void AimAtTarget(){
        if (target == null) return;
        var targetDistance = Vector3.Distance(this.transform.position, target.position);
        weapon.LookAt(target);   
        Shoot(targetDistance <= shootingRange);
    }

    void FindClosestTarget(){
        var enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        var maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies){
            var targetDistance = Vector3.Distance(this.transform.position, enemy.transform.position);

            if (!(targetDistance < maxDistance)) continue;
            closestTarget = enemy.transform;
            maxDistance = targetDistance;
        }

        if (closestTarget is { }) this.target = closestTarget.transform;
    }

    void Shoot(bool isActive){
        var emission = bulletParticle.emission;
        emission.enabled = isActive;
    }
}
