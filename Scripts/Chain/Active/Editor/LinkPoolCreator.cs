using UnityEditor;

namespace Chain
{
    public class LinkPoolCreator : PoolCreator<LinksPool, ChainLink>
    {
        [MenuItem("Tools/Chain Generator/Pool Creator/Link Pool Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LinkPoolCreator));
        }
    }
}