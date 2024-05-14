using System.Linq;
using Chain;
using UnityEngine;
using UnityEditor;

namespace ChainEditor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Machinery))]
    public class ChainPrefabEditor : Editor
    {
        [SerializeField] private Cogwheel[] cogs;
        private string[] cogHolderLabels;
        private int selectedIndex = 0;

        [SerializeField] private bool newChainData;
        private string chainDataName;

        [SerializeField] private GearData gearData;
        public Machinery machinery;
        private GUIStyle _narrowButton;
        string cogDataName = "Gear Data Name";
        private bool changeCogData = false;
        private bool changeWithNewCogData = false;
        private GearData _otherGearData;

        int gearIndex;

        [SerializeField] int cogToDestroyIndex;

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying) return;
            DrawDefaultInspector();

            if (machinery == null)
                machinery = target as Machinery;
            if (machinery.CompareTag("Model")) return;


            GUILayout.Label("Chain & Gear Generator", EditorStyles.boldLabel);
            _narrowButton = new GUIStyle(GUI.skin.button);
            _narrowButton.fixedWidth = 200f;

            EditorGUI.BeginChangeCheck();
            Save();

            if (cogs == null || (cogs.Length > 0 && cogs[0] == null))
                cogs = machinery.cogHolder.RestoreCogsInEditor();

            if (cogHolderLabels == null || cogHolderLabels.Length != cogs.Length)
            {
                cogHolderLabels = new string[cogs.Length];
                cogHolderLabels = cogs.Select(x => x.ToString()).ToArray();
            }

            if (GUILayout.Button("Reset To 2D Space"))
                machinery.To2D();

            EditorGUILayout.Space();

          


            GUILayout.Label(" _______________ADD or REMOVE GEAR_______________ ",
                EditorStyles.centeredGreyMiniLabel); //\n 
            EditorGUILayout.Space();
            machinery.cogHolder.cogPrefab = (Cogwheel) EditorGUILayout.ObjectField("Gear Prefab",
                machinery.cogHolder.cogPrefab, typeof(Cogwheel), false);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add Gear Options:");
            GUILayout.BeginHorizontal();

            cogDataName = EditorGUILayout.TextField("1- Add Gear Creating New Data", cogDataName);
            if (GUILayout.Button("Apply"))
                AddCog(true, cogDataName);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            gearData = (GearData) EditorGUILayout.ObjectField("2- Add Gear With Selected Data", gearData,
                typeof(GearData), false);
            if (GUILayout.Button("Apply"))
                AddCog(false);

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();

            cogToDestroyIndex = EditorGUILayout.Popup("Remove Selected Gear", cogToDestroyIndex, cogHolderLabels);
            if (GUILayout.Button("Remove"))
                RemoveCog();

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            
            
            GUILayout.Label("GEAR SETTINGS", EditorStyles.boldLabel); //\n 

            if (cogs.Length > 0)
            {
                ShowGizmosOnSelection();
                EditorGUILayout.Space();

                Color originalBackgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.yellow;
                selectedIndex = EditorGUILayout.Popup("Selected Gear :", selectedIndex, cogHolderLabels);
                GUI.backgroundColor = originalBackgroundColor;

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.Space();


                if (selectedIndex >= 0 && selectedIndex < cogs.Length)
                {
                    EditorGUI.indentLevel++;
                    FillCogData(selectedIndex);
                    machinery.cogHolder.DrawGizmosOnSelectedCog(selectedIndex);
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
                EditorStyleHelper.DrawFrames(Color.gray, GUILayoutUtility.GetLastRect());

                EditorGUILayout.Space();
                if (GUILayout.Button("Save Gears"))
                    SaveCogs();

                EditorGUILayout.Space();
            }
            

            ////////////////CHAIN RELATED///////////////////////////////
            EditorStyleHelper.DrawSeparatorLine(Color.gray);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("MACHINERY PROPERTIES", EditorStyles.boldLabel);


            machinery.machinerySpeed = EditorGUILayout.FloatField("Machinery Speed", machinery.machinerySpeed);

            EditorGUILayout.BeginHorizontal();
            machinery.sortingOrder = EditorGUILayout.IntSlider("Sorting Order", machinery.sortingOrder, -20, 0);
            if (GUILayout.Button("Apply"))
            {
                machinery.cogHolder.SetCogsSortingOrder(machinery.sortingOrder);
                machinery.SaveMachinery();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            //gearIndex = EditorGUILayout.IntField("Set pivot as Selected Gear (Optional)", gearIndex);
            gearIndex = EditorGUILayout.IntSlider("Set pivot as Selected Gear (Optional)", gearIndex, 0,
                cogs.Length - 1);

            if (GUILayout.Button("Apply"))
            {
                machinery.SetPivotToSelectedGear(gearIndex);
                GenerateChain();
            }

            EditorGUILayout.EndHorizontal();

            machinery.movingAtStart =  EditorGUILayout.Toggle("Moving At Start", machinery.movingAtStart);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("*******CHAIN RELATION*******", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();
           
            machinery.isChainRelated = EditorGUILayout.Toggle("Chain Related", machinery.isChainRelated);
            if (EditorGUI.EndChangeCheck())
            {
                if (machinery.isChainRelated)
                {
                    if (machinery.chainGenerator.ChainData == null)
                    {
                        Debug.LogWarning("Don't forget to Add Chain Data");
                    }
                }
                else
                {
                    if (machinery.chainGenerator.ChainData != null)
                    {
                        machinery.chainGenerator.ResetLinks();
                    }
                }
            }


            if (machinery.isChainRelated)
            {
                EditorGUILayout.Space();
                EditorStyleHelper.DrawSeparatorLine(Color.gray);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("CHAIN PROPERTIES", EditorStyles.boldLabel);

                SetChainData();

                EditorGUILayout.Space();

                if (machinery.chainGenerator == null)
                {
                    Debug.LogWarning("Add Chain Generator Script");
                    return;
                }


                if (machinery.chainGenerator.ChainData != null)
                {
                    FillChainData();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Generate Chain"))
                        GenerateChain();

                    // if (GUILayout.Button("Delete Link Pool"))
                    //     DeleteLinkPool();

                    if (GUILayout.Button("Reset Links"))
                        machinery.chainGenerator.ResetLinks();
                    EditorGUILayout.EndHorizontal();
                }
            }
            
            EditorStyleHelper.DrawSeparatorLine(Color.gray);
            EditorGUILayout.Space();
            Save();
            EditorGUI.EndChangeCheck();
        }

        void Save()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("SAVE CHANGES"))
                machinery.SaveMachinery();

            if (GUILayout.Button("SAVE ONTO EXISTING PREFAB"))
                machinery.SaveOnExistingPrefab();
            EditorGUILayout.EndHorizontal();
        }

        void SetChainData()
        {
            EditorGUILayout.LabelField("Set Chain Data: ", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            machinery.chainGenerator.ChainData = (ChainData) EditorGUILayout.ObjectField(
                "Use Selected Chain Data", machinery.chainGenerator.ChainData, typeof(ChainData),
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
                    machinery.chainGenerator.CreateChainData(chainDataName);
                    newChainData = false;
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (machinery.chainGenerator.ChainData != null &&
                    machinery.chainGenerator.ChainData.LinksPoolPrefab != null)
                    HandleLinksPoolChange();
                //GenerateChain();
            }
        }

        void ShowGizmosOnSelection()
        {
            EditorGUI.BeginChangeCheck();
            machinery.cogHolder.showGizmos =
                EditorGUILayout.Toggle("Show Gizmos On Selected Gear", machinery.cogHolder.showGizmos);
            if (EditorGUI.EndChangeCheck())
            {
                if (!machinery.cogHolder.showGizmos)
                    machinery.cogHolder.DisableAllGizmos();
                else
                    machinery.cogHolder.DrawGizmosOnSelectedCog(selectedIndex);
                Repaint();
            }
        }


        void AddCog(bool isNew, string cogName = null)
        {
            machinery.cogHolder.AddCog(isNew, gearData, cogName);
            cogs = machinery.cogHolder.RestoreCogsInEditor();
            selectedIndex = cogs.Length - 1;
            cogToDestroyIndex = selectedIndex;
            GenerateChain();
            Repaint();
        }

        void RemoveCog()
        {
          
            machinery.cogHolder.RemoveCog(cogToDestroyIndex);
            cogs = machinery.cogHolder.RestoreCogsInEditor();
           

            if (cogs.Length > 1)
            {
                
                if (selectedIndex == cogToDestroyIndex)
                {
                    int newIndex;
                    do
                    {
                        Debug.Log("try");
                        newIndex = UnityEngine.Random.Range(0, cogs.Length);
                    } while (newIndex == selectedIndex);

                    selectedIndex = newIndex;
                }

                cogToDestroyIndex = cogs.Length - 1;
            }
            else if (cogs.Length == 1)
            {
                selectedIndex = cogs.Length - 1;
                cogToDestroyIndex = selectedIndex;
            }
            
            

            GenerateChain();
            Repaint();
        }


        void GenerateChain()
        {
            if (!machinery.isChainRelated) return;
            machinery.chainGenerator.GenerateChain(SaveCogs, machinery.cogHolder.GetChainRelatedCogs());
        }

        void SaveCogs()
        {
            foreach (var cog in cogs)
            {
                EditorUtility.SetDirty(cog.Data);
            }
        }

        void ChangeCogData(int i)
        {
            GUILayout.BeginHorizontal();
            changeCogData = EditorGUILayout.Toggle("Change With Other Data", changeCogData);
            changeWithNewCogData = EditorGUILayout.Toggle("Change With New Data", changeWithNewCogData);
            GUILayout.EndHorizontal();

            if (changeCogData)
            {
                _otherGearData =
                    (GearData) EditorGUILayout.ObjectField("Gear Data", _otherGearData, typeof(GearData), false);
                if (GUILayout.Button("Apply"))
                {
                    if (_otherGearData == null) return;

                    gearData = _otherGearData;
                    machinery.cogHolder.cogs[i].Data = _otherGearData;
                    GenerateChain();
                    changeCogData = false;
                }
            }

            if (changeWithNewCogData)
            {
                cogDataName = EditorGUILayout.TextField("Gear Data Name", cogDataName);
                if (GUILayout.Button("Apply"))
                {
                    cogs[i].Data =
                        machinery.cogHolder
                            .CreateCogData("Cog " + cogDataName); //machineryPrefab.cogHolder.newCogIndex++
                    changeWithNewCogData = false;
                    GenerateChain();
                    Repaint();
                    machinery.SaveMachinery();
                }
            }
        }

        void FillCogData(int i)
        {
            GearData Data = cogs[i].Data;

            if (Data == null) return;

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

            Data.Volume = EditorGUILayout.FloatField("Gear Depth", Data.Volume);

            EditorGUILayout.Space();

            if (machinery.holeAssetHolder != null && machinery.holeAssetHolder.HoleTypes.Count != 0)
            {
                Data.HoleId = EditorGUILayout.Popup("Hole Type", Data.HoleId, machinery.holeAssetHolder.HoleLabels);
            }

            Data.HoleSize = EditorGUILayout.Slider("Hole Size", Data.HoleSize, Data.Radius, 0);
            Data.HoleDepth = EditorGUILayout.FloatField("Hole Depth", Data.HoleDepth);
            Data.ContactType = (ChainEnums.CogContactType) EditorGUILayout.EnumPopup("Contact Type", Data.ContactType);

            if (Data.ContactType == ChainEnums.CogContactType.Indifferent) //!machinery.isChainRelated ||
            {
                Data.IsMoving = EditorGUILayout.Toggle("Is Moving", Data.IsMoving);
                EditorGUI.BeginChangeCheck();
                int newValue = EditorGUILayout.IntPopup("Rotation Direction", Data.RotationDirection, new[] {"-1", "1"},
                    new[] {-1, 1});
                if (EditorGUI.EndChangeCheck())
                    Data.RotationDirection = newValue;
            }

            if (Data.ContactType == ChainEnums.CogContactType.GearRelated)
            {
                Data.relatedGearData =
                    (GearData) EditorGUILayout.ObjectField("Related Cog Data", Data.relatedGearData, typeof(GearData),
                        true);
                Data.IsMoving = true;
            }

            EditorGUILayout.Space();

            if (EditorGUI.EndChangeCheck())
                AccidentalSetupCogsWithSameData(i);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Teeth Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            Data.WithoutTeeth = EditorGUILayout.Toggle("Without Teeth", Data.WithoutTeeth);
            if (EditorGUI.EndChangeCheck())
            {
                if (Data.WithoutTeeth)
                {
                    cogs[i].DeletePool();
                    SetupCogsWithSameData(i);
                }
                else
                    cogs[i].CreateNewPool();
            }

            if (!Data.WithoutTeeth)
            {
                EditorGUI.BeginChangeCheck();
                GUILayout.BeginHorizontal();
                Data.TeethPoolPrefab = (TeethPool) EditorGUILayout.ObjectField("Teeth Pool Prefab",
                    Data.TeethPoolPrefab, typeof(TeethPool), false);
                if (GUILayout.Button("Apply")) //, narrowButton))
                    HandleTeethPoolChange(i);
                GUILayout.EndHorizontal();

                Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
                Data.MinGapLimit = EditorGUILayout.FloatField("Min Gap Limit", Data.MinGapLimit);
                Data.ToothGap = EditorGUILayout.Slider("Tooth Gap", Data.ToothGap, Data.MinGapLimit, 360);
                Data.Equalize = EditorGUILayout.Toggle("Equal Gaps", Data.Equalize);

                EditorGUILayout.Space();
                ChangeCogData(i);

                if (EditorGUI.EndChangeCheck())
                    SetupCogsWithSameData(i);
            }
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


        void FillChainData()
        {
            ChainData chainData = machinery.chainGenerator.ChainData;
            //chainData.OnTesting = EditorGUILayout.Toggle("On Testing", chainData.OnTesting);

            EditorGUI.BeginChangeCheck();
            chainData.RadiusOffset = EditorGUILayout.FloatField("Distance from Gears", chainData.RadiusOffset);
            chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Chain Link Settings", EditorStyles.boldLabel);
            chainData.LinkInterval = EditorGUILayout.FloatField("Link Interval", chainData.LinkInterval);
            chainData.LinkSize = EditorGUILayout.Vector3Field("Link Size", chainData.LinkSize);
            chainData.LinkRotationEffect = EditorGUILayout.Toggle("Rotate Links", chainData.LinkRotationEffect);


            GUILayout.BeginHorizontal();
            chainData.LinksPoolPrefab = (LinksPool) EditorGUILayout.ObjectField("Links Pool Prefab",
                chainData.LinksPoolPrefab, typeof(LinksPool), false);

            if (GUILayout.Button("Apply")) //, narrowButton))
                HandleLinksPoolChange();

            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            chainData.IsMoving = EditorGUILayout.Toggle("Is Moving", chainData.IsMoving);

            if (chainData.IsMoving)
            {
                chainData.SpeedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", chainData.SpeedMultiplier);
                chainData.LinkRotationMultiplier =
                    EditorGUILayout.FloatField("Link Rotation Amount While Moving", chainData.LinkRotationMultiplier);
                chainData.LinkLagAmount = EditorGUILayout.FloatField("Link Lag Amount", chainData.LinkLagAmount);
                chainData.motionDirection =
                    (ChainEnums.ChainDirection) EditorGUILayout.EnumPopup("Motion Direction",
                        chainData.motionDirection);
                chainData.SetMotionByGear = EditorGUILayout.Toggle("Set Motion By Gear", chainData.SetMotionByGear);
            }

            if (EditorGUI.EndChangeCheck())
                GenerateChain();
        }

        void HandleLinksPoolChange()
        {
            machinery.DeleteLinkPool();
            GenerateChain();
        }

        void HandleTeethPoolChange(int i)
        {
            machinery.DeleteTeethPool(i);
            SaveCogs();
        }
    }
}
#endif