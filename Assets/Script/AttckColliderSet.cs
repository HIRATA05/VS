using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckColliderSet : MonoBehaviour
{
    //攻撃コライダーのセット

    public Collider N_AttackCol;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //N攻撃発生
    public void N_Attack()
    {
        //Debug.Log("N_Attack true");
        //攻撃コライダーを表示
        N_AttackCol.enabled = true;
    }

    //N攻撃消去
    public void N_AttackDelete()
    {
        //Debug.Log("N_Attack false");
        //攻撃コライダーを非表示
        N_AttackCol.enabled = false;
    }

    //ダメージモーション
    //全ての攻撃コライダーを非表示
    public void TakeDamage()
    {
        //攻撃コライダーを非表示
        N_AttackCol.enabled = false;
    }
}
