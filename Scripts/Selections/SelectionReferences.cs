using System.Collections.Generic;
using Enums.Selections;
using UnityEngine;

public class SelectionReferences : MonoBehaviour
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
        _selectors[SelectionType.PlayerOnlyBp].SetData(GetData(SelectionType.PlayerOnlyBp));
        _selectors[SelectionType.SinglePlayerOnlyBP].SetData(GetData(SelectionType.SinglePlayerOnlyBP));
        _selectors[SelectionType.SingleRivalOnlyBP].SetData(GetData(SelectionType.SingleRivalOnlyBP));
        _selectors[SelectionType.All].SetData(GetData(SelectionType.All));
    }

    void CreateSelectors()
    {
        _selectors.Add(SelectionType.PlayerOnlyStd, new SingleTypeSelector());
        _selectors.Add(SelectionType.PlayerOnlyBp, new SingleTypeSelector());
        _selectors.Add(SelectionType.SinglePlayerOnlyBP, new SingleTypeSelector());
        _selectors.Add(SelectionType.SingleRivalOnlyBP, new SingleTypeSelector());
        _selectors.Add(SelectionType.All, new MultiTypeSelector());
    }


    
   
}