
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(BlueprintData))]
    public class BlueprintData : ScriptableObject
    {
        public Color Color;
        public Sprite Sprite;
        public Color GlowColor;
        public string Title;
        public string Description;
    }

}
