using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectTextManager : MonoBehaviour
{
    [SerializeField] private StateList stateList;
    [SerializeField] private GameObject spawnTextObject;
    private Color currentSetColor;
    [SerializeField] private Color damageTextColor;
    [SerializeField] private Color recoveryTextColor;
    [SerializeField] private Color noughtTextColor;
    public void SpawnNumText(int setNum, Vector3 SetPosition)
    {
        setNum = DamageColorSet(setNum);

        var Text = Instantiate(spawnTextObject, SetPosition, Quaternion.identity, transform);
        Text.GetComponent<EffectTextObject>().SetTextInfomation(setNum.ToString(), currentSetColor);
    }
    public void SpawnStateText(int stateId,Vector3 SetPosition)
    {
        var Text = Instantiate(spawnTextObject, SetPosition, Quaternion.identity, transform);
        Text.GetComponent<EffectTextObject>().SetTextInfomation(stateList.Data[stateId].effectText, stateList.Data[stateId].effectTextColor);
    }
    private int DamageColorSet(int setNum)
    {
        if (setNum > 0)
        {
            currentSetColor = damageTextColor;
        }
        else if (setNum < 0)
        {
            currentSetColor = recoveryTextColor;
            setNum *= -1;
        }
        else
        {
            currentSetColor = noughtTextColor;
        }
        return setNum;
    }
}
