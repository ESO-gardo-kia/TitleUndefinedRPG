using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "ScriptableObject/SkillList")]
public class SkillList : ScriptableObject
{
    public List<Skill_List> Data;
    [System.Serializable]
    public class Skill_List
    {
        public string skillName;
        public int damage;
        public List<StateInfomation> giveStateInfomation;
        public List<StateInfomation> getStateInfomation;
        [System.Serializable]
        public class StateInfomation
        {
            public int giveStateId;
            public int durationTurn;
        }
    }
}
