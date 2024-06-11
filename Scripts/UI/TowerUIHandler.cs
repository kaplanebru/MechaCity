using TMPro;
using UnityEngine;

namespace GameUI
{
    public class TowerUIHandler : MonoBehaviour, ITowerSegment
    {
        public TextMeshPro[] heightTexts;
        public int Id { get; set; }

        public void SetId(int id)
        {
            Id = id;
        }

        public void Initialize() {}

       
        public void ChangeHeightUI(float height) 
        {
            int heightInt = Mathf.FloorToInt(height); //todo: temporary
            foreach (var heightText in heightTexts)
            {
                if(heightText != null)
                    heightText.text = heightInt.ToString();
            }
        }
    }
}