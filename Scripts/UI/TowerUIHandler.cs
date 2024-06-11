using System;
using TMPro;
using UnityEngine;

namespace GameUI
{
    [Serializable]
    public class TowerUIData : TowerSegmentData
    {
        public TextMeshPro[] HeightTexts;
    }
    public class TowerUIHandler : ITowerSegment
    {
        private TowerUIData Data;
        public TowerUIHandler(TowerSegmentData data)
        {
            Data = data as TowerUIData;
        }
        
        public int Id { get; set; }

        public void SetId(int id)
        {
            Id = id;
        }

        public void Initialize() {}
        
        public void ChangeHeightUI(float height) 
        {
            int heightInt = Mathf.FloorToInt(height); //todo: temporary
            foreach (var heightText in Data.HeightTexts)
            {
                if(heightText != null)
                    heightText.text = heightInt.ToString();
            }
        }
    }
}