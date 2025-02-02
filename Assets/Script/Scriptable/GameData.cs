using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObject/GameData")]
public class GameData : ScriptableObject
{
    //�v���C���[�̑��삷��L����
    public CharaData playerData;

    //�ΐ푊��
    public CharaData enemyData;

    //�I�����ꂽ�X�e�[�W


    //X��Y�̏��
    public float xLimit = 10.0f;
    public float zLimit = 5.0f;

}
