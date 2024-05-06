using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//DISTANCE BASED
namespace Chain
{
    public interface IChainGenerator
    {
        ChainGeneratorData Data { get; set; }
        public List<ChainLink> ExecutePhase();
        //public dynamic ExecutePhase<T>() where T : new();
    }

    public class ChainPointCreator : IChainGenerator 
    {
        public ChainGeneratorData Data { get; set; }
        
        private ChainData ChainData;
        private readonly List<Cogwheel> _cogs;

        public ChainPointCreator(Cogwheel[] cogs, ChainGeneratorData data)
        {
            Data = data;
            ChainData = Data.ChainData;
            _cogs = cogs.ToList();
        }
        
        private Arc[] _arcs;
        private int _arcCount;
        private int _linearPointAmount;
        private bool _stopExecution = false;

        public List<ChainLink> ExecutePhase()
        {
            StartChain();
            IChainGenerator chainDrawer = new ChainDrawer(Data);
            return chainDrawer.ExecutePhase();
        }
        
        void StartChain()
        {
            _stopExecution = false;
            Data.ChainPoints = new();
            Data.ChainPoints.Clear();
            GenerateChainPoints();
        }


        void GenerateChainPoints()
        {
            CreateArcs();
            Setup();
            CreateParts(0);
            BindPoints();
        }

        void CreateArcs()
        {
            _arcCount = _cogs.Count;
            _arcs = new Arc[_arcCount];
            for (int i = 0; i < _cogs.Count; i++)
            {
                _arcs[i] = new Arc(_cogs[i]);
                _arcs[i].edgeAngles = new EdgeAngles(); //temp
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

            //ChainEvents.OnMotionStateSet?.Invoke(ChainData.IsMoving);
        }

        void CommonTangentAngles(int i)
        {
            Arc arc = _arcs[i];
            Arc relatedArc = _arcs[_arcs[i].relatedArcId];

            Vector3[] tangentPoints = TrigonometryHelper.CommonTangentPoints(
                _arcs[i].cog.transform.localPosition,
                relatedArc.cog.transform.localPosition,
                _arcs[i].radius,
                relatedArc.radius);


            arc.baseAngle = TrigonometryHelper.AngleBySin(ChainData.LinkInterval, _arcs[i].radius);
            arc.edgeAngles.End = TrigonometryHelper.AngleInCirclePoint(
                tangentPoints[0],
                arc.cog.transform.localPosition) - ChainData.Tension * arc.radius;


            relatedArc.edgeAngles.Start =
                TrigonometryHelper.AngleInCirclePoint(
                    tangentPoints[1],
                    relatedArc.cog.transform.localPosition) + ChainData.Tension * relatedArc.radius;
        }

        void CreateParts(int i)
        {
            CreateArcPoints(i);
            if (_stopExecution) return;
            PositionPoints(i);
            SetNextArcPoint(i);
            AddLinearPoints(i);
        }

        void SetArcs()
        {
            for (int i = 0; i < _arcCount; i++)
            {
                _arcs[i].id = i;

                if (ChainData.SetRadiusByGear)
                    _arcs[i].SetRadiusByGear(ChainData.RadiusOffset);
                else
                    _arcs[i].radius += ChainData.RadiusOffset;
            }
        }

        void OrderArcsClockwise()
        {
            var arcPositions = new Vector3[_arcs.Length];
            for (int i = 0; i < _arcs.Length; i++)
            {
                arcPositions[i] = _arcs[i].cog.transform.localPosition;
            }

            _arcs = new ClockwiseSorter<Arc>(_arcs, arcPositions).SortItems();
        }

        void RelateArcs()
        {
            for (int i = 0; i < _arcCount; i++)
            {
                _arcs[i].relatedArcId = (i + 1) % _arcCount;
            }
        }

        void CreateArcPoints(int i)
        {
            var start = _arcs[i].edgeAngles.Start;
            var end = _arcs[i].edgeAngles.End;
            float angle = _arcs[i].baseAngle;
            float a = start;

            end -= angle;
            if (end < 0)
                end = (end + 360) % 360;

            while (a < end)
            {
                _arcs[i].arcPoints.Add(TrigonometryHelper.CirclePoint(a, _arcs[i].radius));
                a -= angle;
                if (a < 0)
                {
                    a = (a + 360) % 360;
                    break;
                }
            }

            while (a >= end)
            {
                _arcs[i].arcPoints.Add(TrigonometryHelper.CirclePoint(a, _arcs[i].radius));
                a -= angle;
            }


            if (_arcs[i].arcPoints.Count == 0)
            {
                Debug.LogWarning("Not enough links for chain generation / or try decreasing the link interval");
                _stopExecution = true;
                return;
            }

            _arcs[i].arcPoints[_arcs[i].arcPoints.Count - 1] = LastPointOffset(i);
        }


        Vector3 LastPointOffset(int i) //todo: add to trig helper
        {
            var lastPointAngle = TrigonometryHelper.AngleInCirclePoint(_arcs[i].arcPoints.Last(), Vector3.zero);
            var alphaDegrees = 90 - Mathf.Abs(lastPointAngle - _arcs[i].edgeAngles.End);
            alphaDegrees = (alphaDegrees + 360) % 360;
            float opposite = _arcs[i].radius;
            float alphaRadians = Mathf.Deg2Rad * alphaDegrees;
            float hypotenuse = opposite / Mathf.Sin(alphaRadians);
            var direction = _arcs[i].arcPoints.Last().normalized;
            return Vector3.zero + hypotenuse * direction;
        }

        Vector3 PositionSinglePoint(Cogwheel cog, Vector3 point)
        {
            var positionedPoint = cog.transform.localPosition + point; // + cog.transform.localRotation * point;
            return positionedPoint;
        }

        void PositionPoints(int i)
        {
            var arcPoints = _arcs[i].arcPoints;
            var cog = _arcs[i].cog;

            for (var j = 0; j < arcPoints.Count; j++)
            {
                var point = arcPoints[j];
                arcPoints[j] = PositionSinglePoint(cog, point);
            }
        }

        void SetNextArcPoint(int i)
        {
            var relatedArc = _arcs[_arcs[i].relatedArcId];

            if (relatedArc.id == 0)
            {
                _arcs[i].nextArcPoint = relatedArc.arcPoints.First();
                return;
            }

            Vector3 point = TrigonometryHelper.CirclePoint(relatedArc.edgeAngles.Start,
                relatedArc.radius); // + Data.Tension));

            relatedArc.arcPoints.Add(PositionSinglePoint(relatedArc.cog, point));
            _arcs[i].nextArcPoint = relatedArc.arcPoints.First(); //bug: hiç point yoksa geliyor
        }


        void AddLinearPoints(int i)
        {
            _linearPointAmount =
                TrigonometryHelper.LinearPointAmountByDistance(_arcs[i].nextArcPoint, _arcs[i].arcPoints.Last(),
                    ChainData.LinkInterval);

            Vector3 edgeDirection =
                (_arcs[i].nextArcPoint - _arcs[i].arcPoints.Last()).normalized; //arcs[i].arcPoints.Last()
            Vector3 unitDistance = edgeDirection * ChainData.LinkInterval;

            var arcPoints = _arcs[i].arcPoints;
            for (int j = 0; j < _linearPointAmount; j++)
            {
                arcPoints.Add(arcPoints.Last() + unitDistance); //
            }

            if (_arcs[i].relatedArcId == 0) return;
            Arc relatedArc = _arcs[_arcs[i].relatedArcId];


            var lastPointDistance = relatedArc.arcPoints.First() - arcPoints.Last();
            var rest = unitDistance - lastPointDistance;
            rest += relatedArc.arcPoints.First();

            float newAngle = TrigonometryHelper.AngleInCirclePoint(rest, relatedArc.cog.transform.localPosition);
            relatedArc.edgeAngles.Start = newAngle;

            relatedArc.arcPoints.Clear(); //parceque first changes

            CreateParts(relatedArc.id);
        }

        void BindPoints()
        {
            int i = 0;
            while (true)
            {
                Data.ChainPoints.AddRange(_arcs[i].arcPoints);
                i = _arcs[i].relatedArcId;
                if (i == 0) break;
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