
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
        public string Title;
        public string Description;
        public string Instruction;

    }

}
