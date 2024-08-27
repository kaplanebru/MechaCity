using Enums;
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

    public SelectionData GetData(Selections.SelectionType type) => assetHolder.DataByType[type];

   
}