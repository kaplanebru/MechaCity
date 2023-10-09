using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainMover : MonoBehaviour
{
    [SerializeField] private List<Transform> _chains = new();
    public float speed = 0.1f;

    public void Setup(List<Transform> chains)
    {
        _chains = chains;
        StartCoroutine(nameof(MoveRoutine));
    }

    IEnumerator MoveRoutine()
    {
        for (int i = 0; i < _chains.Count; i++)
        {
            if (i == _chains.Count - 1)
            {
                _chains[i].transform.position =  Vector3.Lerp(_chains[i].transform.position, 
                    _chains[0].transform.position, speed);
                // _chains[i].transform.rotation =
                //     Quaternion.Lerp(_chains[i].transform.rotation, _chains[0].transform.rotation, speed);

                i = 0;
                yield return new WaitForFixedUpdate();
                //continue;
            }
            _chains[i].transform.position =
                Vector3.Lerp(_chains[i].transform.position, 
                    _chains[i + 1].transform.position, speed);
            // _chains[i].transform.rotation =
            //     Quaternion.Lerp(_chains[i].transform.rotation, _chains[i + 1].transform.rotation, speed);
            yield return new WaitForFixedUpdate();
        }
    }
}
