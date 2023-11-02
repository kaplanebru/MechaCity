using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNamespace;
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

        [SerializeField] private List<Cogwheel> cogs = new();

        public Transform testCubePb;
        public Transform testSpherePb;
        public Arc[] arcs;
        private int _arcCount;
        public static ChainEnums.UpAxis Upwards;

        private int linearPointAmount;
        [ReadOnly] public List<Vector3> chainPoints = new();

        private void OnEnable()
        {
            cogs.Clear();
            if (!Application.isPlaying)
            {
                ChainEvents.OnCogReady += GetCogs;
                ChainEvents.OnChainRequest += StartChain;
            }
          
            //ChainEvents.OnCogsSelected += GenerateChainBySelection;

            cogs = GetComponentsInChildren<Cogwheel>().ToList();
        }

        private void Start() //runtime ise startta çağrılmaması lazım
        {
            //yield return new WaitUntil(() => cogs.Count == Data.CogAmount);
        }

        void GenerateChain()
        {
            chainPoints.Clear();

            CreateArcs();
            Setup();
            CreateParts(0);
            BindPoints();
        }

        private void StartChain()
        {
            Upwards = Data.UpwardsAxis;
            GenerateChain();
        }

        private void GetCogs(Cogwheel newCog)
        {
            //cogs.Add(newCog);
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
                arcs[i].cog.transform.position,
                relatedArc.cog.transform.position,
                arcs[i].radius,
                relatedArc.radius);


            arc.baseAngle = TrigonometryHelper.AngleBySin(Data.Unit, arcs[i].radius);
            arc.edgeAngles.End = TrigonometryHelper.AngleInPoint(
                tangentPoints[0],
                arc.cog.transform.position);

            relatedArc.edgeAngles.Start =
                TrigonometryHelper.AngleInPoint(
                    tangentPoints[1],
                    relatedArc.cog.transform.position);

            Instantiate(testCubePb, tangentPoints[0], Quaternion.identity);
            Instantiate(testSpherePb, tangentPoints[1], Quaternion.identity).transform.localScale *= 2;
        }

        void CreateParts(int i)
        {
            CreateArcPoints(i);
            PositionPoints(i);
            SetNextPoint(i);
            AddLinearPoints(i);
        }

        void SetArcs()
        {
            for (int i = 0; i < _arcCount; i++)
            {
                arcs[i].id = i;
                arcs[i].cog.SetSpinDirection(Data.motionDirection);

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
                arcPositions[i] = arcs[i].cog.transform.position;
            }

            arcs = new ClockwiseSorter<Arc>(arcs, arcPositions).SortItems();
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
        }

        void PositionPoints(int i)
        {
            var arcPoints = arcs[i].arcPoints;
            var gear = arcs[i].cog;

            for (var j = 0; j < arcPoints.Count; j++)
            {
                var point = arcPoints[j];
                arcPoints[j] = Data.FollowGearRotation
                    ? gear.transform.position + gear.transform.rotation * point
                    : gear.transform.position + point;
            }
        }

        void SetNextPoint(int i)
        {
            var relatedArc = arcs[arcs[i].relatedArcId];

            if (relatedArc.id == 0)
            {
                arcs[i].nextPoint = relatedArc.arcPoints.First();
                return;
            }

            relatedArc.arcPoints.Add(TrigonometryHelper.CirclePoint(relatedArc.edgeAngles.Start,
                relatedArc.radius + Data.Tension));
            PositionPoints(relatedArc.id);
            arcs[i].nextPoint = relatedArc.arcPoints.First(); //bug: hiç point yoksa geliyor
        }


        void AddLinearPoints(int i)
        {
            linearPointAmount =
                TrigonometryHelper.LinearPointAmountByDistance(arcs[i].nextPoint, arcs[i].arcPoints.Last(), Data.Unit);

            Vector3 edgeDirection = (arcs[i].nextPoint - arcs[i].arcPoints.Last()).normalized;
            Vector3 unitDistance = edgeDirection * Data.Unit;

            var arcPoints = arcs[i].arcPoints;
            for (int j = 0; j < linearPointAmount; j++)
            {
                arcPoints.Add(arcPoints.Last() + unitDistance);
            }

            if (arcs[i].relatedArcId == 0) return;
            Arc relatedArc = arcs[arcs[i].relatedArcId];

            float extraAngle = Vector3.Angle(arcPoints.Last(), relatedArc.arcPoints.First());
            relatedArc.edgeAngles.Start =
                (extraAngle + relatedArc.edgeAngles.Start) % 360; //potential bug: 0 da sorun olabilir

            relatedArc.arcPoints.Clear();

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

            ChainEvents.OnPointsCreated?.Invoke(chainPoints);
        }

        void AdaptUnitToCircle()
        {
            Data.Unit = Vector3.Distance(chainPoints[0], chainPoints[1]); //print(chainPoints[1].z);
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                ChainEvents.OnCogReady -= GetCogs;
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