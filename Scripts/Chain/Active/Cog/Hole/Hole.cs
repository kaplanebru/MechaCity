using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace Chain
{
    public class Hole : MonoBehaviour
    {
        // public ChainEnums.HoleType holeType;
        public int id;
        
        public int Id
        {
            get => id;
            set => id = value;
        }
    }

}
