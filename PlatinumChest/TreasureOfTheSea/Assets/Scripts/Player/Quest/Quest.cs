using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Quest : MonoBehaviour
{    
    public bool completed;

    public List<QuestGoal> Goals;
    public List<QuestSlot_UI> questSlots;
    public GameObject middle;
    public GameObject questUI;

    [Header("Fog Settings For Bridge")]
    public GameObject fogEffect;
    public Transform fogTranslation;

    private GameObject fogObject;

    private InventoryHolder inventoryHolder;
    private StaticInventoryDisplay staticInventoryDisplay;
    private InventorySlot_UI[] UISlots;

    public List<GameObject> questScriptTextUI;
    private GameObject[] checkScriptTexts;
    QuestScript_UI questScriptUI;

    public void Start()
    {
        Initialize();
        middle.SetActive(false);
        questUI.SetActive(false);

        fogObject = Instantiate(fogEffect, fogTranslation.position, Quaternion.identity);

        inventoryHolder = GameObject.FindWithTag("Player").GetComponent<InventoryHolder>();
        staticInventoryDisplay = GameObject.FindWithTag("InventoryDisplay").GetComponent<StaticInventoryDisplay>();
        UISlots = staticInventoryDisplay.GetAllSlots();
        questScriptUI = GameObject.FindWithTag("QuestScriptUI").GetComponent<QuestScript_UI>();

        if (questScriptUI)
            Debug.Log("There is no quest script UI in the quets trigger.cs");

        if (!inventoryHolder)
            Debug.Log("There is no inventoryHolder in the quest script");

        if (!staticInventoryDisplay)
            Debug.Log("There is no static inventory display in the quest script");

        if (UISlots.Length < 1)
            Debug.Log("UI slots are not initialized in the quest script");

        if (!questScriptUI)
            Debug.Log("There is no quest script UI script in the quets script");
    }

    private void Update()
    {
        if(questScriptUI.GetIsQuestUIOn() && !completed)
        {
            Evaulate();
        }
    }

    public void Initialize()
    {
        completed = false;
        for (int i = 0; i < Goals.Count; i++)
        {
            Goals[i].Initialize();
        }

        for(int i = 0; i < Goals.Count; i++)
        {
            questSlots[i].SetSprite(Goals[i].icon);
            questSlots[i].SetTMP(Goals[i].requiredAmount.ToString());
        }
    }

    protected void Evaulate()
    {
        bool isEmptyInventory = true;
        checkScriptTexts = questScriptUI.GetAllQuestScripts();

        for (int i = 0; i < Goals.Count; i++)
        {
            Goals[i].called = true;
            if (inventoryHolder.InventorySystem.IsExistSlot(Goals[i].requiredName))
            {
                isEmptyInventory = false;
                InventorySlot test = inventoryHolder.InventorySystem.GetInventorySlot(Goals[i].requiredName);
                Goals[i].currentAmount = test.StackSize;
                
                int check = Goals[i].currentAmount;

                if (check >= Goals[i].requiredAmount)
                {
                    questSlots[i].EnableCheckImage();
                    Goals[i].Complete();

                    if(checkScriptTexts[0] != null)
                    {
                        GameObject completedScript = Array.Find(checkScriptTexts, arr => arr.GetComponent<TextMeshProUGUI>() != null && arr.GetComponent<TextMeshProUGUI>().text == Goals[i].questDescription);
                        completedScript.GetComponentInChildren<Image>().enabled = true;
                    }
                }
                else
                {
                    questSlots[i].DisableCheckImage();
                }

                if(!questScriptUI.GetIsQuestUIOn())
                {
                    questUI.SetActive(true);
                }
            }
            else
            {
                questSlots[i].DisableCheckImage();
            }
        }

        if(isEmptyInventory && !questScriptUI.GetIsQuestUIOn())
            questUI.SetActive(true);
    }


    public bool CheckGoals()
    {
        Evaulate();
        completed = Goals.All(g => g.completed);
        
        if(completed)
        {
            for(int i = 0; i < Goals.Count; i++)
            {
                InventorySlot test = inventoryHolder.InventorySystem.GetInventorySlot(Goals[i].requiredName);
                test.RemoveFromStack(Goals[i].requiredAmount);
            }

            for (int i = 0; i < UISlots.Length; i++)
            {
                UISlots[i].UpdateUISlot();                
            }

            middle.SetActive(true);
            Destroy(questUI);
            Destroy(fogObject);

            return true;
        }
        return false;
    }
}
