using Model;
using ScriptableObjects;
using UnityEngine;

namespace Controller
{
    public class ItemSelectController : MonoBehaviour
    {
        public static ItemSelectController Instance;
        [SerializeField] private Items[] unlockOrder;
        [SerializeField] private ItemDatabase itemDatabase;
        private HiddenItem[] itemsInScene;
        private int _itemIndexToSearch;

        private void Awake()
        {
            Instance = this;
            _itemIndexToSearch = 0;
            itemsInScene = FindObjectsOfType<HiddenItem>();
        }

        private void Start()
        {
            ActivateNextItem(exclude: unlockOrder[_itemIndexToSearch]);
        }

        public void CollectItem(HiddenItem item)
        {
            // Add it to inventory
            UIInventoryController.Instance.CollectItemInUI(unlockOrder[_itemIndexToSearch]);

            // Show a Text so you collect it
            TextController.Instance.ShowText(itemDatabase.GetDescriptionText(unlockOrder[_itemIndexToSearch]), true, callback:ActivateNextItemAction);
// TODO: FIX THE BUG -> ONCE THE SHOW TEXT FOR THE COLLECTED IS GOING TO BE DISPLAYED, THEN IN A SECOND THE NEXT ONE TIP WILL BE DISPLAYED CAUSE TO HIDE THE EARLIER SHOW MESSAGE
            
            
            // Unlock The Next one
            _itemIndexToSearch++;
          //  DisableItems(unlockOrder[_itemIndexToSearch]);
        }

        void ActivateNextItemAction()
        {
            if (_itemIndexToSearch >= itemsInScene.Length)
            {
                return;
            }
           
            ActivateNextItem(unlockOrder[_itemIndexToSearch]);
        }
        private void ActivateNextItem(Items? exclude = null)
        {
            for (var i = 0; i < itemsInScene.Length; i++)
            {
                if (exclude != null && itemsInScene[i].item == exclude)
                {
                    
                    itemsInScene[i].EnableItem();
                    UIInventoryController.Instance.UnlockItemInUI(unlockOrder[_itemIndexToSearch]);
                    TextController.Instance.ShowText(itemDatabase.GetHintText(unlockOrder[_itemIndexToSearch]), true, 3);
                    continue;
                }
                    itemsInScene[i].DisableItem();
            }
        }
    }
}