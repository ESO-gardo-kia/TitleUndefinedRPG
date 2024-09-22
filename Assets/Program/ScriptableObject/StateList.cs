using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/StateList")]
public class StateList : ScriptableObject
{
    public List<State_List> Data;
    [System.Serializable]
    public class State_List
    {
        [Header("UI�֘A")]
        public string Name;
        public Sprite sprite;
        public string effectText;
        public Color effectTextColor;
        [Header("���ʓ��e")]
        public int takeLife;
        public bool invincible;
        public int triggerProbability;
        [Header("���ʂ̔����^�C�~���O")]
        public bool takeDamageTiming;
        public bool useSkillTimig;
        public bool useItemTimig;
        public bool everyTrunTiming;
        public bool hullTankTiming;


    }
    private bool TriggerCheck(int stateId)
    {
        if (Data[stateId].triggerProbability <= UnityEngine.Random.Range(0, 100))
        {
            return true;
        }
        return false;
    }
    public bool InvincibleAction(int stateId)
    {
        return TriggerCheck(stateId);
    }
    public int EveryTrunAction(int stateId)
    {
        if(!TriggerCheck(stateId))return 0;
        return Data[stateId].takeLife;
    }
}