using System.Collections.Generic;
using Actor;
using DataModels;
using DG.Tweening;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform combatTransform;
    
    public float linkDistance = 15;
    public float linkRotationOffset = 10;

    public CombatTimingData timingData;
    public Ease ease;
    private Vector3 startPos;
    private Quaternion startRot;

    private LinkCam LinkCam = new();

    private void OnEnable()
    {
        Eventbus.LinkEvents.OnLinkActorsLoaded += SwitchToLinkCam;
        Eventbus.CombatEvents.OnCombatReady += SwitchToCombatCam;
        Eventbus.CombatEvents.OnCombatTerminated += ResetCam;
    }
    
    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void SwitchToLinkCam(List<uint> ids)
    {
        List<Vector3> centers = new();
        foreach (var id in ids)
        {
            centers.Add(ActorHolder.GetActor(id).Center);
        }

        var center = LinkCam.GetCenter(centers.ToArray());
        var dir = (center - transform.position).normalized;
        var rot = Quaternion.LookRotation(dir);
        rot *= Quaternion.Euler(-linkRotationOffset, 0, 0);
        //center = new Vector3(center.x, 0, center.z) + linkTransform.position;
        Move(dir * -linkDistance + center, rot);
    }

    void SwitchToCombatCam()
    {
        Move(combatTransform.position, combatTransform.rotation);
    }

    void ResetCam()
    {
        Move(startPos, startRot);
    }

    void Move(Vector3 pos, Quaternion rot)
    {
        transform.DOMove(pos, timingData.cameraDelay).SetEase(ease);
        transform.DORotateQuaternion(rot, timingData.cameraDelay).SetEase(ease);
    }

    private void OnDisable()
    {
        Eventbus.LinkEvents.OnLinkActorsLoaded -= SwitchToLinkCam;
        Eventbus.CombatEvents.OnCombatReady -= SwitchToCombatCam;
        Eventbus.CombatEvents.OnCombatTerminated -= ResetCam;
    }
}
