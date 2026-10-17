using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //selectable item slot for inventory system
    public GameObject selectedItem;
    public bool isSelected;
    //Item Slot properties
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private int maxStackSize = 99;
    [SerializeField] private Image itemIconImage;
    public string itemName;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool isFulled;

    private InventoryManager inventoryManager;
    
    public void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        selectedItem.SetActive(false);
        isSelected = false;
    }

    public void AddItem(ItemDetails itemDetails)
    {
        itemName = itemDetails.ItemName;
        itemIcon = itemDetails.ItemIcon;
        AddQuantity(itemDetails.Quantity);
    }
    
    public bool CanStackWith(string otherItemName)
    {
        return !isFulled && itemName == otherItemName;
    }

    public void AddQuantity(int amount)
    {
        itemQuantity += amount;
        isFulled = itemQuantity >= maxStackSize;

        quantityText.text = itemQuantity.ToString();
        quantityText.enabled = true;
        itemIconImage.sprite = itemIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
    }

    private void LeftClick()
    {
        inventoryManager.DesselectAllSlots();
        Debug.Log("Left click on item: " + itemName);
        selectedItem.SetActive(true);
        isSelected = true;
        
    }
    private void RightClick()
    {
        Debug.Log("Right click on item: " + itemName);
        
    }   
}
