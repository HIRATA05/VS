using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaData", menuName = "ScriptableObject/CharaData")]
public class CharaData : ScriptableObject
{
    //キャラごとのデータ

    //アニメーション
    //public AnimatorController animatorController;

    //名前
    public string Name;

    //HP
    public int Hp;
    //SP

    //攻撃力
    public int Atk;
    //移動速度
    public float MoveSpeed;
    //ジャンプ力
    public float JampPower;
    //ジャンプ速度
    public float JampSpeed;
    //ノックバック力
    public float KnockBackPower = 2.0f;
    //技　技設定のクラスを作る　N横上下の技がある
    public Skill N_Skill;
    //


}

[System.Serializable]
public class Skill
{
    //威力
    public int Power;
    //消費SP

    //モーション中判定
    public bool isMotion;
}