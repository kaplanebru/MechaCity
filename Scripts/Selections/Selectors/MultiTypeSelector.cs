using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Enums.Selections;
using Teams;
using UnityEngine;

public class MultiTypeSelector : Selector, IBlockable
{
    
    private bool isFull = false;

    protected override void Register()
    {
        TeamEvents.OnTeamsSent += GetTeamsData;
    }

    protected override void Unregister()
    {
        TeamEvents.OnTeamsSent -= GetTeamsData;
    }

    public override void StartWithNewTowers()
    {
        DeselectAll();
        CurrentGroup = Data.Groups[0];

        if (CurrentGroup.BlockType != BlockType.None) //bunu sileriz
            TeamEvents.OnTeamsRequest?.Invoke();
    }

    private void GetTeamsData(Dictionary<TeamState, Team> teams) //sürekli değiştiği için, burda almakta fayda var
    {
        SetTeams(teams);
    }

    protected override void GetTower(params object[] args)
    {
        int towerId = (int) args[0];
        
        Debug.Log("multi");

        if (isFull)
        {
            DeselectAll();
            isFull = false;
            ShowCompleteButton(false);
        }

        // if(SelectedTwice(towerId)) return; //sadece seçili olan deselect olur. TODO: tıklanamaz olduğu için sıkıntı

        HandleSelection(true, towerId);

        if (CurrentGroup.SelectedTowers.Count == CurrentGroup.MaxTowers)
        {
            ShiftGroup();
        }
    }

    protected override void HandleUI()
    {
    }

    void ShiftGroup()
    {
        int nextGroupIndex = CurrentGroup.Index + 1; // % groupAmount - 1;

        if (nextGroupIndex == Data.Groups.Length)
        {
            FullSituation();
            nextGroupIndex = 0;
        }

        CurrentGroup = Data.Groups[nextGroupIndex];
        Block();
    }

    public void TryBlock(Dictionary<TeamState, Team> teamsByTurn)
    {
        _teamsByTurn = teamsByTurn;
        Block();
    }

    void Block()
    {
        //Blocker.BlockedTeamState = CurrentGroup.BlockedTeamState;
        Blocker.BlockType = CurrentGroup.BlockType;
        Blocker.BlockSelection(_teamsByTurn, CurrentGroup.BlockedTeam);
       
    }

    // void Block()
    // {
    //     AllTowers.EnableClickability();
    //
    //     switch (CurrentGroup.BlockState)
    //     {
    //         case Selections.BlockState.BlockCurrent:
    //             playerBlocker.BlockSelection(_teams);
    //             break;
    //
    //         case Selections.BlockState.BlockRival:
    //             rivalBlocker.BlockSelection(_teams);
    //             break;
    //     }
    // }

    void FullSituation()
    {
        ShowCompleteButton(true);
        isFull = true;
    }

    protected override void DeselectAll()
    {
        foreach (var group in Data.Groups)
        {
            group.ResetTowers();
        }
    }




    protected override bool SelectedTwice(int selectedTower)
    {
        return false;
    }

    public override List<int> SendAllTowers()
    {
        List<int> towers = new();
        foreach (var group in Data.Groups)
        {
            towers.AddRange(group.SelectedTowers);
        }

        return towers;
    }

   
}