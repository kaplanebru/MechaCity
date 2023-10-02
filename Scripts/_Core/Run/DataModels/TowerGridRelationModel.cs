using Grid;
using Towers;

namespace DataModels
{
    public class TowerGridRelationModel
    {
        public GameGrid Grid { get; }
        public Tower Tower { get; }
        public TowerGridRelationModel(GameGrid grid, Tower tower)
        {
            Grid = grid;
            Tower = tower;
        }
    }
}
