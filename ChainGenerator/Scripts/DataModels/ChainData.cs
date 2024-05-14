using UnityEngine;



namespace Chain
{
    [CreateAssetMenu(fileName = nameof(ChainData), menuName = "Chain Generator/" + nameof(ChainData))]
    public class ChainData : ScriptableObject, IMachinePartData
    {
        public float LinkInterval = 1.8f;
        public Vector3 LinkSize = new Vector3(.8f, .8f, .8f);
        public float RadiusOffset = 0.45f;
        public float Tension = 0.8f;
        public LinksPool LinksPoolPrefab;

        public float SpeedMultiplier = 1f;
        public float LinkRotationMultiplier = 1f;
        public ChainEnums.ChainDirection motionDirection = ChainEnums.ChainDirection.Clockwise;
        
        public bool SetRadiusByGear = true;
        public bool SetMotionByGear = true;
        public bool IsMoving = true;
        public float LinkLagAmount = 0.001f;
        public bool LinkRotationEffect;


        public bool OnTesting;
    }

    public interface IMachinePartData
    {}
   
}

