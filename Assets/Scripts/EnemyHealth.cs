using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour{
    [SerializeField] int maxHealth = 5;
    int currentHealth;
    Enemy enemy;

    void Start(){
        enemy = GetComponent<Enemy>();
    }

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
            enemy.RewardGold();
        }
    }
}
