using TMPro;
using UnityEngine;

namespace GameUI
{
    public class TowerUIHandler : MonoBehaviour
    {
        public TextMeshPro[] heightTexts;
        public SpriteRenderer Sun;
        
        private void OnEnable() //TODO: tower scriptinden yönet
        {
            UIEventbus.OnTowerHeightChange += ChangeHeightUI;
        }

        void ChangeHeightUI(float height, GameObject obj) //DoTween
        {
            if (obj != gameObject) return;

            int heightInt = Mathf.FloorToInt(height); //todo: temporary
            foreach (var heightText in heightTexts)
            {
                if(heightText != null)
                    heightText.text = heightInt.ToString();
            }
        }

        private void OnDisable()
        {
            UIEventbus.OnTowerHeightChange -= ChangeHeightUI;
        }
    }
}