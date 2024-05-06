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
        private int counter = 0;
        private float totalCogSpeed = 0;
        private float _rotationExtentPerLink;
        
        private void OnEnable()
        {
            ChainEvents.OnCogSpeedSet += GetTotalCogSpeed;
            StopCoroutine(MoveRoutine());
        }

        public void StartMotion()
        {
            StartCoroutine(nameof(MoveRoutine));
        }

        public void StopMotion()
        {
            StopCoroutine(nameof(MoveRoutine));
            runningCoroutines.ForEach(StopCoroutine);
            runningCoroutines.Clear(); // Clear the list of running coroutines
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

        public IEnumerator MoveRoutine()
        {
            if (!Data.IsMoving) yield break;
            if (!Data.SetMotionByGear) _speedSet = true;

            yield return new WaitUntil(() => _speedSet);
            _speedSet = false;

            MoveChain();
        }


        private void GetTotalCogSpeed(float cogSpeed, int machineryId)
        {
           
            if (MachineryId != machineryId) return;
            if (!Data.SetMotionByGear) return;
            
            totalCogSpeed += cogSpeed;
            counter++;

            if (counter != _cogAmount) return;
            counter = 0;
            SetSpeed();
        }

        void ResetCogValues()
        {
            totalCogSpeed = 0;
            counter = 0;
        }


        private bool _speedSet = false;
        private float speed;

        void SetSpeed()
        {
            LinearSpeed = totalCogSpeed / _cogAmount / _links.Count; // * 1.3f; 

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

        void MoveChain()
        {
            if (Data.motionDirection == ChainEnums.ChainDirection.None)
            {
                Debug.LogWarning("Motion Direction is set to None");
                return;
            }
            GetRotationPoints();
            speed = Data.SetMotionByGear ? LinearSpeed * Data.SpeedMultiplier : Data.SpeedMultiplier;

            _rotationExtentPerLink = speed * Data.LinkRotationMultiplier;

            
            for (int i = 0; i < _links.Count; i++)
            {
                Coroutine coroutine = StartCoroutine(LinkMotionRoutine(i, speed));
                runningCoroutines.Add(coroutine);
            }
        }

        IEnumerator LinkMotionRoutine(int startIndex, float speed)
        {
            int j = startIndex;

            while (true)
            {
                switch (Data.motionDirection)
                {
                    case ChainEnums.ChainDirection.Clockwise:
                        j++;
                        j %= _points.Count;
                        break;
                    case ChainEnums.ChainDirection.ReverseClock:
                        j--;
                        if (j < 0)
                            j = _points.Count - 1;
                        break;
                }
                while (Vector3.Distance(_links[startIndex].transform.localPosition, _points[j]) > 0.001f) //0.1f
                {
                    _links[startIndex].transform.localPosition = Vector3.MoveTowards(
                        _links[startIndex].transform.localPosition,
                        _points[j], speed);

                    _links[startIndex].transform.localRotation = Quaternion.Slerp(
                        _links[startIndex].transform.localRotation,
                        _rotations[j], _rotationExtentPerLink);

                    yield return new WaitForFixedUpdate();
                }

                _links[startIndex].transform.localPosition = _points[j];
                //_links[startIndex].transform.rotation = _rotations[j];
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnCogSpeedSet -= GetTotalCogSpeed;
            StopCoroutine(MoveRoutine());
        }
    }
}