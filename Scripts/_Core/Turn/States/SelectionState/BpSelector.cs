using System.Collections;
using System.Collections.Generic;
using Teams;
using UnityEngine;

public class BpSelector : BaseSelector
{
    public Material bpSelectionMat;
    public TeamData SelectingTeam { get; }
    public BpSelector(TeamData selectingTeam)
    {
        SelectingTeam = selectingTeam;
        SetMaterials();
    }
    
    public sealed override void SetMaterials()
    {
        defaultMat = SelectingTeam.TeamTowerData.DefaultMaterial;
        selectionMat = SelectingTeam.TeamTowerData.BlueprintMaterial;
    }
}
