using Chain;
using UnityEditor;

namespace ChainEditor
{
#if UNITY_EDITOR
    public class TeethPoolCreator : PoolCreator<TeethPool, Tooth>
    {
        [MenuItem("Tools/Chain Generator/Pool Creator/Teeth Pool Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TeethPoolCreator));
        }
    }

}
#endif