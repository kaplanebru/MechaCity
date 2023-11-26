using UnityEditor;

namespace Chain
{
    public class LinkPoolCreator : PoolCreator<LinksPool, ChainLink>
    {
        [MenuItem("Tools/Pool Creator/Link Pool Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LinkPoolCreator));
        }
    }
}