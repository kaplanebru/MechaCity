using System.Collections;
using System.Collections.Generic;
using Core;
using Teams;
using UnityEngine;

public class BpSelector : BaseSelector
{
    public Material BpSelectionMat;
  
    public BpSelector() //(Material bpSelectionMat)
    {
        //BpSelectionMat = bpSelectionMat;
        SetMaterials();
    }
    
    public sealed override void SetMaterials()
    {
        selectionMat = Initializer.Teams[0].Data.TeamTowerData.BlueprintMaterial; //temp
    }
}
