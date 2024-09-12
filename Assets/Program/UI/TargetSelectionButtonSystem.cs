using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelectionButtonSystem : MonoBehaviour
{
    public GameObject closeWindow;
    public int targetNumber;
    public BattleManager battleManager;
    public void SetTargetId()
    {
        closeWindow.SetActive(false);
        BattleManager.targetId = targetNumber;
        battleManager.ActionStart();
    }
}
