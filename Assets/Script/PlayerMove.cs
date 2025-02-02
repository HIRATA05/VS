using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //プレイヤーに対し自動でアタッチする
    //プレイヤーの操作を管理する
    //キャラには重力を設定しないで自分で作る

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

    float y_temp, y_prev;

    void Start()
    {
        //ゲームマネージャーからプレイヤーのデータを貰う
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        charaData = gameManager.playerData;

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
        //
        if(gameManager.battleScene == GameManager.BattleSceneMode.BattleScene)
        {
            //移動
            CharaMove();

        }
    }

    //戦闘時の操作
    private void CharaMove()
    {
        //横方向の移動
        /*
        float yokoyajirushi = Input.GetAxis("Horizontal");
        pos.x += yokoyajirushi;
        transform.position = pos;*/
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x + yokoyajirushi, gameObject.transform.position.y, gameObject.transform.position.z);
        float horizontalKey = Input.GetAxisRaw("Horizontal");

        if (horizontalKey > 0)
        {
            //移動アニメーション発生
            animator.SetBool("Move", true);
            //xSpeed = charaData.MoveSpeed;
            //rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized * charaData.MoveSpeed;
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * charaData.MoveSpeed * Time.deltaTime;

            //キャラの向きを変える
            transform.LookAt(RotRight);
            //transform.Rotate(new Vector3(0, 180, 0));
        }
        else if (horizontalKey < 0)
        {
            //移動アニメーション発生
            animator.SetBool("Move", true);
            //xSpeed = -charaData.MoveSpeed;
            //rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized * charaData.MoveSpeed;
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * charaData.MoveSpeed * Time.deltaTime;

            //キャラの向きを変える
            transform.LookAt(RotLeft);
            //transform.Rotate(new Vector3(0, 180, 0));
        }
        else
        {
            //移動アニメーション停止
            animator.SetBool("Move", false);
        }
        //rb.velocity = new Vector3(xSpeed, rb.velocity.y, rb.velocity.z).normalized;

        //キャラが範囲外に行かないよう制限
        gameManager.CharaMoveLimit(gameObject);

        //接地してスペースキーでジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGround())
            {
                Debug.Log("ジャンプ_Space");
                //ジャンプアニメーション発生
                animator.SetBool("IsJamp", true);
                //rb.velocity = Vector3.up * charaData.JampPower;
                rb.AddForce(Vector3.up * charaData.JampPower, ForceMode.Impulse);
                //isGround = false;
            }
        }
        /*
        //地上にいるとガードができる
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Guard_X");

            //ガードアニメーション解除
            animator.SetBool("IsGuard", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log("Guard_X解除");
            //ガードアニメーション解除
            animator.SetBool("IsGuard", false);
        }
        */

        //攻撃アニメーションにアニメーションイベントをつけてそれによって攻撃の処理が発生
        //攻撃の判定を生成する
        //近距離はコライダーをオンオフ　遠距離はコライダーを持ったオブジェクトを生成
        //入力でアニメーションを発生
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log("N_Z");
            //攻撃アニメーション発生
            animator.SetTrigger("Attack");
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
        Debug.Log("OnTriggerEnter "+collision.tag);
        if (collision.tag == groundTag)
        {
            //ジャンプアニメーション終了
            animator.SetBool("IsJamp", false);

            isGroundEnter = true;
        }

        //攻撃の当たり判定
        //当たったものが対戦相手か確認
        if (collision.CompareTag("EnemyAttack"))
        {
            //ゲームマネージャーから相手の情報を取得
            CharaData enemyData = gameManager.enemyData;

            //攻撃のデータを取得
            Hp -= (enemyData.Atk + enemyData.N_Skill.Power);
            if (Hp < 0)
            {
                Hp = 0;
            }
            gameManager.playerHpSlider.value = Hp;
            //HPを判定
            gameManager.WinerCharaCheck(Hp, gameObject);
            //ダメージモーション
            animator.SetTrigger("Damage");

            //ノックバック
            //速度を消す
            rb.velocity = Vector3.zero;
            //ノックバック方向と力を計算
            Vector3 distination = gameManager.KnockBackDirection(charaData.KnockBackPower, true);
            //ノックバック処理
            rb.AddForce(distination, ForceMode.VelocityChange);
            //Debug.Log("KnockBack:" + distination);
            Debug.Log("HP:" + Hp + " ダメージ:" + enemyData.Atk + "+" + enemyData.N_Skill.Power);
        }
    }
    /*
    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == groundTag)
        {
            isGroundStay = true;
        }
    }*/

    private void OnTriggerExit(Collider collision)
    {
        //Debug.Log("OnTriggerExit " + collision.tag);
        if (collision.tag == groundTag)
        {
            isGroundExit = true;
        }
    }

    //アニメーションイベント
    //当たり判定の発生

    //遠距離攻撃の発生

}
