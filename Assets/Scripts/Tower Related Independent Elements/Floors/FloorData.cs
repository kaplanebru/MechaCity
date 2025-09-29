using System.Collections;
using System.Collections.Generic;
using TowerRelated;
using UnityEngine;

[CreateAssetMenu(menuName = "ExternalElements/" + nameof(FloorData), fileName = nameof(FloorData))]
public class FloorData : ScriptableObject
{
    public float OpenPosY = -3f;
    public float Duration = 0.5f;
}
