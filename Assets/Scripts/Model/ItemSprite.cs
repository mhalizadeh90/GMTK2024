using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class ItemsData
    {
        public Items item;
        public ItemStatus itemStatus;
        public string hintToFind;
        public string itemDescription;
        public Sprite fullImage;
        public Sprite silhouetteImage;
    }
}