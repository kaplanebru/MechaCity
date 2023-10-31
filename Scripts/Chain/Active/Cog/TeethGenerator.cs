using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNamespace;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class TeethGenerator : MonoBehaviour
    {
        private CogData Data;
        public Tooth toothPb;
        private Transform _teethParent;
        float _intervalAngle = 60;

        private void OnEnable()
        {
             ChainEvents.OnCogDataSet += ReadyForTeethCreation;
            //ChainEvents.OnPoolReady += ReadyForTeethCreation;
        }


        void ReadyForTeethCreation(CogData data, Transform teethParent)
        {
            if (teethParent.position != transform.position) return;

            Data = data;
            _teethParent = teethParent;
            CreateTeethPoints();
        }


        void SetIntervalAngle()
        {
            _intervalAngle = Data.ToothGap / (Data.Radius);
            if (Data.Equalize)
                _intervalAngle = TrigonometryHelper.Angle360(_intervalAngle);

            if (_intervalAngle < Data.MinGapLimit)
                _intervalAngle = Data.MinGapLimit;
        }


        public void CreateTeethPoints() //CogData data, Transform teethParent //params object[] args
        {
            ResetTeeth();

            Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y,
                1f / transform.localScale.z);
            SetIntervalAngle();

            int counter = 0;
            for (float i = 0; i < 360; i += _intervalAngle)
            {
                
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);
                Tooth tooth;
                

                if (teeth.Count > 0)
                {
                    if (counter < teeth.Count)
                    {
                        teeth[counter].gameObject.SetActive(true);
                        tooth = teeth[counter];
                        
                    }
                    else
                    {
                        tooth = Instantiate(toothPb);
                        teeth.Add(tooth);
                    }
                }
                else //TODO: BURASI 1 KEZ ÇAĞRILIYOR LİSTEYE EKLENDİĞİ İÇİN
                {
                    tooth = Instantiate(toothPb);
                    teeth.Add(tooth);
                }
                
                counter++;
        
               
               

                
                tooth.transform.position = transform.position + transform.rotation * point;
                tooth.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);
                tooth.transform.localRotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                    ? Quaternion.LookRotation(point)
                    : Quaternion.LookRotation(point, Vector3.forward);
                
                

                // Tooth tooth = ToothPool.Instance.GetItem(t =>
                // {
                //     t.transform.position = transform.position + transform.rotation * point;
                //     //t.transform.parent = _teethParent;
                //
                //     t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);
                //
                //
                //     t.transform.localRotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                //         ? Quaternion.LookRotation(point)
                //         : Quaternion.LookRotation(point, Vector3.forward);
                // });
                
                tooth.transform.SetParent(transform.GetChild(1)); //_teethParent //BUG FİX : teethparenttan patlıyormuş


                
                //TODO: follow olmayan koşlda takip etmesin
            }

            // if (teeth.Count < counter)
            // {
            //     int rest = counter - teeth.Count;
            //     for (int i = rest; i < teeth.Count; i++)
            //     {
            //         teeth[i].gameObject.SetActive(false);
            //     }
            // }
            SetTeethInfo(teeth.Count, Vector3.Distance(teeth[0].transform.position, teeth[1].transform.position));
            print(counter);
        }


        private void SetTeethInfo(int teethCount, float toothUnit)
        {
            Data.TeethCount = teethCount;
            Data.ToothUnit = toothUnit;
        }

        public List<Tooth> teeth = new();

        void ResetTeeth()
        {
            //teeth.Clear();
            teeth = GetComponentsInChildren<Tooth>(true).ToList();
            teeth.ForEach(t=>t.gameObject.SetActive(false));
        }

        public void DeleteTeeth()
        {
            ResetTeeth();
            // for (int i = 0; i < teeth.Count; i++)
            // {
            //     var tooth = teeth[0];
            //     teeth.Remove(tooth);
            //     DestroyImmediate(tooth.gameObject);
            // }

            for (int i = teeth.Count - 1; i >= 0; i--)
            {
                var tooth = teeth[i];
                teeth.Remove(tooth);
                DestroyImmediate(tooth.gameObject);
            }
        }
        void ResetTeeth2()
        {
            //if (!Application.isEditor) return;
            if (teeth.Count == 0)
            {
                print("zero");


                List<Tooth> deadTeeth = _teethParent.GetComponentsInChildren<Tooth>().ToList();
                if (deadTeeth.Count == 0) return;
                deadTeeth.ForEach(t =>
                {
                    t.transform.parent = ToothPool.Instance.transform;
                    ToothPool.Instance.ReleaseItem(t);
                });
                teeth.Clear();
            }
            else
            {
                print("not zero");
                teeth.ForEach(t =>
                {
                    if (t != null)
                    {
                        print(ToothPool.Instance.name);
                        t.transform.parent = ToothPool.Instance.transform;
                        ToothPool.Instance.ReleaseItem(t);
                    }
                });
                teeth.Clear();
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnCogDataSet -= ReadyForTeethCreation;
            //ChainEvents.OnPoolReady -= ReadyForTeethCreation;

        }
    }
}