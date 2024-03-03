using Teams;
using UnityEngine;

public interface ISelectionBlocker<out TTeam, out TTTeam> where TTeam : TeamData where TTTeam : TeamData
{
    public TTeam SelectingTeam { get; }
    
    public TTTeam RivalTeam { get;  }
    public void EliminateNonSelectables();

    public void SetMaterials();

}

public class Test
{
    public void Print<T>(T input)
    {
        Debug.Log(input);
    }
}
