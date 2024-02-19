
using System;
using Enums;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    

    // private void Start()
    // {
    //      BpExample bpExample = new BpExample(BpType.Reverse,__data, new ReverseAction()); 
    // }

   
}






public interface IBpAction
{
    public void Execute();
   
}

public class ReverseAction : IBpAction
{
    public void Execute()
    {
        
    }
}


//bp türleri: on combat, before combat