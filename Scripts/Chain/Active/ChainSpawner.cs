using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Unity.Collections;
using UnityEngine;

//DISTANCE BASED
namespace Chain
{
    [RequireComponent(typeof(ChainDrawer))]
    public class ChainSpawner : MonoBehaviour
    {
        public ChainData Data;

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
            _center = ChainHelper.CenterDirection(arcs);
            SetArcs();
            RelateArcs();
            RotateGearToCenter(0);

            for (var i = 0; i < arcs.Length; i++)
            {
                SetAngles(i);
            }
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
                if (Data.SetRadiusByObject)
                    arcs[i].SetRadiusByGear();
            }
        }

        void RelateArcs()
        {
            for (int i = 0; i < arcs.Length; i++)
            {
                var id = i - 1;
                if (id < 0) id += arcs.Length;
                arcs[i].relatedArcId = id;
            }
        }
        

        void RotateGearToCenter(int i)
        {
            var mainArc = arcs[i];
            var direction = (mainArc.gear.transform.position - _center).normalized;
            mainArc.gear.transform.rotation = Quaternion.LookRotation(direction);

            if (mainArc.relatedArcId == 0) return;
            RotateGearToCenter(mainArc.relatedArcId);
        }

        void SetAngles(int i)
        {
            arcs[i].baseAngle = ChainHelper.AngleByDistance(Data.Unit, arcs[i].radius);

            // if (arcs.Length <= 2)
            //     arcs[i].edgeAngles = new EdgeAngles(0, 180);
            // else
            // {
            //     
            // }

            arcs[i].edgeAngles = arcs.Length <= 2
                ? new EdgeAngles(arcs[i].baseAngle, 0)
                : new EdgeAngles(arcs[i].baseAngle, arcs[i].edgeSmoother);
        }

        void CreateArcPoints(int i)
        {
            for (float j = arcs[i].edgeAngles.Start; j <= arcs[i].edgeAngles.End; j += arcs[i].baseAngle)
            {
                var newAngle = j;
                arcs[i].arcPoints.Add(ChainHelper.CirclePoint(newAngle, arcs[i].radius));
            }
        }

        void PositionPoints(int i)
        {
            var arcPoints = arcs[i].arcPoints;
            var gear = arcs[i].gear;

            for (var j = 0; j < arcPoints.Count; j++)
            {
                var point = arcPoints[j];
                arcPoints[j] = gear.transform.position + gear.transform.rotation * point;
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
            
            relatedArc.arcPoints.Add(ChainHelper.CirclePoint(relatedArc.edgeAngles.Start, relatedArc.radius));
            PositionPoints(relatedArc.id);
            arcs[i].nextPoint = relatedArc.arcPoints.First(); //bug: hiç point yoksa geliyor
        }


        void AddLinearPoints(int i)
        {
            linearPointAmount = ChainHelper.LinearPointAmountByDistance(arcs[i].nextPoint, arcs[i].arcPoints.Last(), Data.Unit);
            
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
            relatedArc.edgeAngles.Start -= extraAngle;
            
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

            if (Data.Type == ChainType.BikeChain)
            {
                if (Vector3.Distance(chainPoints.Last(), chainPoints.First()) < Data.Unit)
                {
                    var dir = (chainPoints[^2] - chainPoints.Last()).normalized;
                    chainPoints[^1] = chainPoints.Last() + dir; //TODO: * unit*0.4f;
                }
            }

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
        public float Start;
        public float End;
        public float EdgeSmoother;

        public EdgeAngles(float baseAngle, float edgeSmoother)
        {
            EdgeSmoother = edgeSmoother;
            Start = baseAngle * edgeSmoother;
            End = 180 - baseAngle * edgeSmoother;
        }
        
    }
    
}