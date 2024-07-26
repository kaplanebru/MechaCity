using System.Collections;
using System.Collections.Generic;
using TowerExternal;
using UnityEngine;

[CreateAssetMenu(menuName = "ExternalElements/" + nameof(FloorData), fileName = nameof(FloorData))]
public class FloorData : ScriptableObject
{
    public float Duration = 0.5f;
    public float OpenSize = 0.4f;
    public float CloseDelay = 0.5f; //TODO Floor Data SO
}
