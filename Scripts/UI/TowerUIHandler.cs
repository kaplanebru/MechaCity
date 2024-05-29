using TMPro;
using UnityEngine;

namespace GameUI
{
    public class TowerUIHandler : MonoBehaviour
    {
        public TextMeshPro heightText;
        public TextMeshPro healthIndicator;

        private void OnEnable() //TODO: tower scriptinden yönet
        {
            UIEventbus.OnTowerHeightChange += ChangeHeightUI;
            //UIEventbus.OnHealthChange += AdjustHealthUI;
        }

        private void AdjustHealthUI(int health, GameObject towerGameObject)
        {
            if (towerGameObject != gameObject) return;

            healthIndicator.text = health.ToString();
        }

        void ChangeHeightUI(float height, GameObject obj) //DoTween
        {
            if (obj != gameObject) return;

            int heightInt = Mathf.FloorToInt(height); //todo: temporary
            heightText.text = heightInt.ToString();
        }

        private void OnDisable()
        {
            UIEventbus.OnTowerHeightChange -= ChangeHeightUI;
            //UIEventbus.OnHealthChange -= AdjustHealthUI;
        }

        // void AdjustHealthIndicatorPosition(float height)
        // {
        //    var pos = healthIndicator.transform.localPosition;
        //    pos.y = height;
        //    healthIndicator.transform.localPosition = pos;
        // }
    }
}