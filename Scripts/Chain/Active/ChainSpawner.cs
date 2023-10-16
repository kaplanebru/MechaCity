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

        public Transform testCubePb;
        public Arc[] arcs;


        private int linearPointAmount;
        private Vector3 _center;

        [ReadOnly] public List<Vector3> chainPoints = new();

        private void Start()
        {
            chainPoints.Clear();
            Setup();
            // CreateParts(0);
            //BindPoints();
        }

        void Setup()
        {
            ChainEvents.OnStartAndMove?.Invoke(Data.IsMoving);
            _center = TrigonometryHelper.CenterDirection(arcs);
            SetArcs();
            RelateArcs();
            EbrusWay(0);
            //CommonTangentPoint(0);
            //CommonIntersectionPoint(0);


            /*RotateGearToCenter(0);

            for (var i = 0; i < arcs.Length; i++)
            {
                SetAngles(i);
            }*/
        }

        private float unitOffset = 0.5f;

        void EbrusWay(int i)
        {
            Arc relatedArc = arcs[arcs[i].relatedArcId];
            // Vector3 posA = new Vector3(), posB = new Vector3();
            // float radiusA, radiusB;

            Vector3[] tangentPoints = TrigonometryHelper.CommonTangentPoints(
                    arcs[i].gear.transform.position, 
                    relatedArc.gear.transform.position,
                    arcs[i].radius, 
                    relatedArc.radius, 
                    unitOffset);
            

            Instantiate(testCubePb, tangentPoints[1], Quaternion.identity);
            Instantiate(testCubePb, tangentPoints[0], Quaternion.identity);
        }

        void CommonTangentPoint(int i)
        {
            Arc relatedArc = arcs[arcs[i].relatedArcId];
            var posA = arcs[i].gear.transform.position;
            var posB = relatedArc.gear.transform.position;
            var radiusA = arcs[i].radius;
            var radiusB = relatedArc.radius;


            print(radiusB);

            // Vector3 InternalSection = new Vector3();
            //
            // InternalSection.x = (radiusA * posB.x + radiusB * posA.x) / radiusA + radiusB;
            // InternalSection.z = (radiusA * posB.z + radiusB * posA.z) / radiusA + radiusB;
            //Instantiate(testCubePb, InternalSection, Quaternion.identity);


            Vector3 ExternalSection = new Vector3();
            ExternalSection.x = (radiusA * posB.x + radiusB * posA.x) / radiusA + radiusB;
            ExternalSection.z = (radiusA * posB.z + radiusB * posA.z) / radiusA + radiusB;
            Instantiate(testCubePb, ExternalSection, Quaternion.identity);
        }

        void CommonIntersectionPoint(int i)
        {
            Arc relatedArc = arcs[arcs[i].relatedArcId];
            var posA = arcs[i].gear.transform.position;
            var posB = relatedArc.gear.transform.position;
            float distance = Vector3.Distance(posA, posB);

            Vector3 pointA = new Vector3();
            pointA.x = posA.x + (arcs[i].radius * (posB.x - posA.x)) / distance;
            pointA.z = posA.z + (arcs[i].radius * (posB.z - posA.z)) / distance;


            Vector3 pointB = new Vector3();
            pointB.x = posB.x + (relatedArc.radius * (posA.x - posB.x)) / distance;
            pointB.z = posB.z + (relatedArc.radius * (posA.z - posB.z)) / distance;


            Instantiate(testCubePb, pointB, Quaternion.identity);
            Instantiate(testCubePb, pointA, Quaternion.identity);

            // P1_x = A_x + (r1 * (B_x - A_x)) / d
            // P1_y = A_y + (r1 * (B_y - A_y)) / d

            // P2_x = B_x + (r2 * (A_x - B_x)) / d
            // P2_y = B_y + (r2 * (A_y - B_y)) / d
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
            arcs[i].baseAngle = TrigonometryHelper.AngleBySin(Data.Unit, arcs[i].radius);

            arcs[i].edgeAngles = arcs.Length <= 2
                ? new EdgeAngles(arcs[i].baseAngle, Vector2.zero)
                : new EdgeAngles(arcs[i].baseAngle, arcs[i].edgeSmoother);
        }

        void CreateArcPoints(int i)
        {
            for (float j = arcs[i].edgeAngles.Start; j <= arcs[i].edgeAngles.End; j += arcs[i].baseAngle)
            {
                var newAngle = j;
                arcs[i].arcPoints.Add(TrigonometryHelper.CirclePoint(newAngle, arcs[i].radius));
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

            relatedArc.arcPoints.Add(TrigonometryHelper.CirclePoint(relatedArc.edgeAngles.Start, relatedArc.radius));
            PositionPoints(relatedArc.id);
            arcs[i].nextPoint = relatedArc.arcPoints.First(); //bug: hiç point yoksa geliyor

            float lengthA = Vector3.Distance(_center, relatedArc.gear.transform.position);
            float lengthB = Vector3.Distance(arcs[i].gear.transform.position, relatedArc.gear.transform.position);
            float lengthC = Vector3.Distance(_center, arcs[i].gear.transform.position);

            float distanceAngle1 = TrigonometryHelper.GetAngleByAllLength(lengthA, lengthB, lengthC);
            float distanceAngle2 = TrigonometryHelper.GetAngleByAllLength(lengthC, lengthB, lengthB);

            print(distanceAngle1 + " " + distanceAngle2);
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


    public class EdgeAngles
    {
        public float Start;
        public float End;

        public EdgeAngles(float baseAngle, Vector2 edgeSmoother)
        {
            Start = baseAngle * edgeSmoother[0];
            End = 180 - baseAngle * edgeSmoother[1];
        }
    }
}