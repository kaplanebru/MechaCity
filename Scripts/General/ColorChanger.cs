using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using UnityEngine;


public interface ISelectionColors
{
     public Color SelectionColor {get; set; }
}

public interface ITeamColorSetter //teamdatada mı eklenecek
{
    public Color[] TeamColors {get; set; }
}
public class ColorChanger
{
    private CombatTimingData _timingData;
    public ColorChanger(CombatTimingData timingData)
    {
        _timingData = timingData;
    }
    
    public void FadeColors(MeshRenderer[] meshes, ITeamColorSetter teamColorSetter)
    {
        FadeColor(meshes[0], teamColorSetter.TeamColors[0]);
        if(meshes.Length < 2) return;
        
        for (var i = 1; i < meshes.Length; i++)
        {
            FadeColor(meshes[i], teamColorSetter.TeamColors[1]);
        }
    }
    private void FadeColor(MeshRenderer mesh, Color newColor)
    {
        mesh.material.DOColor(newColor, _timingData.colorFadeDuration);
    }

    public void ChangeSpriteColor(SpriteRenderer sprite)
    {
        sprite.color = Color.cyan;
    }
    
    public void SetMats(MeshRenderer[] meshes, Material[] mats)
    {
        meshes[0].material = mats[0];
        for (var i = 1; i < meshes.Length; i++)
        {
            meshes[i].material = mats[1];
        }
    }
}
