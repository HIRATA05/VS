using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //

    //XとYの上限
    //float xLimit = 8.5f;
    //float yLimit = 4.5f;

    public enum BattleSceneMode
    {
        BattleBefor,
        BattleScene,
        End
    }
    public BattleSceneMode battleScene = BattleSceneMode.BattleBefor;

    //戦闘のデータ
    public GameData gameData;

    //プレイヤーのデータ
    public CharaData playerData;

    //対戦相手のデータ
    public CharaData enemyData;

    //キャラの実体
    public GameObject playerChara;
    public GameObject enemyChara;

    //戦闘前演出UI
    [SerializeField] private GameObject BattleBeforUI;

    //戦闘中UI
    [SerializeField] private GameObject BattleUI;
    //名前表示UI
    [SerializeField] private TextMeshProUGUI PlayerNameText;
    [SerializeField] private TextMeshProUGUI EnemyNameText;
    //HP表示UI
    public Slider playerHpSlider;
    public Slider enemyHpSlider;

    //戦闘前演出時間
    private float BattleBeforTime = 3.0f;

    //戦闘終了UI
    [SerializeField] private GameObject BattleFinishUI;

    //勝者敗者
    private GameObject WinerChara;
    private GameObject LoserChara;
    //勝利時の時間
    private float EndTime = 10.0f;
    private float elapsedTime = 0;
    //戦闘終了
    private bool isFinish = false;
    //勝敗表示UI
    [SerializeField] private TextMeshProUGUI JudgeText;
    //勝敗文字
    private string WinText = "Win";
    private string LoseText = "Lose";

    void Start()
    {
        //UIに名前を表示
        PlayerNameText.text = playerData.Name;
        EnemyNameText.text = enemyData.Name;
        //スライダーをHPの数値にする
        playerHpSlider.value = playerData.Hp;
        enemyHpSlider.value = enemyData.Hp;

    }

    void Update()
    {
        //
        if(battleScene == BattleSceneMode.BattleBefor)
        {
            //バトル開始のUIを表示
            if (!BattleBeforUI.activeSelf) 
            {
                BattleBeforUI.SetActive(true);
            }

            //一定時間で戦闘開始
            if (BattleBeforTime < elapsedTime)
            {
                BattleBeforUI.SetActive(false);
                battleScene = BattleSceneMode.BattleScene;
                elapsedTime = 0;

            }
            elapsedTime += Time.deltaTime;
        }
        else if(battleScene == BattleSceneMode.BattleScene)
        {
            //HP表示のUI
            if (!BattleUI.activeSelf)
            {
                BattleUI.SetActive(true);
            }
        }
        else if(battleScene == BattleSceneMode.End)
        {
            if (!isFinish)
            {
                isFinish = true;
                //戦闘終了UIを表示
                BattleFinishUI.SetActive(true);
                //勝者に勝利モーション、敗者に敗北モーション
                WinerChara.GetComponent<Animator>().SetTrigger("Win");
                LoserChara.GetComponent<Animator>().SetTrigger("Lose");

                //勝敗の表示
                if (playerChara == WinerChara)
                {
                    //プレイヤーが勝者の時WINを表示
                    JudgeText.text = WinText;
                }
                else
                {
                    //プレイヤーが敗者の時LOSEを表示
                    JudgeText.text = LoseText;
                }
            }

            //一定時間後にタイトルに戻る
            elapsedTime += Time.deltaTime;
            if (EndTime < elapsedTime)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    //キャラのHPを確認しHPがゼロだったら相手の勝利
    public void WinerCharaCheck(int Hp, GameObject Loser)
    {
        if(Hp <= 0)
        {
            LoserChara = Loser;
            Debug.Log("LoserChara:" + LoserChara + " Loser:" + Loser + " playerChara:" + playerChara + " enemyChara:" + enemyChara);
            if (LoserChara == playerChara)
            {
                WinerChara = enemyChara;
            }
            else
            {
                WinerChara = playerChara;
            }
            battleScene = BattleSceneMode.End;
        }
    }


    //キャラが範囲外に行かないよう制限
    public void CharaMoveLimit(GameObject chara)
    {
        //追加　現在のポジションを保持する
        Vector3 currentPos = chara.transform.position;

        //追加　Mathf.ClampでX,Yの値それぞれが最小～最大の範囲内に収める。
        //範囲を超えていたら範囲内の値を代入する
        currentPos.x = Mathf.Clamp(currentPos.x, -gameData.xLimit, gameData.xLimit);
        currentPos.z = Mathf.Clamp(currentPos.z, -gameData.zLimit, gameData.zLimit);

        //追加　positionをcurrentPosにする
        chara.transform.position = currentPos;
        //Debug.Log(chara + "Mathf.Clamp" + currentPos);
    }

    //ノックバックの方向を計算
    public Vector3 KnockBackDirection(float knockBackPower , bool CharaType)
    {
        Vector3 distination;
        if (CharaType)//TRUEでプレイヤー　FALSEで敵
        {
            // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
            distination = (playerChara.transform.position - enemyChara.transform.position).normalized;
        }
        else
        {
            // 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
            distination = (enemyChara.transform.position - playerChara.transform.position).normalized;
        }
        
        return distination * knockBackPower;
    }
}
