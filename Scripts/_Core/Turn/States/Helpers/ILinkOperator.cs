using System.Collections.Generic;
using Enums;
using Towers;

namespace Turn
{
    public interface ILinkOperator
    {
        public LinkOperatorType Type { get; set; }
        public int[] Towers { get; set; }
        public List<TowerData> SafeGroup { get; set; } //new
        public void TowerSelected(params object[] args);
        public void SetTowers(int[] newTowers);
    }
}