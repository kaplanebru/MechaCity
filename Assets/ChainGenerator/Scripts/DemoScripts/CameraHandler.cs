using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Chain
{
    public class CameraHandler : MonoBehaviour
{
    public Camera mainCam;
    public List<Transform> camPoints;
    public float speed = 0.5f;
    public float ease = 0.1f;
    public Button MoveButton;
    
    
    [Header("Label")]
    public Transform label;
    private Transform _labelImage;
    public float labelEase = 0.1f;

    private Vector3 _startCamPos;
    private Quaternion _startCamRot;
    
    void Start()
    {
       GetTransforms();
    }
    
    public void Play()
    {
        label.gameObject.SetActive(false);
        ChainEvents.OnMovieClipBegin?.Invoke();
        
        SetMainCam(camPoints[0]);
        StartCoroutine(nameof(MoveCam), 0);
    }


    private void GetTransforms()
    {
        _startCamPos = mainCam.transform.position;
        _startCamRot = mainCam.transform.rotation;
        
        camPoints = GetComponentsInChildren<Transform>(true).Where(t => t != transform).ToList();
        
        camPoints.Last().position = _startCamPos;
        camPoints.Last().rotation = _startCamRot;

        _labelImage = label.transform.GetChild(0);

    }

    void SetMainCam(Transform _transform)
    {
        mainCam.transform.position =_transform.position;
        mainCam.transform.rotation =_transform.rotation;
    }
    
    IEnumerator MoveCam(int i)
    {
        while (true)
        {
            if (mainCam.transform.position == camPoints[i].transform.position)
            {
                if (i + 1 == camPoints.Count)
                { label.gameObject.SetActive(true);
                    yield break;
                }
                
                StartCoroutine(MoveCam(i + 1));
                yield break;
            }
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, camPoints[i].transform.position, speed);
            mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, camPoints[i].transform.rotation, ease);
            yield return null;
        }
    }

    IEnumerator LabelAnimation()
    {
        _labelImage.transform.localScale = Vector3.zero;
        label.gameObject.SetActive(true);

        while (Vector3.Distance(_labelImage.transform.localScale, Vector3.one) > 0.01f)
        {
            _labelImage.transform.localScale = Vector3.Lerp(_labelImage.transform.localScale, Vector3.one, labelEase);
            yield return new WaitForFixedUpdate();
        }

        _labelImage.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var buttons = FindObjectsOfType<Button>();
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
            Play();
        }
            
    }
}
}

