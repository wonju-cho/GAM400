using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot_UI : MonoBehaviour
{
    public List<GameObject> materialSprites;
    public List<GameObject> checkMarks;
    public List<CraftingReceipt> craftingReceipts;

    private InventoryHolder inventoryHolder;
    private StaticInventoryDisplay staticInventoryDisplay;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        inventoryHolder = GameObject.FindWithTag("Player").GetComponent<InventoryHolder>();
        staticInventoryDisplay = GameObject.FindGameObjectWithTag("InventoryDisplay").GetComponent<StaticInventoryDisplay>();
        
        if (!inventoryHolder)
            Debug.Log("There is no inventory holder in the crafting script");

        if (!staticInventoryDisplay)
            Debug.Log("There is no static inventory display in the crafting script");

        for (int i = 0; i < materialSprites.Count; i++)
        {
            materialSprites[i].SetActive(false);
            checkMarks[i].SetActive(false);
        }
    }

    public void EnableSprites()
    {
        for (int i = 0; i < materialSprites.Count; i++)
        {
            materialSprites[i].SetActive(true);
        }
    }

    public void DisableSprites()
    {
        for (int i = 0; i < materialSprites.Count; i++)
        {
            materialSprites[i].SetActive(false);    
        }
    }

    public void CheckUISlots(string receiptName)
    {
        CraftingReceipt cr = craftingReceipts.Find(i => i.resultItem.displayName == receiptName);

        for (int i = 0; i < cr.materials.Count; i++)
        {
            if (inventoryHolder.InventorySystem.IsExistSlot(cr.materials[i].item.displayName))
            {
                InventorySlot inventorySlot = inventoryHolder.InventorySystem.GetInventorySlot(cr.materials[i].item.displayName);
                if (inventorySlot.StackSize >= cr.materials[i].amount)
                {
                    checkMarks[i].SetActive(true);
                }
                else
                {
                    checkMarks[i].SetActive(false);
                }
            }
            else
            {
                checkMarks[i].SetActive(false);
            }
        }
    }
}
