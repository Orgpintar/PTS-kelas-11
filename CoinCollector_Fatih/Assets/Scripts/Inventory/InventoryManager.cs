using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private int slotCount = 16;
    [SerializeField] private ItemDetailsPanel detailsPanel;
    private bool InventoryActive;
    public ItemSlot[] itemSlot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateSlots();
        InventoryCanvas.SetActive(false);
        inventoryAction = InputSystem.actions.FindAction("Inventory");
        InventoryActive = false;

        if (detailsPanel == null)
        {
            Debug.LogWarning("No ItemDetailsPanel is assigned, item details will not be shown.", this);
        }
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
            ShowFirstItem();
        } else if (inventoryAction.triggered && InventoryCanvas != null && InventoryActive == true)
        {
            InventoryActive = false;
            Time.timeScale = 1f;
            Debug.Log("Inventory is deactivated");
            InventoryCanvas.SetActive(false);
        }
    }

    // Build the slot objects at runtime so no manual wiring is needed in the scene
    private void CreateSlots()
    {
        ItemSlot slotPrefab = Resources.Load<ItemSlot>("ItemSlots");
        if (slotPrefab == null)
        {
            Debug.LogError("ItemSlot prefab was not found in a Resources folder.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        // Put the slots inside the GridLayoutGroup container (InventorySlots), if there is one
        Transform parent = InventoryCanvas.transform;
        GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
        if (grid != null)
        {
            parent = grid.transform;
        }

        itemSlot = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(slotPrefab, parent);
        }
    }

    public void AddItem(ItemDetails item)
    {
        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

        if (itemSlot == null || itemSlot.Length == 0)
        {
            Debug.LogWarning("Inventory has no slots, '" + item.ItemName + "' was lost.");
            return;
        }

        // Logic to check if item already exists in the inventory and can be stacked
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].CanStackWith(item.ItemName))
            {
                itemSlot[i].AddQuantity(item.Quantity);
                return;
            }
        }

        // Logic to add the item to an empty slot if it cannot be stacked
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].IsEmpty)
            {
                itemSlot[i].AddItem(item);
                return;
            }
        }

        Debug.LogWarning("Inventory is full, '" + item.ItemName + "' was lost.");
    }

    // Called by a slot when the player clicks on it
    public void SelectSlot(ItemSlot slot)
    {
        DesselectAllSlot();

        // Show the selection highlight on the clicked slot
        slot.selectedPanel.SetActive(true);
        slot.isItemSelected = true;

        // Show the item details on the right-side panel
        if (detailsPanel != null)
        {
            detailsPanel.Show(slot.ItemDetails, slot.itemQuantity);
        }
    }

    // When the inventory opens, show the first item on the details panel
    private void ShowFirstItem()
    {
        if (itemSlot != null)
        {
            foreach (ItemSlot slot in itemSlot)
            {
                if (!slot.IsEmpty)
                {
                    SelectSlot(slot);
                    return;
                }
            }
        }

        // No items yet: leave the panel empty
        if (detailsPanel != null)
        {
            detailsPanel.Clear();
        }
    }

    public void DesselectAllSlot()
    {
        foreach (ItemSlot slot in itemSlot)
        {
            slot.isItemSelected = false;
            slot.selectedPanel.SetActive(false);
        }
    }
}
