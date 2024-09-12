using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleLogSystem : MonoBehaviour
{
    [SerializeField] private GameObject logTextObject;
    private GameObject[] logTextobjList;
    [SerializeField] private int maxLogNum;
    private bool isListMax;

    private int currentLogNum;
    private int logLevel = 0;
    private void Start()
    {
        logTextobjList = new GameObject[maxLogNum];
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LogOutPut("テストログ"+ logLevel);
            logLevel++;
        }
    }
    public void LogOutPut(string logtext)
    {
        for(int q = 0; q < logTextobjList.Length; q++)
        {
            if (q == maxLogNum - 1 && isListMax)
            {
                ElementNextMove();
                CreateLogText(logtext, maxLogNum - 1);
                return;
            }
            if (logTextobjList[q] == null)
            {
                CreateLogText(logtext, q);
                isListMaxCheck(q);
                return;
            }
        }
    }
    private void isListMaxCheck(int q)
    {
        if (q == maxLogNum - 1) isListMax = true;
        else isListMax = false;
    }
    private void CreateLogText(string logtext,int textNum)
    {
        GameObject log = Instantiate(logTextObject, transform);
        log.GetComponent<Text>().text = logtext;
        log.transform.position -= Vector3.up * (maxLogNum * 50);
        log.name = log.name + currentLogNum;
        currentLogNum++;
        logTextobjList[textNum] = log;
    }
    private void ElementNextMove()
    {
        Destroy(logTextobjList[0]);
        logTextobjList[0] = null;
        for (int i = 0; i < maxLogNum; i++)
        {
            if (maxLogNum - 1 != i)
            {
                logTextobjList[i] = logTextobjList[i + 1];
            }
        }
    }
}
