using UnityEngine;

public class ItemDisplayButtonSystem : MonoBehaviour
{
    public GameObject openWindow;
    public GameObject closeWindow;
    public int itemNumber;
    public void Window_Change()
    {
        SetItemId();
        closeWindow.SetActive(false);
        openWindow.SetActive(true);
    }
    public void SetItemId()
    {
        BattleManager.usedItemId = itemNumber;
    }
}
