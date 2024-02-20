
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
        public BpType type;
        public BlueprintData Data;
        public SpriteRenderer spriteHolder;

        public TextMeshPro titleHolder;
        public TextMeshPro descriptionHolder;


        public BpType currentBpType;

        private void OnMouseDown() //TODO: Ray'in çarptığı slotun enumından da click enum'ı invoke edilebilir
        {
            NetworkEventbus.BlueprintEvents.OnBpSelected?.Invoke(currentBpType); //zaten network event olacak: 2 playerda da uygulanacağı için
            //TODO: Aslında burda tıklayınca da değil, ortadaki blueprint merkezine götürülünce invoke olacak!
        }

        public void Setup(BlueprintData data)
        {
            Data = data;
            SetImage();
            SetTexts();
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
