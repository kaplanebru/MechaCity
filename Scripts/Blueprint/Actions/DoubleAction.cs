
using Towers;

namespace Blueprint
{
    public class DoubleAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            var selectedTowers = (int[]) obj[0];

            foreach (var selectedTower in selectedTowers)
            {
                var tower = AllTowers.GetTower(selectedTower);
                tower.ToOriginalColor();
                //tower.DisableSelection();
            }
        }
        public void Restore(params object[] obj)
        {
           //sonsuza kadar double kalacaksa gerek yok
        }
    }
}
