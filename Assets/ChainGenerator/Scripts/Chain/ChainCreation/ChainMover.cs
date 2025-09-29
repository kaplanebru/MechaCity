using System.Collections;
using System.Collections.Generic;
using ChainInGame;
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

        public int _cogAmount;
        
        public AlteredState AlteredState;
        public OngoingState OngoingState;
        private BaseMovingChainState _currentState;


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
        public bool pause = false;

        private void OnEnable()
        {
            ChainEvents.OnCogSpeedSet += GetTotalCogSpeed;
            AlteredState = new AlteredState(this);
            OngoingState = new OngoingState(this);
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
            SwitchState(AlteredState);
            _links = links;
            _cogAmount = cogAmount;
        }
        
        public void SwitchState(BaseMovingChainState newMovingChainState)
        {
            _currentState = newMovingChainState;
            _currentState.EnterState();
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
        
        void GetPointsAndRotations()
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
            _currentState.StartMotion();
        }
        
        public void StopLinkRoutines()
        {
            runningCoroutines.ForEach(StopCoroutine);
            runningCoroutines.Clear();
        }

        public void StopMotion()
        {
            _currentState.StopMotion();
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
            pause = false;
            if (Data.motionDirection == ChainEnums.ChainDirection.None)
            {
                Debug.LogWarning("Motion Direction is set to None");
                return;
            }

            GetPointsAndRotations();

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

                while (Vector3.Distance(_links[Index].transform.localPosition, _points[pointIndex]) > Data.LinkLagAmount) //link takip offseti  0.001f
                {
                    if(pause) yield return new WaitWhile(() => pause);

                    _links[Index].transform.localPosition = Vector3.MoveTowards(
                        _links[Index].transform.localPosition,
                        _points[pointIndex], speed);

                    _links[Index].transform.localRotation = Quaternion.Slerp(
                        _links[Index].transform.localRotation,
                        _rotations[pointIndex], _rotationExtentPerLink);
                    
                    yield return new WaitForFixedUpdate();
                }

                if (!pause)
                {
                    _links[Index].transform.localPosition = _points[pointIndex];
                }
            }
        }
        
        private void OnDisable()
        {
            ChainEvents.OnCogSpeedSet -= GetTotalCogSpeed;
        }
    }
}