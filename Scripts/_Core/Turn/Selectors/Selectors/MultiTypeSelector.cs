using System.Collections;
using System.Collections.Generic;
using _Core.Turn.Selectors;
using Enums;
using Network;
using Teams;
using Towers;
using Turn;
using UnityEngine;

public class MultiTypeSelector : Selector<BpSelectionColor>
{
    private PlayerBlocker playerBlocker = new PlayerBlocker();
    private RivalBlocker rivalBlocker = new RivalBlocker();

    public SelectionData Data;
    private bool isFull = false;

    protected override void Register()
    {
        TurnHelper.TurnEvents.OnTeamsSent += GetTeamsData;
    }

    protected override void Unregister()
    {
        TurnHelper.TurnEvents.OnTeamsSent -= GetTeamsData;
    }

    public override void StartWithNewTowers()
    {
        CurrentGroup = Data.Groups[0];

        if (CurrentGroup.BlockState != Selections.BlockState.None) //bunu sileriz
            TurnHelper.TurnEvents.OnTeamsRequest?.Invoke();
    }

    private void GetTeamsData(Dictionary<TeamState, Team> teams) //sürekli değiştiği için, burda almakta fayda var
    {
        _teams = teams;
    }

    protected override void GetTower(params object[] args)
    {
        int towerId = (int) args[0];

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

    void Block()
    {
        AllTowers.EnableClickability();

        switch (CurrentGroup.BlockState)
        {
            case Selections.BlockState.BlockCurrent:
                playerBlocker.BlockSelection(_teams);
                break;

            case Selections.BlockState.BlockRival:
                rivalBlocker.BlockSelection(_teams);
                break;
        }
    }

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