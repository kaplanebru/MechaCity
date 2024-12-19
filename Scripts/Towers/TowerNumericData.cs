using System;
using Enums;

namespace Towers
{
    [Serializable]
    public class TowerNumericData
    {
        public int UniqID;
        public int Height { get; set; }
        public int ShotAmount { get; set; } = 1;
        public int ShieldHeight { get; set; }= 0;
        public TeamType TeamType { get; set; }
        public LockStatus LockStatus { get; set; }
    }
}