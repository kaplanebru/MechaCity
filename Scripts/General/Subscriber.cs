using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subscriber<T> where T : class
{
    protected T MainClass;
    public Subscriber(T mainClass)
    {
        MainClass = mainClass;
    }
    public abstract void Subscribe();
    public abstract void Unsubscribe();
}
