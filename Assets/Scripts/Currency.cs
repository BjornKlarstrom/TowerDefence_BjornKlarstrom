using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Currency : MonoBehaviour{
    [SerializeField] int startingCurrency = 500;
    [SerializeField] int currentBalance;
    public int CurrentBalance => currentBalance;

    [SerializeField] TextMeshProUGUI goldText;

    void Awake(){
        this.currentBalance = this.startingCurrency;
        UpdateCurrencyText();
    }

    public void Deposit(int amount){
        currentBalance += Mathf.Abs(amount);
        UpdateCurrencyText();

        if (currentBalance >= 1000){
            ReloadCurrentScene();
        }
    }

    public void Withdraw(int amount){
        currentBalance -= Mathf.Abs(amount);
        UpdateCurrencyText();

        if (currentBalance < 0){
            ReloadCurrentScene();
        }
    }

    void UpdateCurrencyText(){
        this.goldText.text = "Currency: " + currentBalance;
    }

    static void ReloadCurrentScene(){
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
