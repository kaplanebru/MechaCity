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

            //SetCircularPoints();
            //SetLinearPoints();
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


           
            CreateArcPoints(0);
            PositionPoints(0); //not recursive, only for the first arc
            SetConnectionPoints(0);
            AddLinearPoints(0);
            
            
           
        }

       

        void SetArcs()
        {
            for (int i = 0; i < arcs.Length; i++)
            {
                arcs[i].id = i;
                if(Data.SetRadiusByObject)
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

        void SetCircularPoints()
        {
            //SetArcRotation(0);
            SetAngles(0);
            PositionPoints(0);
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

            arcs[i].edgeAngles = arcs.Length <= 2
                ? new EdgeAngles(0, 180)
                : new EdgeAngles(arcs[i].baseAngle * arcs[i].edgeSmoother, 180 - arcs[i].baseAngle * arcs[i].edgeSmoother);
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

            // if (arcs[i].relatedArcId == 0) return;
            // PointRotationAndPositions(arcs[i].relatedArcId);
        }

        void SetLinearPoints()
        {
            //SetConnectionPoints(0);
            AddLinearPoints(0);
        }


        void SetConnectionPoints(int i)
        {
            var relatedArc = arcs[arcs[i].relatedArcId];
            
            SetAngles(relatedArc.id);
            //CreateArcPoints(relatedArc.id);
            relatedArc.arcPoints.Add(ChainHelper.CirclePoint(relatedArc.edgeAngles.Start, relatedArc.radius));
            PositionPoints(relatedArc.id); 
            
            arcs[i].connectionPoint = relatedArc.arcPoints.First(); //bug: hiç point yoksa geliyor

           
        }


        void AddLinearPoints(int i)
        {
           
            linearPointAmount =
                ChainHelper.LinearPointAmountByDistance(arcs[i].connectionPoint, arcs[i].arcPoints.Last(), Data.Unit);

            linearPointAmount++;
           
            Vector3 edgeDirection = (arcs[i].connectionPoint - arcs[i].arcPoints.Last()).normalized;

            var unitDistance = edgeDirection * Data.Unit;

            
            var arcPoints = arcs[i].arcPoints;
            for (int j = 0; j < linearPointAmount; j++)
            {
                arcPoints.Add(arcPoints.Last() + unitDistance);
            }

            if(arcs[i].relatedArcId == 0) return;
            var relatedArc = arcs[arcs[i].relatedArcId];
            

            float sin = Vector3.Distance(relatedArc.arcPoints.First(), arcPoints.Last());
            float extraAngle = ChainHelper.AngleByDistance(sin, relatedArc.radius); // - relatedArc.edgeAngles.Start;

            // Vector3 dir = (arcPoints.Last() - relatedArc.gear.transform.position).normalized;
            // var extraAngle = Vector3.Angle(relatedArc.arcPoints[0], -dir * relatedArc.radius);

           // float extraAngle = Vector3.Angle(arcPoints.Last(), relatedArc.arcPoints.First());
            
            print(extraAngle);
            relatedArc.edgeAngles.Start += extraAngle;
            print(relatedArc.edgeAngles.Start);
            relatedArc.arcPoints.Clear();
            CreateArcPoints(relatedArc.id);
            PositionPoints(relatedArc.id); //not recursive, only for the first arc
            arcs[i].connectionPoint = relatedArc.arcPoints[0];
            if (i == 1) //related arcı 0 olan yani
            {
                arcs[i].connectionPoint = relatedArc.arcPoints.First();
            }
            else
            {
                SetConnectionPoints(relatedArc.id);
            }
                
            AddLinearPoints(relatedArc.id);
           
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

    public class EdgeAngles
    {
        public float Start;
        public float End;

        public EdgeAngles(float start, float end)
        {
            Start = start;
            End = end;
        }
    }
    
    //AutoSmoother
    // float arcDifferance = mainRadius - arcParts[arcParts[i].relatedArcId].radius;
    // if (arcDifferance > 0 && Mathf.Abs(arcDifferance) > 3)
    // {
    //     edgeAngles.Start = -baseAngle;
    //     edgeAngles.End = 180 + baseAngle;
    // }
}