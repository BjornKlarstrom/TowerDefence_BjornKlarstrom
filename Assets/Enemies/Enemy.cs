using UnityEngine;

namespace Enemies{
    public class Enemy : MonoBehaviour{
        [SerializeField] int goldReward = 25;
        [SerializeField] int goldPenalty = 25;

        Currency currency;

        void Start(){
            currency = FindObjectOfType<Currency>();
        }
        public void GainDominance(){
            if(currency == null) { return; }
            currency.Deposit(goldReward);   
        }
        public void LoseDominance(){
            if(currency == null) { return; }
            currency.Withdraw(goldPenalty);   
        }
    }
}
