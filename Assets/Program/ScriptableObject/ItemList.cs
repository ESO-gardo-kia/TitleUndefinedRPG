using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/ItemList")]
public class ItemList : ScriptableObject
{
    public List<Item_List> Data;
    [System.Serializable]
    public class Item_List
    {
        public string itemName;
        [Tooltip("�g�p�����L�����ɗ^���鐔�l")]
        public int life;
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
