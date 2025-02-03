using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    //

    //ゲームマネージャー
    GameManager gameManager;
    //ゲームマネージャーから貰ったプレイヤーのデータ
    CharaData charaData;

    private Animator animator;

    private Rigidbody rb;

    int Hp, Atk;

    //向き
    Vector3 RotRight = new Vector3(180, 0, 0);
    Vector3 RotLeft = new Vector3(-180, 0, 0);
    //着地状態
    private bool isGround = false;
    private string groundTag = "Ground";
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //攻撃距離
    float AttackDistance = 1;

    //一定時間ごとに行動を変化
    enum EnemyState
    {
        Move,
        Attack,
        Escape
    }
    EnemyState enemyState = EnemyState.Move;

    float StateMoveTime = 5;
    float StateEscapeTime = 1.5f;
    float elapsedTime = 0;

    void Start()
    {
        //ゲームマネージャーから敵のデータを貰う
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        charaData = gameManager.enemyData;

        //プレイヤーキャラのアニメーターを設定
        animator = GetComponent<Animator>();

        //Rigidbodyを取得
        rb = GetComponent<Rigidbody>();

        //キャラの情報を取得
        Hp = charaData.Hp;
        Atk = charaData.Atk;
    }

    void Update()
    {
        if(gameManager.battleScene == GameManager.BattleSceneMode.BattleScene)
        {
            //ステートによって行動が変化
            if(enemyState == EnemyState.Move)
            {
                Vector3 dis = (gameManager.playerChara.transform.position - gameManager.enemyChara.transform.position).normalized;
                Debug.Log("dis " + dis);
                
                if (dis.x > 0)
                {
                    //キャラの向きを変える
                    transform.LookAt(RotRight);
                }
                else if (dis.x < 0)
                {
                    //キャラの向きを変える
                    transform.LookAt(RotLeft);
                }

                transform.position += new Vector3(dis.x, 0, 0) * charaData.MoveSpeed * Time.deltaTime;
                //移動アニメーション発生
                animator.SetBool("Move", true);

                //Debug.Log("elapsedTime " + elapsedTime);
                //一定の近さの場合攻撃モーション発生
                if (Vector3.Distance(gameManager.playerChara.transform.position, gameManager.enemyChara.transform.position) < AttackDistance)
                {
                    //他のステートに移行
                    enemyState = EnemyState.Attack;
                }

                elapsedTime += Time.deltaTime;
                if (StateMoveTime < elapsedTime)
                {
                    elapsedTime = 0;
                    //他のステートに移行
                    enemyState = EnemyState.Escape;
                }
            }
            else if(enemyState == EnemyState.Attack)
            {
                //移動アニメーション停止
                animator.SetBool("Move", false);
                
                //攻撃モーション発生
                animator.SetTrigger("Attack");

                //攻撃後Escapeに変化
                enemyState = EnemyState.Escape;

            }
            else if (enemyState == EnemyState.Escape)
            {
                Vector3 dis = (gameManager.enemyChara.transform.position - gameManager.playerChara.transform.position).normalized;
                Debug.Log("dis " + dis);

                if (dis.x > 0)
                {
                    //キャラの向きを変える
                    transform.LookAt(RotLeft);
                }
                else if (dis.x < 0)
                {
                    //キャラの向きを変える
                    transform.LookAt(RotRight );
                }

                transform.position += new Vector3(dis.x, 0, 0) * charaData.MoveSpeed * Time.deltaTime;
                //移動アニメーション発生
                animator.SetBool("Move", true);

                //Debug.Log("elapsedTime " + elapsedTime);
                elapsedTime += Time.deltaTime;
                if (StateEscapeTime < elapsedTime)
                {
                    elapsedTime = 0;
                    if (IsGround())
                    {
                        //ジャンプアニメーション発生
                        animator.SetBool("IsJamp", true);
                        rb.AddForce(Vector3.up * charaData.JampPower, ForceMode.Impulse);
                    }
                    //一定の近さの場合攻撃モーション発生
                    if (Vector3.Distance(gameManager.playerChara.transform.position, gameManager.enemyChara.transform.position) < AttackDistance)
                    {
                        //他のステートに移行
                        enemyState = EnemyState.Attack;
                    }

                    //他のステートに移行
                    enemyState = EnemyState.Move;
                }
            }
            //Debug.Log("enemyState "+ enemyState);

            //キャラが範囲外に行かないよう制限
            gameManager.CharaMoveLimit(gameObject);

            //入力でアニメーションを発生
            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("敵のN_M");
                //攻撃アニメーション発生
                animator.SetTrigger("Attack");
            }
        }
    }

    //接地判定を返す
    public bool IsGround()
    {
        if (isGroundEnter)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        //isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("OnTriggerEnter " + collision.tag);
        if (collision.tag == groundTag)
        {
            //ジャンプアニメーション終了
            animator.SetBool("IsJamp", false);

            isGroundEnter = true;
        }

        //攻撃の当たり判定
        //当たったものが対戦相手か確認
        if (collision.CompareTag("PlayerAttack"))
        {
            //ゲームマネージャーから相手の情報を取得
            CharaData playerData = gameManager.playerData;
            
            //攻撃のデータを取得
            Hp -= (playerData.Atk + playerData.N_Skill.Power);
            if (Hp < 0)
            {
                Hp = 0;
            }
            gameManager.enemyHpSlider.value = Hp;
            //HPを判定
            gameManager.WinerCharaCheck(Hp, gameObject);
            //ダメージモーション
            animator.SetTrigger("Damage");

            //ノックバック
            //速度を消す
            rb.velocity = Vector3.zero;
            //ノックバック方向と力を計算
            Vector3 distination = gameManager.KnockBackDirection(charaData.KnockBackPower, false);
            //ノックバック処理
            rb.AddForce(distination, ForceMode.VelocityChange);
            //Debug.Log("KnockBack:" + distination);
            Debug.Log("HP:"+ Hp + " ダメージ:" + playerData.Atk + "+" + playerData.N_Skill.Power);
        }

    }
    
    private void OnTriggerExit(Collider collision)
    {
        //Debug.Log("OnTriggerExit " + collision.tag);
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
    }

}
