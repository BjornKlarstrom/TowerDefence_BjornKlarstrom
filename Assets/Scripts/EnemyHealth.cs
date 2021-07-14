using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour{
    [SerializeField] int maxHealth = 5;
    int currentHealth = 0;

    void OnEnable(){
        this.currentHealth = this.maxHealth;
        Debug.Log(currentHealth);
    }

    void OnParticleCollision(GameObject other){
        HandleHit();
    }

    void HandleHit(){
        currentHealth--;

        if (currentHealth <= 0){
            this.gameObject.SetActive(false);
        }
    }
}
