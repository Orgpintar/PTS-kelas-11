using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    private bool InventoryActive;
    [SerializeField] private int slotCount = 16;
    private ItemSlot[] itemSlots;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryCanvas.SetActive(false);
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        InventoryActive = false;

        CreateInventorySlots();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (inventoryAction.triggered && InventoryCanvas != null && InventoryActive == false)
        {
            InventoryActive = true;
            Time.timeScale = 0f;
            Debug.Log("Inventory is active");
            InventoryCanvas.SetActive(true);
        } else if (inventoryAction.triggered && InventoryCanvas != null && InventoryActive == true)
        {
            InventoryActive = false;
            Time.timeScale = 1f;
            Debug.Log("Inventory is deactivated");
            InventoryCanvas.SetActive(false);
        }
    }
    public void AddItem(ItemDetails item)
    {
        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);
        // Add item to inventory logic here

        if (itemSlots == null || itemSlots.Length == 0)
        {
            Debug.LogError("Item slots are not initialized.", this);
            return;
        }

        // First, try to stack the item onto a slot that already holds the same item
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].CanStackWith(item.ItemName))
            {
                itemSlots[i].AddQuantity(item.Quantity);
                return;
            }
        }

        // Otherwise, put the item into the first empty slot
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFulled)
            {
                itemSlots[i].AddItem(item);
                return;
            }
        }
    }

    private void CreateInventorySlots()
    {
        ItemSlot slotPrefabs = Resources.Load<ItemSlot>("ItemSlots");

        if (slotPrefabs == null)
        {
            Debug.LogError("ItemSlot prefab was not found in a Resources folder.", this);
            itemSlots = new ItemSlot[0];
            return;
        }
        // Put the slots inside the GridLayoutGroup container (InventorySlots), if there is one
        Transform parent = InventoryCanvas.transform;
        GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
        if (grid != null)
        {
            parent = grid.transform;
        }

        itemSlots = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = Instantiate(slotPrefabs, parent);
        }

        
    }
}
