using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour{
    [SerializeField] int startingBalance = 200;
    [SerializeField] int currentBalance;
    public int CurrentBalance => currentBalance;

    void Awake(){
        this.currentBalance = this.startingBalance;
    }

    public void Deposit(int amount){
        currentBalance += Mathf.Abs(amount);
    }

    public void Withdraw(int amount){
        currentBalance -= Mathf.Abs(amount);
    }
}
