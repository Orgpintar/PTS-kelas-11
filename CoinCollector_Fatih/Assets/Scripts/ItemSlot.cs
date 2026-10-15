using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text quantityText;
    // [SerializeField] private TMP_Text itemQuantityText;
    [SerializeField] private Image itemIconImage;
    public string itemName;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool isFulled;

    public void AddItem(ItemDetails itemDetails)
    {
        itemName = itemDetails.ItemName;
        itemQuantity = itemDetails.Quantity;
        itemIcon = itemDetails.ItemIcon;

        quantityText.text  = itemQuantity.ToString();
        quantityText.enabled = true;
        itemIconImage.sprite = itemIcon;

        isFulled = true;
    }
    
}
