using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObject/GameData")]
public class GameData : ScriptableObject
{
    //プレイヤーの操作するキャラ
    public CharaData playerData;

    //対戦相手
    public CharaData enemyData;

    //選択されたステージ


    //XとYの上限
    public float xLimit = 10.0f;
    public float zLimit = 5.0f;

}
