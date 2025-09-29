using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoroutineStarter : MonoBehaviour
{
   private void OnEnable()
   {
      GeneralEventbus.OnCoroutineTrigger += StartGivenCoroutine;
   }

   public void StartGivenCoroutine(IEnumeratorContainer enumeratorContainer)
   {
      StartCoroutine(enumeratorContainer.LeCoroutine());
   }

   private void OnDisable()
   {
      GeneralEventbus.OnCoroutineTrigger -= StartGivenCoroutine;
   }
}

public interface IEnumeratorContainer
{
   IEnumerator LeCoroutine();
}