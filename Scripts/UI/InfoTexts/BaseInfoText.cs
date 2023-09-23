using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using TMPro;
using UnityEngine;

public abstract class BaseInfoText : MonoBehaviour
{
    protected Team[] _teams;
    protected TextMeshProUGUI infoText;
    
    private void OnEnable()
    {
        infoText = GetComponentInChildren<TextMeshProUGUI>();
        SubscribeEvents();
    }
    
    protected virtual void SubscribeEvents(){}
    
    public void Setup(Team[] teams)
    {
        _teams = teams;
        SetInfoText(_teams[0].Data.Name);
    }

    public void UpdateInfoText(TeamType currentTeamType)
    {
        SetInfoText(_teams.FirstOrDefault(t=>t.Data.TeamType == currentTeamType).Data.Name);
    }
    protected abstract void SetInfoText(string teamName);

    protected virtual void Unsubscribe(){}
    private void OnDisable()
    {
        Unsubscribe();
    }
}
