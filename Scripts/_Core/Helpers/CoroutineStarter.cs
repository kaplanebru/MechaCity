using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoroutineStarter : MonoBehaviour
{
   private void OnEnable()
   {
      Eventbus.CombatEvents.OnCoroutineTrigger += StartGivenCoroutine;
   }

   public void StartGivenCoroutine(IEnumeratorContainer enumeratorContainer)
   {
      StartCoroutine(enumeratorContainer.EnumeratorInstance());
   }

   private void OnDisable()
   {
      Eventbus.CombatEvents.OnCoroutineTrigger -= StartGivenCoroutine;
   }
}

public interface IEnumeratorContainer
{
   IEnumerator EnumeratorInstance();
}