using UnityEngine;

public class SelectionReferences : MonoBehaviour //TODO: TEMP
{
    public static SelectionReferences Instance;
    public SelectionAssetHolder assetHolder;

    private void Awake()
    {
        Instance = this;
        assetHolder.Setup();
    }
}