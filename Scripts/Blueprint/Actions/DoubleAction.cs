
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class DoubleAction : IBpAction
    {
        DoubleWithRival doubleWithRival = new DoubleWithRival();

        public void Execute(params object[] obj)
        {
            Debug.Log("execute bp");
            var selectedTowers = (int[]) obj[0];

            foreach (var selectedTower in selectedTowers)
            {
                var tower = AllTowers.GetData(selectedTower);
                doubleWithRival.HighlightNeighbours(tower.UniqID);
                // tower.ColorHandler.ToOriginalColor();

            }
        }
        public void Restore(params object[] obj)
        {
           //sonsuza kadar double kalacaksa gerek yok
        }
    }
}
