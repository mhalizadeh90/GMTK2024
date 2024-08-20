using Controller;
using Model;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    public Items item;

    [FormerlySerializedAs("imageDatabase")] [SerializeField]
    private ItemDatabase itemDatabase;

    [Header("Image")] [SerializeField] private Image itemImage;

    private Button _button;
    private Outline _outline;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ShowText);
        _outline = itemImage.GetComponent<Outline>();
        _outline.enabled = false;

        if (itemDatabase.GetItemStatus(item) == ItemStatus.Locked) HideItem();
    }

    private void ShowText()
    {
        if (itemDatabase.GetItemStatus(item) == ItemStatus.Locked) return;

        switch (itemDatabase.GetItemStatus(item))
        {
            case ItemStatus.Found:
                TextController.Instance.ShowText(itemDatabase.GetDescriptionText(item), true);
                break;
            case ItemStatus.Unlocked:
                TextController.Instance.ShowText(itemDatabase.GetHintText(item), true);
                break;
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UnlockItem();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            ShowItem();
        }
    }
#endif

    // When Item is Unlocked and need to be found in the game, this method will be called to put siloute and set it as unlocked for hint text
    public void UnlockItem()
    {
        itemImage.enabled = true;
        _button.interactable = true;
        itemImage.sprite = itemDatabase.GetSilhouetteImage(item);
        itemImage.color = Color.black;
        _outline.enabled = true;
        itemImage.enabled = true;
        itemDatabase.SetItemStatus(item, ItemStatus.Unlocked);
    }


    // When Item is Found, this method will be called to put Full Image and set it as Found for text
    public void ShowItem()
    {
        itemImage.enabled = true;
        _button.interactable = true;
        itemImage.sprite = itemDatabase.GetFullImage(item);
        itemImage.color = Color.white;
        _outline.enabled = false;
        itemImage.enabled = true;
        itemDatabase.SetItemStatus(item, ItemStatus.Found);
    }

    public void HideItem()
    {
        itemImage.enabled = false;
        _button.interactable = false;
    }
}