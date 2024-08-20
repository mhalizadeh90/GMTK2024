using Controller;
using Model;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HiddenItem : MonoBehaviour
{
    [Tooltip("Shaddow Gameobject so once click on it, it become black")][SerializeField] private GameObject highlight;
    [Tooltip("Minimum distance So the player can click on this Object")][SerializeField] private float minDistance;
    [Tooltip("Maximum distance So the player can click on this Object")][SerializeField] private float maxDistance;
    [SerializeField] private ItemStatus status;
    [SerializeField] private BoxCollider2D col2D;
    public Items item;

    void OnMouseDown()
    {
        SelectItem();
    }

    public bool IsItemClickable()
    {
        return (CameraController.CameraDistance >= minDistance && CameraController.CameraDistance <= maxDistance);
    }

    public void SelectItem()
    {
        if(status == ItemStatus.Locked)
            return;
        
        if (highlight)
        {
            highlight.SetActive(true);
        }
       
        // The rest of item found logic here ===> Send it to ItemSelectController To Decide and unlock the next one
        ItemSelectController.Instance.CollectItem(this);
    }

    public void DisableItem()
    {
        col2D.enabled = false;
        status = ItemStatus.Locked;
    }

    public void EnableItem()
    {
        col2D.enabled = true;
        status = ItemStatus.Unlocked;
    }
}