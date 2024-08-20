using Model;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    InventoryHandler[] inventoryHandlers;
    public static UIInventoryController Instance;

    private void Awake()
    {
        Instance = this;
        inventoryHandlers = FindObjectsOfType<InventoryHandler>();
        HideAllItemInUI();
    }

    public void UnlockItemInUI(Items item)
    {
        foreach (var t in inventoryHandlers)
        {
            if(t.item == item)
                t.UnlockItem();
        }
    }
    
    public void CollectItemInUI(Items item)
    {
        foreach (var t in inventoryHandlers)
        {
            if(t.item == item)
                t.ShowItem();
        }
    }
    
    private void HideAllItemInUI()
    {
        foreach (var t in inventoryHandlers)
        {
            t.HideItem();
        }
    }
}
