using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HN.Code.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Image image, blinkImage;
        [SerializeField] private Sprite activeSprite, unActiveSprite;
        [SerializeField] private float blinkTime;

        private WaitForSeconds _seconds;

        private void Awake()
        {
            _seconds = new WaitForSeconds(blinkTime);
        }

        public void SetActive(bool isActive, bool isBlink = false)
        {
            image.sprite = isActive ? activeSprite : unActiveSprite;
            
            if(isBlink == false) return;

            StartCoroutine(BlinkCoroutine());
        }

        private IEnumerator BlinkCoroutine()
        {
            blinkImage.enabled = true;
            
            yield return _seconds;
            
            blinkImage.enabled = false;
        }
    }
}