using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

public class SelectionReferences : MonoBehaviour //TODO: TEMP
{
    public static SelectionReferences Instance;
    public SelectionDataHolder dataHolder;
    private Dictionary<SelectionType, Selector> _selectors = new();
    
    
    public SelectionData GetData(SelectionType type) => dataHolder.DataByType[type];
    public Selector GetSelector(SelectionType type) => _selectors[type];
    private void Awake()
    {
        Instance = this;
        dataHolder.Setup();
        
        CreateSelectors();
        SetSelectors();
    }

    void SetSelectors()
    {
        _selectors[SelectionType.PlayerOnlyStd].SetData(GetData(SelectionType.PlayerOnlyStd));
        _selectors[SelectionType.PlayerOnlyStd].SetData(GetData(SelectionType.PlayerOnlyBp));
        _selectors[SelectionType.All].SetData(GetData(SelectionType.All));
    }

    void CreateSelectors()
    {
        _selectors.Add(SelectionType.PlayerOnlyStd, new SingleTypeSelector());
        _selectors.Add(SelectionType.PlayerOnlyBp, new SingleTypeSelector());
        _selectors.Add(SelectionType.All, new MultiTypeSelector());
    }


    
   
}