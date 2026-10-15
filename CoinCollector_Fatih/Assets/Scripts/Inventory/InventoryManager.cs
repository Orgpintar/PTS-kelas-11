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

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFulled)
            {
                itemSlots[i].AddItem(item);
                break;
            }
            else
            {
                Debug.LogWarning($"ItemSlot at index {i} is null.", this);
            }
        }
    }

    private void CreateInventorySlots()
    {
        ItemSlot slotPrefabs = Resources.Load<ItemSlot>("ItemSlots");

        if (slotPrefabs == null)
        {
            Debug.LogError("ItemSlot prefab not found in Resources folder.");
            itemSlots = new ItemSlot[0]; // Initialize with an empty array to avoid null reference

            return;
        }
        Transform parent = InventoryCanvas.transform;
        GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
        if(grid != null)
        {
            parent = grid.transform;
        }

        itemSlots = new ItemSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            itemSlots[i] = Instantiate(slotPrefabs, parent);
        }
    }
}
