using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITowerSegment
{
    public int Id { get; set; }
    public void SetId(int id);

    public void Initialize();
}

public interface ITowerRelatedElement
{
    public int Id { get; set; }
    public void Initialize(int id);
}