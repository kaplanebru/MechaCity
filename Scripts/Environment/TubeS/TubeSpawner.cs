using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Teams;
using Towers;
using UnityEngine;

public class TubeSpawner : MonoBehaviour
{
   public Tube tubeObj;
   public Color selectionColor;
   public Color defaultColor;


   private List<Tower> towers = new();
   private List<Vector3> _directions = new();
   private List<Tube> _tubes = new();
   private Tube _currentTube;

   // private void OnEnable()
   // {
   //    GeneralEventbus.OnTowersCreated += GetTowers;
   //    Eventbus.StateEvents.OnLinkStateBegin += DeselectAll;
   //    
   //    GeneralEventbus.OnTurnTowerSelection += ToSelection;
   //    GeneralEventbus.OnTurnTowerDeselect += Deselect;
   // }

   private void GetTowers()
   {
      for (int i = 0; i < AllTowers.TowersCount; i++)
      {
          towers.Add(AllTowers.GetTower(i));
      }
      
      SetDirections();
      CreateTubes();
   }

   private void Deselect(int id)
   {
      _currentTube = _tubes.FirstOrDefault(t=>t.id == id);
      _currentTube.SetColor(defaultColor);
   }
   private void ToSelection(int id)
   {
      _currentTube = _tubes.FirstOrDefault(t=>t.id == id); //_tubes[index]
      _currentTube.SetColor(selectionColor);
   }

   private void DeselectAll()
   {
      _tubes.ForEach(t=>t.SetColor(defaultColor));
   }

   void SetDirections()
   {
      foreach (var tower in towers)
      {
         var dir = (tower.transform.position - transform.position).normalized;
         _directions.Add(new Vector3(dir.x, 0, dir.z).normalized);
      }
   }
   
   void CreateTubes()
   {
      for (var i = 0; i < _directions.Count; i++)
      {
         var dir = _directions[i];
         var tube = Instantiate(tubeObj, transform);
         
         tube.transform.rotation = Quaternion.LookRotation(dir);
         tube.id = towers[i].Data.UniqID;
         _tubes.Add(tube);
      }
   }

   private void OnDisable()
   {
      GeneralEventbus.OnTowersCreated -= GetTowers;
      Eventbus.StateEvents.OnLinkStateBegin -= DeselectAll;
   
      GeneralEventbus.OnTurnTowerSelection -= ToSelection;
      GeneralEventbus.OnTurnTowerDeselect -= Deselect;
   }
}
