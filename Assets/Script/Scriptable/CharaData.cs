using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaData", menuName = "ScriptableObject/CharaData")]
public class CharaData : ScriptableObject
{
    //�L�������Ƃ̃f�[�^

    //�A�j���[�V����
    //public AnimatorController animatorController;

    //���O
    public string Name;

    //HP
    public int Hp;
    //SP

    //�U����
    public int Atk;
    //�ړ����x
    public float MoveSpeed;
    //�W�����v��
    public float JampPower;
    //�W�����v���x
    public float JampSpeed;
    //�m�b�N�o�b�N��
    public float KnockBackPower = 2.0f;
    //�Z�@�Z�ݒ�̃N���X�����@N���㉺�̋Z������
    public Skill N_Skill;
    //


}

[System.Serializable]
public class Skill
{
    //�З�
    public int Power;
    //����SP

    //���[�V����������
    public bool isMotion;
}