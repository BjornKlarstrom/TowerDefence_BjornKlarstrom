using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour{
    [SerializeField] int startingBalance = 200;
    [SerializeField] int currentBalance;
    public int CurrentBalance => currentBalance;

    [SerializeField] TextMeshProUGUI goldText;

    void Awake(){
        this.currentBalance = this.startingBalance;
        UpdateGoldText();
    }

    public void Deposit(int amount){
        currentBalance += Mathf.Abs(amount);
        UpdateGoldText();
    }

    public void Withdraw(int amount){
        currentBalance -= Mathf.Abs(amount);
        UpdateGoldText();

        if (currentBalance < 0){
            ReloadScene();
        }
    }

    void UpdateGoldText(){
        this.goldText.text = "Gold: " + currentBalance;
    }

    static void ReloadScene(){
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
