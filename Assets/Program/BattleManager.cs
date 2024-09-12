using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;


public class BattleManager : MonoBehaviour
{
    [Tooltip("コンポーネント")]
    public enum CharaType
    {
        player,
        enemy
    }
    private static bool isPlayerCommandPermit;
    [SerializeField]private BattleCharacterStatsList battleCharacterStatsList;
    [SerializeField]private SkillList skillList;
    [SerializeField]private ItemList itemList;
    [SerializeField]private InventoryManager inventoryManager;
    [SerializeField]private BattleLogSystem battleLogSystem;
    [SerializeField]private CommandUiSystem commandUiSystem;
    [SerializeField]private EffectTextManager effectTextManager;

    [SerializeField]private Transform enemyImfomationPanelList;
    [SerializeField]private Transform playerImfomationPanelList;
    [SerializeField]private GameObject enemyImfomationPanel;

    public List<RectTransform> playerList;
    public List<RectTransform> alivePlayerList;
    public List<RectTransform> enemyList;
    public List<RectTransform> sequentialOrderList;
    public int enemySpawnNumber;
    public int playerSpawnNumber;
    public float actionStandbyTime;
    public int usedCharaId = 0;

    static public int targetId = -1;
    static public int usedSkillId = -1;
    static public int usedItemId = -1;

    
    private void Start()
    {
        BattleStartFunction();
    }
    private void BattleStartFunction()
    {
        SelectTypeCharaAllDestroy(enemyImfomationPanelList);
        SelectTypeCharaAllDestroy(playerImfomationPanelList);
        EnemyInfomationCreation();
        PlayerInfomationCreation();
        EveryTurnProcessing();
        StartCoroutine(CharacterActionPostProcessing());
    }
    /// <summary>
    /// 毎ターン処理
    /// </summary>
    private void EveryTurnProcessing()
    {
        battleLogSystem.LogOutPut("ターン経過");
        usedCharaId = 0;
        GetAllBattleCharacter(sequentialOrderList);
        SequentialSort();
        TargetIdRationing();

        for (int i = 0; i < sequentialOrderList.Count; i++)
        {
            sequentialOrderList[i].GetComponent<BattleCharacterSystem>().EveryTurnProcessing();
        }
    }
    /// <summary>
    /// 行動したその後の処理
    /// </summary>
    private void EveryActProcessing()
    {
        commandUiSystem.SkillWindowInitialization();
        commandUiSystem.ItemWindowInitialization();
        commandUiSystem.TargetSelectionWIndowInitialization(sequentialOrderList, this);
    }
    /// <summary>
    /// ダメージの表示
    /// </summary>
    public void GiveSkillInstruction(string usedChara, string targetName, int skillId, Transform target, int damage)
    {
        if(damage > 0)
        {
            battleLogSystem.LogOutPut(usedChara + "が" + targetName + "に" + damage + "ダメージ");
        }
        else if(damage < 0)
        {
            battleLogSystem.LogOutPut(usedChara + "は" + targetName + "を" + damage + "回復させた");
        }
        Standby();
        effectTextManager.SpawnNumText(damage, target.transform.position);
    }
    /// <summary>
    /// アイテム使用処理
    /// </summary>
    public void GiveItemInstruction(string usedChara, string targetName, int itemId, Transform target, int damage)
    {
        battleLogSystem.LogOutPut(usedChara + "が" + targetName + "に" + itemList.Data[itemId].itemName + "を使用した");
        if (damage > 0)
        {
            battleLogSystem.LogOutPut(targetName + "に" + damage + "のダメージ");
        }
        else if (damage < 0)
        {
            battleLogSystem.LogOutPut(targetName + "は" + damage + "回復した");
        }
        InventoryManager.UseItem(itemId);
        effectTextManager.SpawnNumText(itemList.Data[itemId].life, target.transform.position);
        Standby();
        target.GetComponent<EnemyInfomationPanelSystem>().hpSlider.value -= damage;
        //target.GetComponent<BattleCharacterSystem>().TakeSkill(itemId);
    }
    public void GiveStateInstruction(int stateId, Transform target)
    {
        effectTextManager.SpawnStateText(stateId, target.transform.position);
    }
    /// <summary>
    /// currentActionCharaIdに指定されているキャラクターに行動許可を出す
    /// </summary>
    private void CharacterActionDecide()
    {
        if (sequentialOrderList.Count == usedCharaId) EveryTurnProcessing();
        if (sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().charaType == CharaType.player)
        {
            PlayerActionSetup();
        }
        else if (sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().charaType == CharaType.enemy)
        {
            EnemyActionSetup();
        }
    }
    private void EnemyActionSetup()
    {
        var Data = battleCharacterStatsList.Data[sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().charaId];
        battleLogSystem.LogOutPut(sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().myname + "の行動開始" + usedCharaId);
        isPlayerCommandPermit = false;
        commandUiSystem.PlayerActionDeside(false);
        AlivePlayerSerch();
        targetId = alivePlayerList[Random.Range(0, alivePlayerList.Count)].GetComponent<BattleCharacterSystem>().targetId;
        usedSkillId = Data.useSkill[Random.Range(0, Data.useSkill.Count)];
        ActionStart();
    }
    private void PlayerActionSetup()
    {
        if(IfDeathProcessing()) return;
        battleLogSystem.LogOutPut(sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().myname + "の行動開始" + usedCharaId);
        isPlayerCommandPermit = true;
        commandUiSystem.PlayerActionDeside(true);
    }
    public void ActionStart()
    {
        if (isPlayerCommandPermit) isPlayerCommandPermit = false;
        if (usedSkillId > -1)
        {
            sequentialOrderList[targetId].GetComponent<BattleCharacterSystem>().
                TakeSkill(sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().myname, usedSkillId);
        }
        else if (usedItemId > -1)
        {
            sequentialOrderList[targetId].GetComponent<BattleCharacterSystem>().
                TakeItem(sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().myname, usedItemId);
        }
        targetId = -1;
        usedSkillId = -1;
        usedItemId = -1;
        StartCoroutine(CharacterActionPostProcessing());
    }
    /// <summary>
    /// 死んでいるか確認
    /// </summary>
    private bool IfDeathProcessing()
    {
        if (sequentialOrderList[usedCharaId].GetComponent<BattleCharacterSystem>().isDeath == true)
        {
            usedCharaId++;
            CharacterActionDecide();
            return true;
        }
        else return false;
    }
    /// <summary>
    /// プレイヤーが行動する際に呼び出される関数
    /// </summary>
    private void AlivePlayerSerch()
    {
        alivePlayerList.Clear();
        foreach (RectTransform list in playerList)
        {
            if (list.GetComponent<BattleCharacterSystem>().isDeath == false) alivePlayerList.Add(list);
        }
    }
    /// <summary>
    /// 行動終了後の処理
    /// </summary>
    private IEnumerator CharacterActionPostProcessing()
    {
        usedCharaId++;
        yield return new WaitForSeconds(actionStandbyTime);
        if (IsBattleEnd()) BattleEndProcessing();
        else
        {
            EveryActProcessing();
            CharacterActionDecide();
        }
    }
    private IEnumerator Standby()
    {
        yield return new WaitForSeconds(actionStandbyTime);
    }
    /// <summary>
    /// 指定した数だけ敵を生成する
    /// </summary>
    private void EnemyInfomationCreation()
    {
        for (int i = 0; i < enemySpawnNumber; i++)
        {
            GameObject panel = Instantiate(enemyImfomationPanel, enemyImfomationPanelList);
            enemyList.Add((RectTransform)panel.transform);
            panel.GetComponent<BattleCharacterSystem>().StatsInitialization(UnityEngine.Random.Range(1, 4));
            panel.GetComponent<BattleCharacterSystem>().battleManager = this;
            panel.GetComponent<BattleCharacterSystem>().takeDamageEvent = GiveSkillInstruction;
            panel.GetComponent<EnemyInfomationPanelSystem>().PanelInitialization(enemyList[i].GetComponent<BattleCharacterSystem>().charaId);
        }
    }
    /// <summary>
    /// 指定した数だけプレイヤーを生成する
    /// </summary>
    private void PlayerInfomationCreation()
    {
        for (int i = 0; i < playerSpawnNumber; i++)
        {
            GameObject panel = Instantiate(enemyImfomationPanel, playerImfomationPanelList);
            playerList.Add((RectTransform)panel.transform);
            panel.GetComponent<BattleCharacterSystem>().StatsInitialization(0);
            panel.GetComponent<BattleCharacterSystem>().battleManager = this;
            panel.GetComponent<BattleCharacterSystem>().takeDamageEvent = GiveSkillInstruction;
            panel.GetComponent<EnemyInfomationPanelSystem>().PanelInitialization(playerList[i].GetComponent<BattleCharacterSystem>().charaId);
        }
    }
    /// <summary>
    /// 指定したオブジェクトの子を全て削除する
    /// </summary>
    private void SelectTypeCharaAllDestroy(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
            Debug.Log(transform.GetChild(i).gameObject.name + "を削除しました");
        }
    }
    /// <summary>
    /// 敵が死んだ際の処理
    /// </summary>
    /// <param name="my">削除するobject</param>
    public void EnemyDeathProcessing(GameObject my)
    {
        enemyList.Remove((RectTransform)my.transform);
        sequentialOrderList.Remove((RectTransform)my.transform);
        Destroy(my);
        GetAllBattleCharacter(sequentialOrderList);
        SequentialSort();
        TargetIdRationing();
    }
    public void PlayerDeathProcessing()
    {
        battleLogSystem.LogOutPut("プレイヤー死亡");
    }
    /// <summary>
    /// 戦闘が終了したか否か
    /// </summary>
    private bool IsBattleEnd()
    {
        int deathCount = 0;
        for (int i = 0;i < playerList.Count; i++)
        {
            if (playerList[i].GetComponent<BattleCharacterSystem>().isDeath == true) deathCount++;
            if(deathCount == playerList.Count) return true;
        }
        if (enemyList.Count <= 0) return true;
        return false;
    }
    /// <summary>
    /// プレイヤーが勝利したか否か
    /// </summary>
    private bool IsPlayerWin()
    {
        if (enemyList.Count <= 0) return true;
        else return false;
    }
    /// <summary>
    /// 戦闘終了時の処理
    /// </summary>
    private void BattleEndProcessing()
    {
        if (IsPlayerWin())
        {
            battleLogSystem.LogOutPut("勝利");
        }
        else
        {
            battleLogSystem.LogOutPut("敗北");
        }
    }
    /// <summary>
    /// 戦闘中のキャラクターを全て取得
    /// </summary>
    private void GetAllBattleCharacter(List<RectTransform> list)
    {
        list.Clear();
        foreach (RectTransform t in playerList)
        {
            list.Add(t);
        }
        foreach (RectTransform t in enemyList)
        {
            list.Add(t);
        }
    }
    /// <summary>
    /// 素早さを基準に行動順を並び変え
    /// </summary>
    private void SequentialSort()
    {
        for (int i = 0; i < sequentialOrderList.Count; i++)
        {
            for (int q = i + 1; q < sequentialOrderList.Count; q++)
            {
                if (sequentialOrderList[i].GetComponent<BattleCharacterSystem>().currentAgi <
                    sequentialOrderList[q].GetComponent<BattleCharacterSystem>().currentAgi)
                {
                    var cache = sequentialOrderList[i];
                    sequentialOrderList[i] = sequentialOrderList[q];
                    sequentialOrderList[q] = cache;
                }
            }
        }
    }
    /// <summary>
    /// TargetIdを各キャラクターに割り振り
    /// </summary>
    private void TargetIdRationing()
    {
        for (int i = 0; i < sequentialOrderList.Count; i++)
        {
            sequentialOrderList[i].GetComponent<BattleCharacterSystem>().targetId = i;
        }
    }
}
