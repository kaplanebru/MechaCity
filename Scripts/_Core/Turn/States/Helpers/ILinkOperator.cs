using System.Collections.Generic;
using Enums;
using Towers;

namespace Turn
{
    public interface ILinkOperator
    {
        public LinkOperatorType Type { get; set; }
        public void TowerSelected(params object[] args);
        public void SetTowers(uint[] actors);
    }
}