using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BattleManager;
[CreateAssetMenu(menuName = "ScriptableObject/EnemyList")]
public class BattleCharacterStatsList : ScriptableObject
{
    public List<BattleCharacter_StatsList> Data;
    [System.Serializable]
    public class BattleCharacter_StatsList
    {
        public CharaType charaType;
        public string name;
        public Sprite mySprite;

        public int hp = 10;
        public int atk = 5;
        public int agi = 5;
        public int exp = 1;
        public List<int> useSkill;  
        public List<int> useItem;  
    }
}
