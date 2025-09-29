using System.Collections.Generic;
using Chain;
using UnityEngine;

namespace ChainDemo
{
    public class Transposer : MonoBehaviour
    {
      
        private BaseTransposable[] _transposables;
        private List<Vector3> _startPositions = new();
        private List<Quaternion> _startRotations = new();


        private void OnEnable()
        {
            ChainEvents.OnMovieClipBegin += Stop;
        }

        private void Start()
        {
           GetTransposables();
        }

        void GetTransposables()
        {
            _transposables = FindObjectsOfType<BaseTransposable>();
            foreach (var transposable in _transposables)
            {
                _startPositions.Add(transposable.transform.position);
                _startRotations.Add(transposable.transform.rotation);
            }
        }

        void Move()
        {
            foreach (var transposable in _transposables)
            {
                transposable.Initialize();
            }
        }

        void Stop()
        {
            for (var i = 0; i < _transposables.Length; i++)
            {
                var transposable = _transposables[i];
                transposable.Stop();
                transposable.transform.position = _startPositions[i];
                transposable.transform.rotation = _startRotations[i];
            }
        }

        public void Play()
        {
            Stop();
            Move();
        }

        private void OnDisable()
        {
            ChainEvents.OnMovieClipBegin -= Stop;
        }
    }
}