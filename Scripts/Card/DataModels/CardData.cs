using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModels
{
    [CreateAssetMenu(fileName = nameof(CardData))]
    public class CardData : ScriptableObject
    {
        public Color color;
    }

}
