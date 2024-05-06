using Chain;
using UnityEditor;

namespace ChainEditor
{
#if UNITY_EDITOR
    public class LinkPoolCreator : PoolCreator<LinksPool, ChainLink>
    {
        [MenuItem("Tools/Chain Generator/Pool Creator/Link Pool Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LinkPoolCreator));
        }
    }
}
#endif