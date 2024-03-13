using Teams;
using UnityEngine;

public interface ISelectionBlocker< out TTeam> where TTeam : TeamData
{
    public TTeam TeamToBlock { get;  }

    public void EliminateNonSelectables();
    
    public void EliminateSpecificNonSelectables<TTTeam>(TTTeam teamToBlock) where TTTeam : TeamData;

}

public class Tes
{
    public void Print<T>(T input)
    {
        Debug.Log(input);
    }
}
