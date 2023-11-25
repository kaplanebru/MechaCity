using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Chain;
using PlasticPipe.PlasticProtocol.Client;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Machinery))]
public class ChainPrefabEditor : Editor
{
    [SerializeField] private Cogwheel[] cogs;
    private string[] cogHolderLabels;
    private int selectedIndex = 0;

    [SerializeField] private bool newChainData;
    private string chainDataName;
    
    [SerializeField] private CogData cogData;
    public Machinery machineryPrefab;
    private GUIStyle _narrowButton;

    [SerializeField] int cogToDestroyIndex;
    
    public override void OnInspectorGUI()
    {
        if (EditorApplication.isPlaying) return;
        DrawDefaultInspector();

        if (machineryPrefab == null)
            machineryPrefab = target as Machinery;


        GUILayout.Label("Chain & Cog Generator", EditorStyles.boldLabel);
        _narrowButton = new GUIStyle(GUI.skin.button);
        _narrowButton.fixedWidth = 200f;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("SAVE CHANGES"))
            machineryPrefab.SaveMachinery();

        if (GUILayout.Button("SAVE ONTO EXISTING PREFAB"))
            machineryPrefab.SaveOnExistingPrefab();


        EditorGUILayout.EndHorizontal();

        if (cogs == null || (cogs.Length > 0 && cogs[0] == null))
            cogs = machineryPrefab.cogHolder.RestoreCogsInEditor();

        if (cogHolderLabels == null || cogHolderLabels.Length != cogs.Length)
        {
            cogHolderLabels = new string[cogs.Length];
            cogHolderLabels = cogs.Select(x => x.ToString()).ToArray();
        }

        if (GUILayout.Button("Reset To 2D Space"))
            machineryPrefab.To2D();

        EditorGUILayout.Space();

        GUILayout.Label("COG SETTINGS", EditorStyles.boldLabel); //\n 
        //MyEditorHelpers.DrawSeparatorLine(Color.gray);

        ShowGizmosOnSelection();
        EditorGUILayout.Space();

        Color originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.yellow;
        selectedIndex = EditorGUILayout.Popup("Selected Cog :", selectedIndex, cogHolderLabels);
        GUI.backgroundColor = originalBackgroundColor;

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Space();


        if (selectedIndex >= 0 && selectedIndex < cogs.Length)
        {
            EditorGUI.indentLevel++;
            FillCogData(selectedIndex);
            machineryPrefab.cogHolder.DrawGizmosOnSelectedCog(selectedIndex);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
        MyEditorHelpers.DrawFrames(Color.gray, GUILayoutUtility.GetLastRect());

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Cogs"))
            SaveCogs();

        EditorGUILayout.Space();


        GUILayout.Label(" _______________ADD or REMOVE COG_______________ ", EditorStyles.centeredGreyMiniLabel); //\n 
        EditorGUILayout.Space();
        machineryPrefab.cogHolder.cogPrefab = (Cogwheel) EditorGUILayout.ObjectField("Cog Prefab",
            machineryPrefab.cogHolder.cogPrefab, typeof(Cogwheel), false);
        
        GUILayout.BeginHorizontal();
        cogDataName = EditorGUILayout.TextField( "Add Cog Creating New Data",cogDataName);
        if (GUILayout.Button("Apply"))
            AddCog(true, cogDataName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        cogData = (CogData) EditorGUILayout.ObjectField("Add Cog With Selected Data", cogData, typeof(CogData), false);
        if (GUILayout.Button("Apply"))
            AddCog(false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        cogToDestroyIndex = EditorGUILayout.Popup("Remove Selected Cog", cogToDestroyIndex, cogHolderLabels);
        if (GUILayout.Button("Remove"))
            RemoveCog();
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();

        ////////////////CHAIN RELATED///////////////////////////////
        machineryPrefab.machinerySpeed = EditorGUILayout.FloatField("Machinery Speed", machineryPrefab.machinerySpeed);

        machineryPrefab.isChainRelated = EditorGUILayout.Toggle("Chain Related", machineryPrefab.isChainRelated);

        if (machineryPrefab.isChainRelated)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("CHAIN PROPERTIES", EditorStyles.boldLabel);
            MyEditorHelpers.DrawSeparatorLine(Color.gray);
            EditorGUILayout.Space();

            
            EditorGUILayout.LabelField("Set Chain Data: ", EditorStyles.boldLabel);

            machineryPrefab.chainGenerator.ChainData = (ChainData) EditorGUILayout.ObjectField(
                "Use Selected Chain Data", machineryPrefab.chainGenerator.ChainData, typeof(ChainData),
                false);
            EditorGUILayout.BeginHorizontal();
            newChainData = EditorGUILayout.Toggle("Create New Chain Data", newChainData);
            EditorGUILayout.LabelField("(Optional)");
            EditorGUILayout.EndHorizontal();

            if (newChainData)
            {
                chainDataName = EditorGUILayout.TextField("Chain Data Name", chainDataName);

                if (GUILayout.Button("Apply"))
                {
                    machineryPrefab.chainGenerator.CreateChainData(chainDataName);
                    newChainData = false;
                }
            }
            
            EditorGUILayout.Space();

            if (machineryPrefab.chainGenerator == null)
            {
                Debug.LogWarning("Add Chain Generator Script");
                return;
            }

          

            if (machineryPrefab.chainGenerator.ChainData != null)
            {
                FillChainData();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Generate Chain"))
                    GenerateChain();

                // if (GUILayout.Button("Delete Link Pool"))
                //     DeleteLinkPool();

                if (GUILayout.Button("Reset Links"))
                    machineryPrefab.chainGenerator.ResetLinks();
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space();
        EditorGUI.EndChangeCheck();
    }

    void ShowGizmosOnSelection()
    {
        EditorGUI.BeginChangeCheck();
        machineryPrefab.cogHolder.showGizmos =
            EditorGUILayout.Toggle("Show Gizmos On Selected Cog", machineryPrefab.cogHolder.showGizmos);
        if (EditorGUI.EndChangeCheck())
        {
            if (!machineryPrefab.cogHolder.showGizmos)
                machineryPrefab.cogHolder.DisableAllGizmos();
            else
                machineryPrefab.cogHolder.DrawGizmosOnSelectedCog(selectedIndex);
            Repaint();
        }
    }


    void AddCog(bool isNew, string cogName = null)
    {
        machineryPrefab.cogHolder.AddCog(isNew, cogData, cogName);
        cogs = machineryPrefab.cogHolder.RestoreCogsInEditor();
        selectedIndex = cogs.Length - 1;
        cogToDestroyIndex = selectedIndex;
        GenerateChain();
        Repaint();
    }

    void RemoveCog()
    {
        machineryPrefab.cogHolder.RemoveCog(cogToDestroyIndex);
        cogs = machineryPrefab.cogHolder.RestoreCogsInEditor();

        if (cogs.Length > 0)
        {
            if (selectedIndex == cogToDestroyIndex)
            {
                int newIndex;
                do
                {
                    newIndex = UnityEngine.Random.Range(0, cogs.Length);
                } while (newIndex == selectedIndex);

                selectedIndex = newIndex;
            }

            cogToDestroyIndex = cogs.Length - 1;
        }

        GenerateChain();
        Repaint();
    }


    void GenerateChain()
    {
        machineryPrefab.chainGenerator.GenerateChain(SaveCogs, machineryPrefab.cogHolder.GetChainRelatedCogs());
    }

    void SaveCogs()
    {
        foreach (var cog in cogs)
        {
            EditorUtility.SetDirty(cog.Data);
        }
    }

    private bool changeCogData = false;
    private bool changeWithNewCogData = false;
    private CogData otherCogData;

    void ChangeCogData(int i)
    {
        GUILayout.BeginHorizontal();
        changeCogData = EditorGUILayout.Toggle("Change With Other Data", changeCogData);
        changeWithNewCogData = EditorGUILayout.Toggle("Change With New Data", changeWithNewCogData);
        GUILayout.EndHorizontal();

        if (changeCogData)
        {
            otherCogData = (CogData) EditorGUILayout.ObjectField("Cog Data", otherCogData, typeof(CogData), false);
            if (GUILayout.Button("Apply"))
            {
                if (otherCogData == null) return;

                cogData = otherCogData;
                machineryPrefab.cogHolder.cogs[i].Data = otherCogData;
                GenerateChain();
                changeCogData = false;
            }
        }

        if (changeWithNewCogData)
        {
            if (GUILayout.Button("Apply"))
            {
                cogDataName = EditorGUILayout.TextField("Cog Data Name", cogDataName);
                cogs[i].Data = machineryPrefab.cogHolder.CreateCogData("Cog " + cogDataName); //machineryPrefab.cogHolder.newCogIndex++
                changeWithNewCogData = false;
                GenerateChain();
                Repaint();
                machineryPrefab.SaveMachinery();
            }
        }
    }
    
    string cogDataName;
    

    void FillCogData(int i)
    {
        CogData Data = cogs[i].Data;

        if (Data == null)
        {
            Debug.Log(cogs.Length);
            Debug.Log(cogs[i].name);
            return;
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data Name");
        EditorGUILayout.LabelField(Data.name, EditorStyles.helpBox);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Shape Settings", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
        if (EditorGUI.EndChangeCheck())
        {
            SetupCogsWithSameData(i);
            GenerateChain();
        }

        EditorGUI.BeginChangeCheck();

        Data.Volume = EditorGUILayout.FloatField("Cog Volume", Data.Volume);
        if (Data.ContactType == ChainEnums.CogContactType.CogRelated)
        {
            Data.RelatedCog =
                (Cogwheel) EditorGUILayout.ObjectField("Related Cog", Data.RelatedCog, typeof(Cogwheel), true);
        }

        EditorGUILayout.Space();

        Data.HoleType = (ChainEnums.HoleType) EditorGUILayout.EnumPopup("Hole Type", Data.HoleType);
        Data.HoleSize = EditorGUILayout.FloatField("Hole Size", Data.HoleSize);
        Data.HoleDepth = EditorGUILayout.FloatField("Hole Depth", Data.HoleDepth);

        if (!machineryPrefab.isChainRelated || Data.ContactType == ChainEnums.CogContactType.Indifferent)
        {
            Data.IsMoving = EditorGUILayout.Toggle("Is Moving", Data.IsMoving);
            Data.RotationDirection = EditorGUILayout.IntField("Rotation Direction", Data.RotationDirection);
        }

        EditorGUILayout.Space();

        Data.ContactType = (ChainEnums.CogContactType) EditorGUILayout.EnumPopup("Contact Type", Data.ContactType);

        if (EditorGUI.EndChangeCheck())
            AccidentalSetupCogsWithSameData(i);


        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Teeth Settings", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        Data.TeethPoolPrefab = (TeethPool) EditorGUILayout.ObjectField("Teeth Pool Prefab",
            Data.TeethPoolPrefab, typeof(TeethPool), false);
        if (GUILayout.Button("Apply")) //, narrowButton))
            HandleTeethPoolChange(i);
        GUILayout.EndHorizontal();

        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
        Data.ToothGap = EditorGUILayout.FloatField("Tooth Gap", Data.ToothGap);
        Data.Equalize = EditorGUILayout.Toggle("Equal Gaps", Data.Equalize);
        Data.MinGapLimit = EditorGUILayout.FloatField("Min Gap Limit", Data.MinGapLimit);

        EditorGUILayout.Space();
        ChangeCogData(i);

        if (EditorGUI.EndChangeCheck())
            SetupCogsWithSameData(i);
        //EditorUtility.SetDirty(cogs[i].Data); //TODO: removed
    }

    void SetupCogsWithSameData(int i)
    {
        Cogwheel selectedCog = cogs[i];
        foreach (var cog in cogs)
        {
            if (cog.Data == selectedCog.Data)
                cog.Setup();
        }
    }

    void AccidentalSetupCogsWithSameData(int i)
    {
        Cogwheel selectedCog = cogs[i];
        foreach (var cog in cogs)
        {
            if (cog.Data == selectedCog.Data)
                cog.AccidentalSetup();
        }
    }

    //private ChainLink lastLinkPrefab;

    void FillChainData()
    {
        ChainData chainData = machineryPrefab.chainGenerator.ChainData;
        chainData.OnTesting = EditorGUILayout.Toggle("On Testing", chainData.OnTesting);

        EditorGUI.BeginChangeCheck();
        //chainData.Type = (ChainEnums.ChainType) EditorGUILayout.EnumPopup("Type", chainData.Type);
        chainData.RadiusOffset = EditorGUILayout.FloatField("Distance from Cogs", chainData.RadiusOffset);
        chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Chain Link Settings", EditorStyles.boldLabel);
        chainData.LinkInterval = EditorGUILayout.FloatField("Link Interval", chainData.LinkInterval);
        chainData.LinkSize = EditorGUILayout.Vector3Field("Link Size", chainData.LinkSize);
        chainData.LinkRotationEffect = EditorGUILayout.Toggle("Rotate Links", chainData.LinkRotationEffect);

        if (EditorGUI.EndChangeCheck())
            GenerateChain();

        GUILayout.BeginHorizontal();
        chainData.LinksPoolPrefab = (LinksPool) EditorGUILayout.ObjectField("Links Pool Prefab",
            chainData.LinksPoolPrefab, typeof(LinksPool), false);

        if (GUILayout.Button("Apply")) //, narrowButton))
            HandleLinksPoolChange();

        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //chainData.SetRadiusByGear = EditorGUILayout.Toggle("Set Radius By Cog", chainData.SetRadiusByGear);
        chainData.IsMoving = EditorGUILayout.Toggle("Is Moving", chainData.IsMoving);

        if (chainData.IsMoving)
        {
            //chainData.MachinerySpeed = EditorGUILayout.FloatField("Machinery Speed", chainData.MachinerySpeed);
            chainData.SpeedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", chainData.SpeedMultiplier);
            chainData.LinkRotationMultiplier =
                EditorGUILayout.FloatField("Link Rotation Multiplier", chainData.LinkRotationMultiplier);

            chainData.motionDirection =
                (ChainEnums.ChainDirection) EditorGUILayout.EnumPopup("Motion Direction", chainData.motionDirection);
            chainData.SetMotionByGear = EditorGUILayout.Toggle("Set Motion By Cog", chainData.SetMotionByGear);
        }
    }

    void HandleLinksPoolChange()
    {
        DeleteLinkPool();
        GenerateChain();
    }

    void HandleTeethPoolChange(int i)
    {
        DeleteTeethPool(i);
        SaveCogs();
    }

    void DeleteLinkPool()
    {
        if (MyPrefabHelpers.IsPrefabInstance(machineryPrefab.gameObject))
        {
            Debug.LogWarning("Change pool from prefab view");
            return;
        }

        machineryPrefab.chainGenerator.DeletePoolClearLinks();
        machineryPrefab.SaveMachinery();
        machineryPrefab.chainGenerator.CreatePool();
    }

    void DeleteTeethPool(int i)
    {
        if (MyPrefabHelpers.IsPrefabInstance(machineryPrefab.gameObject))
        {
            Debug.LogWarning("Change pool from prefab view");
            return;
        }

        ChainEvents.OnDeleteTeethPool?.Invoke(cogs[i].CogId);
        machineryPrefab.SaveMachinery();
        ChainEvents.OnCreateTeethPool?.Invoke(cogs[i].CogId);
    }
    
}