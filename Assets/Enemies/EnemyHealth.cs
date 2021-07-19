using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour{
    
    [SerializeField] int maxHealth = 5;
    
    [Tooltip("Adds this amount to enemy maxHealth when enemy dies")]
    [SerializeField] int difficultyRamp = 1;
    int currentHealth;
    Enemy enemy;

    void Start(){
        enemy = GetComponent<Enemy>();
    }

    void OnEnable(){
        this.currentHealth = this.maxHealth;
    }

    void OnParticleCollision(GameObject other){
        HandleHit();
    }

    void HandleHit(){
        currentHealth--;

        if (currentHealth > 0) return;
        this.gameObject.SetActive(false);
        maxHealth += difficultyRamp;
        enemy.RewardGold();
    }
}
