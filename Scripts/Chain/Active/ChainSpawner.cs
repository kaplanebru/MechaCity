using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

//DISTANCE BASED
namespace Chain
{
    [RequireComponent(typeof(ChainDrawer))]
    public class ChainSpawner : MonoBehaviour
    {
        public ChainData Data;

        public Transform testCubePb;
        public Arc[] arcs;


        private int linearPointAmount;
        private Vector3 _center;

        [ReadOnly] public List<Vector3> chainPoints = new();

        private void Start()
        {
            chainPoints.Clear();
            Setup();
           
            CreateParts(0);
            

            BindPoints();
        }

        void Setup()
        {
            ChainEvents.OnStartAndMove?.Invoke(Data.IsMoving);
            _center = TrigonometryHelper.CenterDirection(arcs);
            SetArcs();
            RelateArcs();
            for (int i = 0; i < arcs.Length; i++)
            {
                CommonTangentAngles(i);
            }
        }

        private float unitOffset = 0.5f;

        void CommonTangentAngles(int i)
        {
            Arc arc = arcs[i];
            Arc relatedArc = arcs[arcs[i].relatedArcId];

            Vector3[] tangentPoints = TrigonometryHelper.CommonTangentPoints(
                arcs[i].gear.transform.position,
                relatedArc.gear.transform.position,
                arcs[i].radius,
                relatedArc.radius,
                unitOffset);


            arc.baseAngle = TrigonometryHelper.AngleBySin(Data.Unit, arcs[i].radius);
            arc.edgeAngles.End = TrigonometryHelper.AngleInPoint(
                tangentPoints[0], 
                arc.gear.transform.position);

            relatedArc.edgeAngles.Start =
                TrigonometryHelper.AngleInPoint(
                    tangentPoints[1],
                    relatedArc.gear.transform.position);
           
            Instantiate(testCubePb, tangentPoints[0], Quaternion.identity);
            Instantiate(testCubePb, tangentPoints[1], Quaternion.identity);
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
            for (int i = 0; i < arcs.Length; i++)
            {
                arcs[i].id = i;
                arcs[i].gear.Data.RotationDirection = Data.motionDirection == ChainDirection.Clockwise ? 1 : -1;
                if (Data.SetRadiusByObject)
                    arcs[i].SetRadiusByGear();
            }
        }

        void RelateArcs()
        {
            for (int i = 0; i < arcs.Length; i++)
            {
                arcs[i].relatedArcId = (i + 1) % arcs.Length;
            }
        }
        
        void CreateArcPoints(int i)
        {
            var start = arcs[i].edgeAngles.Start;
            var end = arcs[i].edgeAngles.End;


            Calculation:
            if (start > end)
            {
                for (float j = start; j >= end; j -= arcs[i].baseAngle) //% ekle
                {
                    var newAngle = j;
                    arcs[i].arcPoints.Add(TrigonometryHelper.CirclePoint(newAngle, arcs[i].radius));
                }
            }
            else
            {
                for (float j = start; j < end; j -= arcs[i].baseAngle)
                {
                    j = (j + 360) % 360;
                    if (j > start)
                    {
                        start = j;
                        goto Calculation;
                    }
                    arcs[i].arcPoints.Add(TrigonometryHelper.CirclePoint(j, arcs[i].radius));
                   
                }
            }
        }

        void PositionPoints(int i)
        {
            var arcPoints = arcs[i].arcPoints;
            var gear = arcs[i].gear;

            for (var j = 0; j < arcPoints.Count; j++)
            {
                var point = arcPoints[j];
                arcPoints[j] = gear.transform.position + point; //+ gear.transform.rotation * point;
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

            relatedArc.arcPoints.Add(TrigonometryHelper.CirclePoint(relatedArc.edgeAngles.Start, relatedArc.radius));
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
            relatedArc.edgeAngles.Start = (extraAngle + relatedArc.edgeAngles.Start) % 360; //potential bug: 0 da sorun olabilir

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
    }


    [Serializable]
    public class EdgeAngles
    {
        public Vector2 EdgeSmoother;
        private float start;
        public float Start
        {
            get { return start; }
            set
            {
                start = value; // * EdgeSmoother.x;
            }
        }

        private float end;

        public float End
        {
            get { return end; }
            set
            {
                end = value; // * (EdgeSmoother.y * baseAngle);
            }
        }
        

        // public EdgeAngles(float startAngle, float endAngle, )
        // {
        //     Start = startAngle; //* edgeSmoother[0];
        //     End = endAngle; //* edgeSmoother[1];
        // }
    }
}