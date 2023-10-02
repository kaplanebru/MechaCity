using TMPro;
using UnityEngine;

namespace Towers
{
    public class TowerUIHandler : MonoBehaviour
    {
        public TextMeshPro heightText;
        public TextMeshPro healthIndicator;

        private void OnEnable() //TODO: tower scriptinden yönet
        {
            Eventbus.UIEvents.OnTowerHeightChange += ChangeHeightUI;
            Eventbus.UIEvents.OnHealthChange += AdjustHealthUI;
        }

        private void AdjustHealthUI(int health, Tower tower)
        {
            if (tower.gameObject != gameObject) return;

            healthIndicator.text = health.ToString();
        }

        void ChangeHeightUI(float height, GameObject obj) //DoTween
        {
            if (obj != gameObject) return;

            int heightInt = Mathf.RoundToInt(height);
            heightText.text = heightInt.ToString();
        }

        private void OnDisable()
        {
            Eventbus.UIEvents.OnTowerHeightChange -= ChangeHeightUI;
            Eventbus.UIEvents.OnHealthChange -= AdjustHealthUI;
        }

        // void AdjustHealthIndicatorPosition(float height)
        // {
        //    var pos = healthIndicator.transform.localPosition;
        //    pos.y = height;
        //    healthIndicator.transform.localPosition = pos;
        // }
    }
}