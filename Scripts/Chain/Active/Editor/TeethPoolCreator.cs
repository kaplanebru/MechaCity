using GenericHelper;
using UnityEditor;

namespace Chain
{
    public class TeethPoolCreator : PoolCreator<TeethPool, Tooth>
    {
        [MenuItem("Tools/Pool Creator/Teeth Pool Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(TeethPoolCreator));
        }
    }

}
