using System;
using UnityEngine;

public class ChainEvents
{
    // public static Action<List<Vector3>> OnPointsCreated;
    //public static Action<List<ChainLink>, List<Vector3>> OnLinksCreated;
    // public static Action<CogHolder> OnCogSetupRequest; 
     // public static Action OnLinksReady; += save machinery
    
   // public static Action<bool> OnMotionStateSet; //todo: startta yapılır: application is playingse
    public static Action<float, int> OnCogSpeedSet;  //Action<int, float, int> 

   
    //public static Action<Cogwheel[], ChainPointCreator> OnChainRequest; //gerek yok chain spawnera ulaşıyoruz zaten
    
    //bunlar anlık tetiklenmediği için eventle yapılabilir
    public static Action<int> OnCreateTeethPool;
    //public static Action OnDeleteLinks;
    public static Action <int> OnDeleteTeethPool;
    public static Action<Transform> OnDeleteObject;

    

}