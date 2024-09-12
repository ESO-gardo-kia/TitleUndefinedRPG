using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUiSystem : MonoBehaviour
{
    [SerializeField]SkillList skillList;
    [SerializeField]ItemList itemList;
    [SerializeField]BattleCharacterStatsList enemyStatsList;
    public static bool isWindowOpen;
    public GameObject isActionPanel;
    public GameObject TargetList;
    public GameObject TargetSelectionWindow;
    public GameObject TargetSelectionButtonObject;
    public GameObject skillMenuWindow;
    public GameObject skillDisplayButtonObject;
    public GameObject itemMenuWindow;
    public GameObject itemDisplayButtonObject;
    public void WindowChange(GameObject window)
    {
        Debug.Log(isWindowOpen);
        if (window.activeSelf && isWindowOpen)
        {
            window.SetActive(false);
            isWindowOpen = false;
        }
        else if (!isWindowOpen)
        {
            SkillWindowInitialization();
            ItemWindowInitialization();
            window.SetActive(true);
            isWindowOpen = true;
        }
    }
    public void BackWindow(GameObject window)
    {
        BattleManager.usedSkillId = -1;
        BattleManager.usedItemId = -1;
        window.SetActive(false);
        isWindowOpen = false;
    }
    public void SkillWindowInitialization()
    {
        ObjectChildClaer(skillMenuWindow);
        for (int i = 0; i < skillList.Data.Count; i++)
        {
            GameObject button = Instantiate(skillDisplayButtonObject, skillMenuWindow.transform);
            button.transform.Find("Text").GetComponent<Text>().text = skillList.Data[i].skillName;
            SkillDisplayButtonSystem skillDisplayButtonSystem = button.GetComponent<SkillDisplayButtonSystem>();
            skillDisplayButtonSystem.skillNumber = i;
            skillDisplayButtonSystem.openWindow = TargetSelectionWindow;
            skillDisplayButtonSystem.closeWindow = skillMenuWindow;
        }
    }
    public void ItemWindowInitialization()
    {
        ObjectChildClaer(itemMenuWindow);
        for (int i = 0; i < itemList.Data.Count; i++)
        {
            if (InventoryManager.isItemPossession(i)) 
            {
                GameObject button = Instantiate(itemDisplayButtonObject, itemMenuWindow.transform);
                button.transform.Find("Text").GetComponent<Text>().text = itemList.Data[i].itemName + " èäéùêîÅF" + InventoryManager.itemInventory[i];
                ItemDisplayButtonSystem itemDisplayButtonSystem = button.GetComponent<ItemDisplayButtonSystem>();
                itemDisplayButtonSystem.itemNumber = i;
                itemDisplayButtonSystem.openWindow = TargetSelectionWindow;
                itemDisplayButtonSystem.closeWindow = itemMenuWindow;
            }
        }
    }
    public void TargetSelectionWIndowInitialization(List<RectTransform> CharacterList, BattleManager battleManager)
    {
        ObjectChildClaer(TargetList);
        for (int i = 0; i < CharacterList.Count; i++)
        {
            //Debug.Log(i);
            GameObject button = Instantiate(TargetSelectionButtonObject, TargetList.transform);
            button.transform.Find("Text").GetComponent<Text>().text = enemyStatsList.Data[CharacterList[i].GetComponent<BattleCharacterSystem>().charaId].name;
            button.GetComponent<TargetSelectionButtonSystem>().closeWindow = TargetSelectionWindow;
            button.GetComponent<TargetSelectionButtonSystem>().targetNumber = CharacterList[i].GetComponent<BattleCharacterSystem>().targetId;
            button.GetComponent<TargetSelectionButtonSystem>().battleManager = battleManager;
        }
    }

    private void ObjectChildClaer(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }
    }

    public void PlayerActionDeside(bool isAction)
    {
        isWindowOpen = false;
        if (isAction)
        {
            isActionPanel.SetActive(false);
        }
        else
        {
            isActionPanel.SetActive(true);
        }
    }
}
