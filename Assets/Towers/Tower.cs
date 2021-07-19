using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour{
    [SerializeField] const int cost = 50;

    public static bool CreateTower(Tower tower, Vector3 position){
        var bank = FindObjectOfType<Bank>();

        if (bank == null){
            return false;
        }

        if (bank.CurrentBalance < cost) return false;
        Instantiate(tower.gameObject, position, Quaternion.identity);
        bank.Withdraw(cost);
        return true;
    }
}
