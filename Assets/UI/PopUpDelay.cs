using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace UI{
    public class PopUpDelay : MonoBehaviour
    {
        [SerializeField] float generateButtonTime = 0.2f;
        [SerializeField] float acceptButtonTime = 0.5f;
        [SerializeField] GameObject generateLocationButton;
        [SerializeField] GameObject acceptButton;

        void Start(){
            StartCoroutine(ShowGenerateLocationButton());
            StartCoroutine(ShowAcceptButton());
        }

        IEnumerator ShowGenerateLocationButton(){
            yield return new WaitForSeconds(generateButtonTime);
            this.generateLocationButton.SetActive(true);
        }
    
        IEnumerator ShowAcceptButton(){
            yield return new WaitForSeconds(acceptButtonTime);
            this.acceptButton.SetActive(true);
        }
    }
}
