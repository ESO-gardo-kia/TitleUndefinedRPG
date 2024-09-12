using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfomationPanelSystem : MonoBehaviour
{
    [SerializeField] private BattleCharacterStatsList enemyStatsList;
    [SerializeField] private StateList stateList;
    [SerializeField] private Image enemyImage;
    [SerializeField] private Text text;
    [SerializeField] public Slider hpSlider;
    [SerializeField] public List<GameObject> stateImageList;
    [SerializeField] public GameObject stateImageListObj;
    [SerializeField] private GameObject stateImagePrefab;

    public void CurrentHpApply(float currenthp)
    {
        hpSlider.value = currenthp;
    }
    public void PanelInitialization(int enemyNumber)
    {
        var Data = enemyStatsList.Data;
        enemyImage.sprite = Data[enemyNumber].mySprite;
        text.text = Data[enemyNumber].name;
        hpSlider.maxValue = Data[enemyNumber].hp;
        hpSlider.value = Data[enemyNumber].hp;
    }
    public void CurrentStateApply(int stateId, int durationTurn)
    {
        var stateObj = Instantiate(stateImagePrefab, stateImageListObj.transform);
        stateObj.GetComponent<Image>().sprite = stateList.Data[stateId].sprite;
        stateObj.transform.Find("DurationTurn").GetComponent<Text>().text = durationTurn.ToString();
        stateImageList.Add(stateObj);
    }
    public void StateInfomationUpdate(int stateId, int durationTurn)
    {
        stateImageList[stateId].transform.Find("DurationTurn").GetComponent<Text>().text = durationTurn.ToString();
    }
    public void StateAllDelete()
    {
        for (int i = 0; i < stateImageListObj.transform.childCount; i++)
        {
            Destroy(stateImageListObj.transform.GetChild(i).gameObject);
            Debug.Log(stateImageListObj.transform.GetChild(i).gameObject.name + "‚ðíœ‚µ‚Ü‚µ‚½");
        }
    }
}
