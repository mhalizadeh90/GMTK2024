using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItmeDatabase", menuName = "Scriptable Objects/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemsData> itemDatas;

        public Sprite GetSilhouetteImage(Items item)
        {
            return (from Item in itemDatas where Item.item == item select Item.silhouetteImage).FirstOrDefault();
        }
        
        public Sprite GetFullImage(Items item)
        {
            return (from Item in itemDatas where Item.item == item select Item.fullImage).FirstOrDefault();
        }
        
        public string GetHintText(Items item)
        {
            return (from Item in itemDatas where Item.item == item select Item.hintToFind).FirstOrDefault();
        }
        
        public string GetDescriptionText(Items item)
        {
            return (from Item in itemDatas where Item.item == item select Item.itemDescription).FirstOrDefault();
        }
        
        public ItemStatus GetItemStatus(Items item)
        {
            return (from Item in itemDatas where Item.item == item select Item.itemStatus).FirstOrDefault();
        }

        public void SetItemStatus(Items item, ItemStatus newStatus)
        {
            for (var i = 0; i < itemDatas.Count; i++)
            {
                if (itemDatas[i].item == item)
                {
                    itemDatas[i].itemStatus = newStatus;
                    return;
                }
            }
        }
    }
}