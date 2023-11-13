using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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