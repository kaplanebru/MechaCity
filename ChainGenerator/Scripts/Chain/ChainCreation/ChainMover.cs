using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    public interface Mover
    {
        public float MachinerySpeed { get; set; }
        public int MachineryId { get; set; }

        public ChainEnums.ChainDirection MachineryDirection { get; set; }

        public void StartMotion();

        public void StopMotion();

        public void MachinerySetup(float machinerySpeed, int machineryId, IMachinePartData data,
            ChainEnums.ChainDirection direction) {}
    }

    public class ChainMover : MonoBehaviour, Mover
    {
        public float MachinerySpeed { get; set; }
        public int MachineryId { get; set; }
        public ChainEnums.ChainDirection MachineryDirection { get; set; }

        public ChainData Data;

        private int _cogAmount;

        [SerializeField] private List<ChainLink> _links = new();
        [SerializeField] private List<Vector3> _points = new();
        private List<Quaternion> _rotations = new();
        private List<Coroutine> runningCoroutines = new List<Coroutine>();
        
        public float LinearSpeed = 0;
        private int _counter = 0;
        private float _totalCogSpeed = 0;
        private float _rotationExtentPerLink;
        
        private bool _speedSet = false;
        private float _speed;
        
        private int _oldCogAmount = 0;
        private bool _pause = false;

        private void OnEnable()
        {
            ChainEvents.OnCogSpeedSet += GetTotalCogSpeed;
        }

        public void MachinerySetup(float machinerySpeed, int machineryId, IMachinePartData machinePartData,
            ChainEnums.ChainDirection direction)
        {
            MachinerySpeed = machinerySpeed;
            MachineryId = machineryId;
            Data = machinePartData as ChainData;
            MachineryDirection = direction;
        }

        public void Setup(List<ChainLink> links, int cogAmount)
        {
            _links = links;
            _cogAmount = cogAmount;
        }

        private void GetTotalCogSpeed(float cogSpeed, int machineryId)
        {
            if (MachineryId != machineryId) return;
            if (!Data.SetMotionByGear) return;

            _totalCogSpeed += cogSpeed;
            _counter++;

            if (_counter != _cogAmount) return;
            _counter = 0;
            SetSpeed();
        }

        void ResetCogValues()
        {
            _totalCogSpeed = 0;
            _counter = 0;
        }
        
        void SetSpeed()
        {
            LinearSpeed = _totalCogSpeed / _cogAmount / _links.Count; // * 1.3f; 

            _speedSet = true;
            ResetCogValues();
        }
        
        void GetRotationPoints()
        {
            _points.Clear();
            _rotations.Clear();
            foreach (var link in _links)
            {
                _rotations.Add(link.transform.localRotation);
                _points.Add(link.transform.localPosition);
            }
        }

        public void StartMotion()
        {
            _pause = false;
            StopCoroutine(nameof(MoveRoutine));
            
            if (_cogAmount > 1 && _oldCogAmount == _cogAmount)
                PauseLinkRoutines();
            else
                StopLinkRoutines();
            
            if(_cogAmount > 1)
                StartCoroutine(nameof(MoveRoutine));
        }

        void PauseLinkRoutines()
        {
            var lastPointIndex = _links[0].pointIndex;
            for (var i = 0; i < _links.Count; i++) 
            {
                var link = _links[i];
                link.transform.localPosition = _points[(lastPointIndex + i) % _points.Count];
                link.transform.localRotation = _rotations[(lastPointIndex + i) % _points.Count];
            }
        }

        void StopLinkRoutines()
        {
            runningCoroutines.ForEach(StopCoroutine);
            runningCoroutines.Clear();
        }

        public void StopMotion()
        {
            _pause = true;
            _oldCogAmount = _cogAmount;
            
            if(_cogAmount <= 1)
                StopLinkRoutines();
        }
        
        public IEnumerator MoveRoutine()
        {
            if (!Data.IsMoving) yield break;
            if (!Data.SetMotionByGear) _speedSet = true;

            yield return new WaitUntil(() => _speedSet);
            _speedSet = false;

            MoveChain();
        }
        
        void MoveChain()
        {
            _pause = false;
            if (Data.motionDirection == ChainEnums.ChainDirection.None)
            {
                Debug.LogWarning("Motion Direction is set to None");
                return;
            }

            GetRotationPoints();

            _speed = Data.SetMotionByGear ? LinearSpeed * Data.SpeedMultiplier : Data.SpeedMultiplier;
            _rotationExtentPerLink = _speed * Data.LinkRotationMultiplier;


            for (int i = 0; i < _links.Count; i++)
            {
                Coroutine coroutine = StartCoroutine(LinkMotionRoutine(i, _speed));
                runningCoroutines.Add(coroutine);
            }
        }
        
        IEnumerator LinkMotionRoutine(int Index, float speed)
        {
            int pointIndex = Index;

            while (true)
            {
                if (_pause) yield break;

                switch (Data.motionDirection)
                {
                    case ChainEnums.ChainDirection.Clockwise:
                        pointIndex++;
                        pointIndex %= _points.Count;
                        break;
                    case ChainEnums.ChainDirection.ReverseClock:
                        pointIndex--;
                        if (pointIndex < 0)
                            pointIndex = _points.Count - 1;
                        break;
                }

                _links[Index].pointIndex = pointIndex;

                if (pointIndex >= _points.Count) //for debug
                    yield break;
                
                
                while (Vector3.Distance(_links[Index].transform.localPosition, _points[pointIndex]) > 0.001f) //link takip offseti  0.001f
                {
                    if (_pause) yield break;

                    _links[Index].transform.localPosition = Vector3.MoveTowards(
                        _links[Index].transform.localPosition,
                        _points[pointIndex], speed);

                    _links[Index].transform.localRotation = Quaternion.Slerp(
                        _links[Index].transform.localRotation,
                        _rotations[pointIndex], _rotationExtentPerLink);
                    
                    yield return new WaitForFixedUpdate();
                }

                if (!_pause)
                {
                    _links[Index].transform.localPosition = _points[pointIndex];
                }

                if (_pause) yield break;
            }
        }//varacağı yere ulaşamadan durdurunca sıkıntı çıkıyor.
        
        private void OnDisable()
        {
            ChainEvents.OnCogSpeedSet -= GetTotalCogSpeed;
            StopCoroutine(MoveRoutine());
        }

        #region Reset

        /* void ResetLinkPositions()
       {
           if (_points.Count == 0) return;
           for (var i = 0; i < _links.Count; i++)
           {
               _links[i].transform.position = _points[(i + 1) % _links.Count];
               _links[i].transform.rotation = _rotations[(i + 1) % _links.Count];
           }
       }*/

        #endregion
    }
}