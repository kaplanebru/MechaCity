using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

//DISTANCE BASED
namespace Chain
{
    [RequireComponent(typeof(ChainDrawer))]
    [ExecuteInEditMode]
    public class ChainSpawner : MonoBehaviour
    {
        public ChainData Data;

        [SerializeField] private List<Cogwheel> cogs = new(); //Selected cog vs olacağı için list

        public Transform testCubePb;
        public Transform testSpherePb;
        public Transform testCube2Pb;
        private Transform testCube;
        private Transform testSphere;
        private List<Transform> testCubes = new();

        
        public Arc[] arcs;
        private int _arcCount;
       

        private int linearPointAmount;
        [ReadOnly] public List<Vector3> chainPoints = new();

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                ChainEvents.OnChainRequest += StartChain;
            }
            //ChainEvents.OnCogsSelected += GenerateChainBySelection;
        }

        void GenerateChain()
        {
            if (Data.OnTesting)
            {
                for (int i = testCubes.Count - 1; i >= 0; i--)
                {
                    if(testCubes[i] != null)
                        DestroyImmediate(testCubes[i].gameObject);
                }
            }

            chainPoints.Clear();

            CreateArcs();
            Setup();
            CreateParts(0);
            BindPoints();
        }

        private void StartChain(Cogwheel[] _cogs, ChainSpawner chainSpawner)
        {
            if (chainSpawner != this) return;
            cogs = _cogs.ToList();
            if (cogs.Count <= 1) return;
            GenerateChain();
        }


        void CreateArcs()
        {
            _arcCount = Data.CogAmount;
            arcs = new Arc[_arcCount];
            for (int i = 0; i < cogs.Count; i++)
            {
                arcs[i] = new Arc(cogs[i]);
                arcs[i].edgeAngles = new EdgeAngles(); //temp
            }
        }

        void Setup()
        {
            OrderArcsClockwise();
            SetArcs();
            RelateArcs();
            for (int i = 0; i < _arcCount; i++)
            {
                CommonTangentAngles(i);
            }

            ChainEvents.OnMotionStateSet?.Invoke(Data.IsMoving);
        }

        

        void CommonTangentAngles(int i)
        {
            Arc arc = arcs[i];
            Arc relatedArc = arcs[arcs[i].relatedArcId];

            Vector3[] tangentPoints = TrigonometryHelper.CommonTangentPoints(
                arcs[i].cog.transform.localPosition,
                relatedArc.cog.transform.localPosition,
                arcs[i].radius,
                relatedArc.radius);


            arc.baseAngle = TrigonometryHelper.AngleBySin(Data.Unit, arcs[i].radius);
            arc.edgeAngles.End = TrigonometryHelper.AngleInCirclePoint(
                tangentPoints[0],
                arc.cog.transform.localPosition) - Data.Tension * arc.radius;


            relatedArc.edgeAngles.Start =
                TrigonometryHelper.AngleInCirclePoint(
                    tangentPoints[1],
                    relatedArc.cog.transform.localPosition) + Data.Tension * relatedArc.radius;

            DebugTangentPoints(tangentPoints[0], tangentPoints[1]);
        }

       

        void CreateParts(int i)
        {
            CreateArcPoints(i);
            PositionPoints(i);
            SetNextArcPoint(i);
            AddLinearPoints(i);
        }

        void SetArcs()
        {
            for (int i = 0; i < _arcCount; i++)
            {
                arcs[i].id = i;
                arcs[i].cog.SetSpinDirectionByChain(Data.motionDirection);

                if (Data.SetRadiusByGear)
                    arcs[i].SetRadiusByGear(Data.RadiusOffset);
                else
                    arcs[i].radius += Data.RadiusOffset;
            }
        }

        void OrderArcsClockwise()
        {
            var arcPositions = new Vector3[arcs.Length];
            for (int i = 0; i < arcs.Length; i++)
            {
                arcPositions[i] = arcs[i].cog.transform.localPosition;
            }

            arcs = new ClockwiseSorter<Arc>(arcs, arcPositions).SortItems();
            // foreach (var arc in arcs)
            // {
            //     print(arc.cog.name);
            // }
        }

        void RelateArcs()
        {
            for (int i = 0; i < _arcCount; i++)
            {
                arcs[i].relatedArcId = (i + 1) % _arcCount;
            }
        }
        
        void CreateArcPoints(int i)
        {
            var start = arcs[i].edgeAngles.Start;
            var end = arcs[i].edgeAngles.End;
            float angle = arcs[i].baseAngle;
            float a = start;
            
            end -= angle;
            if (end < 0)
                end = (end + 360) % 360;
            
            while (a < end)
            {
                arcs[i].arcPoints.Add(TrigonometryHelper.CirclePoint(a, arcs[i].radius));
                a -= angle;
                if (a < 0)
                {
                    a = (a + 360) % 360;
                    break;
                }
            }

            while (a >= end)
            {
                arcs[i].arcPoints.Add(TrigonometryHelper.CirclePoint(a, arcs[i].radius));
                a -= angle;
            }

            arcs[i].arcPoints[arcs[i].arcPoints.Count - 1] = LastPointOffset(i);
        }

        Vector3 LastPointOffset(int i) //todo: add to trig hepler
        {
            var lastPointAngle = TrigonometryHelper.AngleInCirclePoint(arcs[i].arcPoints.Last(), Vector3.zero);
            var alphaDegrees = 90 - Mathf.Abs(lastPointAngle - arcs[i].edgeAngles.End);
            alphaDegrees = (alphaDegrees + 360) % 360;
            float opposite = arcs[i].radius;
            float alphaRadians = Mathf.Deg2Rad * alphaDegrees;
            float hypotenuse = opposite / Mathf.Sin(alphaRadians);
            var direction = arcs[i].arcPoints.Last().normalized;
            return Vector3.zero + hypotenuse * direction;

        }

        Vector3 PositionSinglePoint(Cogwheel cog, Vector3 point)
        {
            var positionedPoint = cog.transform.localPosition + point;// + cog.transform.localRotation * point;
            return positionedPoint;
        }
        
        void PositionPoints(int i)
        {
            
            var arcPoints = arcs[i].arcPoints;
            var cog = arcs[i].cog;

            for (var j = 0; j < arcPoints.Count; j++)
            {
                var point = arcPoints[j];
                arcPoints[j] = PositionSinglePoint(cog, point);
            }
        }

        void SetNextArcPoint(int i)
        {
            var relatedArc = arcs[arcs[i].relatedArcId];

            if (relatedArc.id == 0)
            {
                arcs[i].nextArcPoint = relatedArc.arcPoints.First();
                return;
            }

            Vector3 point = TrigonometryHelper.CirclePoint(relatedArc.edgeAngles.Start,
                relatedArc.radius); // + Data.Tension));

            relatedArc.arcPoints.Add(PositionSinglePoint(relatedArc.cog, point));
            arcs[i].nextArcPoint = relatedArc.arcPoints.First(); //bug: hiç point yoksa geliyor
        }


       
        void AddLinearPoints(int i)
        {
            if(Data.OnTesting)
                testCubes.Add(Instantiate(testCube2Pb, arcs[i].arcPoints.Last(), Quaternion.identity));
            
            linearPointAmount =
                TrigonometryHelper.LinearPointAmountByDistance(arcs[i].nextArcPoint, arcs[i].arcPoints.Last(),
                    Data.Unit);

            Vector3 edgeDirection = (arcs[i].nextArcPoint - arcs[i].arcPoints.Last()).normalized; //arcs[i].arcPoints.Last()
            Vector3 unitDistance = edgeDirection * Data.Unit;

            var arcPoints = arcs[i].arcPoints;
            for (int j = 0; j < linearPointAmount; j++)
            {
                arcPoints.Add(arcPoints.Last() + unitDistance); //
            }

            if (arcs[i].relatedArcId == 0) return;
            Arc relatedArc = arcs[arcs[i].relatedArcId];

            
            var lastPointDistance = relatedArc.arcPoints.First() - arcPoints.Last();
            var rest = unitDistance - lastPointDistance;
            rest += relatedArc.arcPoints.First();

            float newAngle = TrigonometryHelper.AngleInCirclePoint(rest, relatedArc.cog.transform.localPosition);
            relatedArc.edgeAngles.Start = newAngle;
            
           

//            print("last point distance: " + lastPointDistance + " unit: " + unitDistance + " extraAngle: " + newAngle);


            relatedArc.arcPoints.Clear(); //çünkü first'ün yeri değişiyor.

            CreateParts(relatedArc.id);
        }

        void BindPoints()
        {
            int i = 0;
            while (true)
            {
                chainPoints.AddRange(arcs[i].arcPoints);
                i = arcs[i].relatedArcId;
                if (i == 0) break;
            }


            // if (Data.Type == ChainType.BikeChain)
            // {
            //     if (Vector3.Distance(chainPoints.Last(), chainPoints.First()) < Data.Unit)
            //     {
            //         var dir = (chainPoints[^2] - chainPoints.Last()).normalized;
            //         chainPoints[^1] = chainPoints.Last() + dir; //TODO: * unit*0.4f;
            //     }
            // }

            //ChainEvents.OnPointsCreated?.Invoke(chainPoints);
            GetComponent<ChainDrawer>().DrawChain(chainPoints);
        }

        void AdaptUnitToCircle()
        {
            Data.Unit = Vector3.Distance(chainPoints[0], chainPoints[1]); //print(chainPoints[1].z);
        }
        
        void DebugTangentPoints( Vector3 tangent0, Vector3 tangent1)
        {
            // if (testSphere != null)
            // {
            //     DestroyImmediate(testSphere.gameObject);
            //     DestroyImmediate(testCube.gameObject);
            // }
            //
            if(!Data.OnTesting) return;
            testCube = Instantiate(testCubePb, tangent0, Quaternion.identity);
            testSphere = Instantiate(testSpherePb, tangent1, Quaternion.identity);
            testSphere.transform.localScale *= 2;
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                ChainEvents.OnChainRequest -= StartChain;
            }
        }
    }


    [Serializable]
    public class EdgeAngles
    {
        public float Start;
        public float End;
    }
}