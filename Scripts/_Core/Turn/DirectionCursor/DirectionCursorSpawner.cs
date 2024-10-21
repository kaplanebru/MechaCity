using System.Collections.Generic;
using System.Linq;
using Actor;
using Towers;
using UnityEngine;

public class DirectionCursorSpawner : MonoBehaviour
{
    public DirectionCursor cursorPrefab;
    public List<DirectionCursor> directionCursors;
    
    public int heightOffset = 0;
    private void OnEnable()
    {
        GeneralEventbus.InitializerEvents.OnTowersCreated += Initialize;
       // Eventbus.CombatEvents.OnPairsSet += SetPositions;
    }

    public void Initialize()
    {
        CreateCursors();
        //SetPositions();
    }

    void CreateCursors()
    {
        for (int i = 0; i < AllTowers.TowersCount; i++)
        {
            directionCursors.Add(Instantiate(cursorPrefab, transform));
            directionCursors.Last().id = i;
        }
    }

    void SetPositions()
    {
        for (int i = 0; i < ActorHolder.Registry.Count; i++)
        {
            var cursor = directionCursors[i];
            
            var pos1 = CalculatePos(ActorHolder.Registry.ElementAt(i).Value.TowerIDs);
            int nextID = (i + 1) % AllTowers.TowersCount;
            var pos2 = CalculatePos(ActorHolder.Registry.ElementAt(nextID).Value.TowerIDs);
            
            cursor.transform.localPosition = (pos1 + pos2) / 2;
            cursor.transform.rotation = Quaternion.LookRotation((pos2 - pos1).normalized);
            cursor.transform.localPosition += Vector3.up * heightOffset; //temp
        }

        Vector3 CalculatePos(int[] towers)
        {
            var pos = Vector3.zero;
            foreach (var tower in towers)
            {
                pos += AllTowers.GetTower(tower).transform.position;
            }

            return pos / towers.Length;
        }
        
        // for (int i = 0; i < AllTowers.TowersCount; i++)
        // {
        //     var cursor = directionCursors[i];
        //     var pos1 = AllTowers.GetTower(i).transform.position;
        //     var pos2 = AllTowers.GetTower((i + 1) % AllTowers.TowersCount).transform.position;
        //     
        //     cursor.transform.localPosition = (pos1 + pos2) / 2;
        //     cursor.transform.rotation = Quaternion.LookRotation((pos2 - pos1).normalized);
        //     cursor.transform.localPosition += Vector3.up * heightOffset; //temp
        // }
    }
    

    private void OnDisable()
    {
        GeneralEventbus.InitializerEvents.OnTowersCreated -= Initialize;
        //Eventbus.CombatEvents.OnPairsSet -= SetPositions;
    }

    //o zaman cursorların towerlardan haberi olmalı
    //TODO: target ölüyse cursor'ın rengi solar, target vurulamazsa da rengi değişir
}
