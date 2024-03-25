
using Enums;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(BlueprintData))]
    public class BlueprintData : ScriptableObject
    {
        public BpType Type;

        //Teams tutabilir: ordan towerlara ulaşılır. Eğer tower seçilecekse bp selection state'i olmalı.
        
        [Header("Slot Data")]
        public Color Color;
        public Sprite Sprite;
        public Color GlowColor;
        
        //TODO: değişken dataya göre belirlenir
        public string Title; //örn 2 tur 1 tower freeze
        public string Description;
        public string Instruction;
        
        //Değişken Data
        [Header("Turn Related Data")]
        public int Lifespan = 1;
        public int MaxTower = 1;
        public int Level = 1;

    }

}
