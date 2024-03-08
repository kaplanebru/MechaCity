using Clicks;
using GameUI;
using UnityEngine;


namespace Towers
{
    //[RequireComponent(typeof(TowerParts))]
    public class Tower : MonoBehaviour
    {
        //Shooter, RiserFall, Selectable Components diye 3'e ayrılabilir

        public TowerConstantData ConstantData;
        public TowerData Data;
        public TowerParts towerParts;
        public ClickHandler clickHandler;

        

        private void OnEnable()
        {
            towerParts = GetComponent<TowerParts>();
            clickHandler = GetComponent<ClickHandler>();
        }

        public void Setup(TeamTowerData teamTowerData)
        {
            Data.Height = ConstantData.StartHeight;
            Data.Health = ConstantData.StartHealth;
            Data.DamagePower = ConstantData.DamagePower;
            RestoreBullets();
            
            UIEventbus.OnHealthChange.Invoke(Data.Health, gameObject);
            clickHandler.SetClickables(Data.UniqID);
            SetTeam(teamTowerData);//for tower and clickables
        }

        public void SetTeam(TeamTowerData teamTowerData)
        {
            Data.TeamTowerData = teamTowerData;
            //towerParts.SetColor(teamTowerData.DefaultMaterial);
            ToOriginalColor();
            clickHandler.SetClickableTeams(teamTowerData.TeamType);
        }

        // public void ToGivenColor(Material givenMat)
        // {
        //     towerParts.SetColor(givenMat);
        // }

        public void ToFreezeColor()
        {
            towerParts.SetColor(Data.TeamTowerData.FreezeMaterial);
        }

        public void ToBlueprintColor()
        {
            towerParts.SetColor(Data.TeamTowerData.BlueprintMaterial);
        }

        public void ToSelectionColor()
        {
            towerParts.SetColor(Data.TeamTowerData.SelectedMaterial);
        }

        public void ToOriginalColor()
        {
            towerParts.SetColor(Data.TeamTowerData.DefaultMaterial);
        }

        public void RestoreBullets() //Todo: name change: bullet hakkı
        {
            Data.BulletAmount = ConstantData.MaxBullet;
        }
    }
}