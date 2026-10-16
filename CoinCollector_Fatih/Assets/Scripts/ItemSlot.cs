using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private int maxStackSize = 99;
    [SerializeField] private Image itemIconImage;
    public string itemName;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool isFulled;

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
}
