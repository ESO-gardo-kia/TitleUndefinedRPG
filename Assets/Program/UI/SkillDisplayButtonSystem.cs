using UnityEngine;

public class SkillDisplayButtonSystem : MonoBehaviour
{
    public GameObject openWindow;
    public GameObject closeWindow;
    public int skillNumber;
    public void Window_Change()
    {
        SetSkillId();
        Debug.Log("‹N“®");
        closeWindow.SetActive(false);
        openWindow.SetActive(true);
    }
    public void SetSkillId()
    {
        BattleManager.usedSkillId = skillNumber;
    }
}
