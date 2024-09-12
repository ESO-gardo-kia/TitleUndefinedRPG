using System;
using System.Collections.Generic;
using UnityEngine;
using static BattleManager;
using static SkillList;

public class BattleCharacterSystem : MonoBehaviour
{
    [SerializeField] BattleCharacterStatsList battleCharacterStatsList;
    [SerializeField] public BattleManager battleManager;
    [SerializeField] public EnemyInfomationPanelSystem enemyInfomationPanelSystem;
    [SerializeField] private SkillList skillList;
    [SerializeField] private ItemList itemList;
    [SerializeField] private StateList stateList;
    public Action<string, string, int, Transform, int> takeDamageEvent;
    public Action<int, Vector3> takeStateEvent;
    public CharaType charaType;
    public bool isDeath;

    public int targetId;
    public int charaId;
    public string myname;
    public int hp;
    public int currentHp;
    public int agi;
    public int currentAgi;
    public List<int> StateIdList;
    public List<int> stateDurationTurnList;
    private bool isSameState;
    private int sameStateId;

    private string usedChara;
    private int usedSkillId;
    private int usedItemId;
    private int takeDamageNum;
    void Start()
    {
        isDeath = false;
    }
    public void TakeSkill(string usedChara, int skillId)
    {
        this.usedChara = usedChara;
        usedSkillId = skillId;
        Debug.Log("スキルID：" + skillId);
        if (skillList.Data[skillId].giveStateInfomation != null)
        {
            for (int i = 0; i < skillList.Data[skillId].giveStateInfomation.Count; i++)
            {
                TakeState(skillList.Data[skillId].giveStateInfomation[i].durationTurn
                    , skillList.Data[skillId].giveStateInfomation[i].giveStateId, i);
            }
        }

        TakeDamage(skillList.Data[skillId].damage);
    }
    public void TakeItem(string usedChara, int itemId)
    {
        this.usedChara = usedChara;
        usedItemId = itemId;
        Debug.Log("アイテムID：" + itemId);
        if (skillList.Data[itemId].giveStateInfomation != null)
        {
            for (int i = 0; i < itemList.Data[itemId].giveStateInfomation.Count; i++)
            {
                TakeState(itemList.Data[itemId].giveStateInfomation[i].durationTurn
                    , itemList.Data[itemId].giveStateInfomation[i].giveStateId, i);
            }
        }
        TakeDamage(itemList.Data[itemId].life);
    }
    private void TakeState(int durationTurn, int giveStateId, int i)
    {
        SameStateCheck(i);
        if (isSameState)
        {
            stateDurationTurnList[sameStateId] = durationTurn;
            enemyInfomationPanelSystem.StateInfomationUpdate(i, stateDurationTurnList[sameStateId]);
        }
        else
        {
            StateIdList.Add(giveStateId);
            stateDurationTurnList.Add(durationTurn);
            enemyInfomationPanelSystem.CurrentStateApply(giveStateId, durationTurn);
        }
    }
    private void TakeDamage(int damage)
    {
        takeDamageNum = damage;
        if (damage > 0)
        {
            for (int i = 0; i < StateIdList.Count; i++)
            {
                if (stateList.Data[StateIdList[i]].takeDamageTiming && stateList.InvincibleAction(StateIdList[i]))
                {
                    takeDamageNum = 0;
                }
            }
        }
        else if (damage < 0)
        {
            if (hp < -damage)
            {
                takeDamageNum = hp;
                takeDamageNum = takeDamageNum * -1 + currentHp;
            }
        }
        currentHp -= takeDamageNum;
        enemyInfomationPanelSystem.hpSlider.value = currentHp;
        if (currentHp <= 0) DeathProcessing();

        takeDamageEvent(usedChara, myname,usedSkillId, transform, takeDamageNum);
    }
    private void SameStateCheck(int StateId)
    {
        isSameState = false;
        for (int j = 0; j < StateIdList.Count; j++)
        {
            if (StateIdList[j] == StateId)
            {
                isSameState = true;
                sameStateId = j;
            }
        }
    }
    public void EveryTurnProcessing()
    {
        EveryStateTurn();
    }
    private void EveryStateTurn()
    {
        for (int i = 0; i < StateIdList.Count; i++)
        {
            if (stateList.Data[StateIdList[i]].everyTrunTiming)
            {
                TakeDamage(stateList.EveryTrunAction(StateIdList[i]));
            }
        }
        for (int i = 0; i < StateIdList.Count; i++)
        {
            stateDurationTurnList[i]--;
            enemyInfomationPanelSystem.StateInfomationUpdate(i, stateDurationTurnList[i]);
            if (stateDurationTurnList[i] > 0) continue;
            stateDurationTurnList.Remove(i);
            StateIdList.Remove(i);
        }
    }
    private void DeathProcessing()
    {
        currentHp = 0;
        isDeath = true;
        if (charaType == CharaType.player)
        {
            battleManager.PlayerDeathProcessing();
        }
        else if (charaType == CharaType.enemy)
        {
            battleManager.EnemyDeathProcessing(gameObject);
        }
    }
    public void StatsInitialization(int id)
    {
        charaId = id;
        var Data = battleCharacterStatsList.Data;
        charaType = Data[charaId].charaType;
        myname = Data[charaId].name;
        hp = Data[charaId].hp;
        currentHp = Data[charaId].hp;
        agi = Data[charaId].agi;
        currentAgi = Data[charaId].agi;
    }
}
