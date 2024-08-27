using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

public class SelectionReferences : MonoBehaviour //TODO: TEMP
{
    public static SelectionReferences Instance;
    public SelectionDataHolder dataHolder;
    public Dictionary<SelectionType, Selector> Selectors = new();
    
    
    public SelectionData GetData(SelectionType type) => dataHolder.DataByType[type];
    private void Awake()
    {
        Instance = this;
        dataHolder.Setup();
        
        CreateSelectors();
        SetSelectors();
    }

    void SetSelectors()
    {
        Selectors[SelectionType.PlayerOnlyStd].SetData(GetData(SelectionType.PlayerOnlyStd));
        Selectors[SelectionType.PlayerOnlyStd].SetData(GetData(SelectionType.PlayerOnlyBp));
        Selectors[SelectionType.All].SetData(GetData(SelectionType.All));
    }

    void CreateSelectors()
    {
        Selectors.Add(SelectionType.PlayerOnlyStd, new SingleTypeSelector());
        Selectors.Add(SelectionType.PlayerOnlyBp, new SingleTypeSelector());
        Selectors.Add(SelectionType.All, new MultiTypeSelector());
    }


    
   
}