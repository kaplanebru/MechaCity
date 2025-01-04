using System;
using TMPro;
using UnityEngine;

namespace GameUI
{
    [Serializable]
    public class TowerUIData : TowerSegmentData
    {
        public TextMeshPro[] HeightTexts;
        public TextMeshPro IDText;
        public CommonData CommonData;
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

        public void Initialize()
        {
            SetIDText();
        }
        
        public void ChangeHeightUI(int height)
        {
            //int heightInt = Mathf.FloorToInt(height / Data.CommonData.TowerHeightPerStep); //todo: later
            foreach (var heightText in Data.HeightTexts)
            {
                if (heightText != null)
                    heightText.text = height.ToString(); //heightInt.ToString();
            }
        }

        private void SetIDText()
        {
            Data.IDText.text = RomanNumberConverter.IntToRoman(Id + 1);
        }
    }
}