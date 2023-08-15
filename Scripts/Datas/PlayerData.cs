using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(PlayerData))]

    public class PlayerData : ScriptableObject
    {
        public GameGrid Grid;
        public PlayerData RivalData;
    }
}