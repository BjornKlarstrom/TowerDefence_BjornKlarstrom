using System;
using System.Collections;
using UnityEngine;

namespace Towers{
    public class Tower : MonoBehaviour{
        [SerializeField] const int cost = 50;
        [SerializeField] float buildDelayTime = 1.0f;

        void Start(){
            StartCoroutine(Build());
        }

        public bool CreateTower(Tower tower, Vector3 position){
            var bank = FindObjectOfType<Bank>();

            if (bank == null){
                return false;
            }

            if (bank.CurrentBalance < cost) return false;
            Instantiate(tower.gameObject, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }
        
        IEnumerator Build(){
            foreach (Transform child in transform){
                child.gameObject.SetActive(false);
                foreach (Transform grandchild in child){
                    grandchild.gameObject.SetActive(false);
                }
            }
            foreach (Transform child in transform){
                child.gameObject.SetActive(true);
                yield return new WaitForSeconds(buildDelayTime);
                foreach (Transform grandchild in child){
                    grandchild.gameObject.SetActive(true);
                }
            }
        }
    }
}
