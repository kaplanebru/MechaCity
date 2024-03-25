
using System;
using DataModels;
using Enums;
using Network;
using TMPro;
using UnityEngine;

namespace Blueprint
{
    public class BPSlot : MonoBehaviour
    {
       
        public BpType currentBpType;
        public BlueprintData Data;
        public int level = 1;
        
        public BPInteraction bpInteraction;
        public SpriteRenderer spriteHolder;

        public TextMeshPro titleHolder;
        public TextMeshPro descriptionHolder;
        private void OnMouseDown() //TODO: Ray'in çarptığı slotun enumından da click enum'ı invoke edilebilir
        {
            //NetworkEventbus.BlueprintEvents.OnBpSelected?.Invoke(currentBpType); //zaten network event olacak: 2 playerda da uygulanacağı için
            //TODO: Aslında burda tıklayınca da değil, ortadaki blueprint merkezine götürülünce invoke olacak!
            print(currentBpType);
        }

        public void Setup(BlueprintData data)
        {
            Data = data;
            SetImage();
            SetTexts();
            Data.Level = level; //todo: check, ref type diye burdan yapılabilir diye düşündüm
            bpInteraction.Setup(data);
        }

        public void SetType(BpType type)
        {
            currentBpType = type;
        }

        void SetImage()
        {
            if (spriteHolder == null) return;
            spriteHolder.sprite = Data.Sprite;
        }


        void SetTexts()
        {
            titleHolder.text = Data.Title;
            descriptionHolder.text = Data.Description;
        }

        void SetColor()
        {
            GetComponentInChildren<MeshRenderer>().material.color = Data.Color;
        }
    }

}
