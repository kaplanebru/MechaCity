using System.Collections.Generic;
using Enums;
using UnityEngine;

public class SelectionReferences : MonoBehaviour //TODO: TEMP
{
    public static SelectionReferences Instance;
    public SelectionDataHolder dataHolder;
    public Dictionary<Selections.SelectionType, Selector> Selectors = new();
    
    
    public SelectionData GetData(Selections.SelectionType type) => dataHolder.DataByType[type];
    private void Awake()
    {
        Instance = this;
        dataHolder.Setup();
        
        CreateSelectors();
        SetSelectors();
    }

    void SetSelectors()
    {
        Selectors[Selections.SelectionType.PlayerOnlyStd].SetData(GetData(Selections.SelectionType.PlayerOnlyStd));
        Selectors[Selections.SelectionType.PlayerOnlyStd].SetData(GetData(Selections.SelectionType.PlayerOnlyBp));
        Selectors[Selections.SelectionType.All].SetData(GetData(Selections.SelectionType.All));
    }

    void CreateSelectors()
    {
        Selectors.Add(Selections.SelectionType.PlayerOnlyStd, new SingleTypeSelector());
        Selectors.Add(Selections.SelectionType.PlayerOnlyBp, new SingleTypeSelector());
        Selectors.Add(Selections.SelectionType.All, new MultiTypeSelector());
    }


    
   
}