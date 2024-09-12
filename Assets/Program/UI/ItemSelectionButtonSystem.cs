using UnityEngine;

public class ItemSelectionButtonSystem : MonoBehaviour
{
    public int itemNumber;
    public void SetItemId()
    {
        BattleManager.usedSkillId = itemNumber;
    }
}
