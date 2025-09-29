using System;
using Enums;

namespace Towers
{
    [Serializable]
    public class TowerNumericData
    {
        public int UniqID;

        private int height;

        public int Height
        {
            get => height;
            set
            {
                height = value;
                if (!LockStatus.Locked)
                {
                    AvailableHeight = value - 1; //-1ler yeni eklendi
                }
                else
                {
                    AvailableHeight = value - 1 - LockStatus.Limit + 1; //+1 limiti sıfırlayabilmek için
                }
            }
        }

        public int AvailableHeight { get; set; }

        public int ShotAmount { get; set; } = 1;
        public int ShieldHeight { get; set; }= 0;
        public TeamType TeamType { get; set; }
        public LockStatus LockStatus { get; set; }

        public int DamagePower;
    }
}