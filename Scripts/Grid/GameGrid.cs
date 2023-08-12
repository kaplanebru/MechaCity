using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class GameGrid
{
    public const int ColumnAmount = 3;
    public const int LineAmount = 1;
    
    [ReadOnly]public Column[] Columns = new Column[ColumnAmount];
}
