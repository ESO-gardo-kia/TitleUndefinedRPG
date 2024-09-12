using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ItemList itemList;
    [SerializeField]static public List<int> itemInventory;
    void Start()
    {
        InitializeInventory();
        GetItem(0, 8);
        GetItem(1, 3);
        GetItem(2, 5);
    }
    void InitializeInventory()
    {
        itemInventory = new List<int>();
        for (int i = 0; i < itemList.Data.Count; i++)
        {
            itemInventory.Add(0);
        }
    }
    static public void GetItem(int id, int num)
    {
        itemInventory[id] += num;
    }
    static public void UseItem(int id)
    {
        if (isItemPossession(id))
        {
            itemInventory[id]--;
        }
    }
    static public bool isItemPossession(int id)
    {
        if(itemInventory[id] > 0)
        {
            return true;
        }
        return false;
    }
}
