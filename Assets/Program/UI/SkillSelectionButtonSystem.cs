using UnityEngine;

public class SkillSelectionButtonSystem : MonoBehaviour
{
    public int skillNumber;
    public void SetSkillId()
    {
        BattleManager.usedSkillId = skillNumber;
    }
}
