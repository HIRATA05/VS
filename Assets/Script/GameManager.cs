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

    //X��Y�̏��
    //float xLimit = 8.5f;
    //float yLimit = 4.5f;

    public enum BattleSceneMode
    {
        BattleBefor,
        BattleScene,
        End
    }
    public BattleSceneMode battleScene = BattleSceneMode.BattleBefor;

    //�퓬�̃f�[�^
    public GameData gameData;

    //�v���C���[�̃f�[�^
    public CharaData playerData;

    //�ΐ푊��̃f�[�^
    public CharaData enemyData;

    //�L�����̎���
    public GameObject playerChara;
    public GameObject enemyChara;

    //�퓬�O���oUI
    [SerializeField] private GameObject BattleBeforUI;

    //�퓬��UI
    [SerializeField] private GameObject BattleUI;
    //���O�\��UI
    [SerializeField] private TextMeshProUGUI PlayerNameText;
    [SerializeField] private TextMeshProUGUI EnemyNameText;
    //HP�\��UI
    public Slider playerHpSlider;
    public Slider enemyHpSlider;

    //�퓬�O���o����
    private float BattleBeforTime = 3.0f;

    //�퓬�I��UI
    [SerializeField] private GameObject BattleFinishUI;

    //���Ҕs��
    private GameObject WinerChara;
    private GameObject LoserChara;
    //�������̎���
    private float EndTime = 10.0f;
    private float elapsedTime = 0;
    //�퓬�I��
    private bool isFinish = false;
    //���s�\��UI
    [SerializeField] private TextMeshProUGUI JudgeText;
    //���s����
    private string WinText = "Win";
    private string LoseText = "Lose";

    void Start()
    {
        //UI�ɖ��O��\��
        PlayerNameText.text = playerData.Name;
        EnemyNameText.text = enemyData.Name;
        //�X���C�_�[��HP�̐��l�ɂ���
        playerHpSlider.value = playerData.Hp;
        enemyHpSlider.value = enemyData.Hp;

    }

    void Update()
    {
        //
        if(battleScene == BattleSceneMode.BattleBefor)
        {
            //�o�g���J�n��UI��\��
            if (!BattleBeforUI.activeSelf) 
            {
                BattleBeforUI.SetActive(true);
            }

            //��莞�ԂŐ퓬�J�n
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
            //HP�\����UI
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
                //�퓬�I��UI��\��
                BattleFinishUI.SetActive(true);
                //���҂ɏ������[�V�����A�s�҂ɔs�k���[�V����
                WinerChara.GetComponent<Animator>().SetTrigger("Win");
                LoserChara.GetComponent<Animator>().SetTrigger("Lose");

                //���s�̕\��
                if (playerChara == WinerChara)
                {
                    //�v���C���[�����҂̎�WIN��\��
                    JudgeText.text = WinText;
                }
                else
                {
                    //�v���C���[���s�҂̎�LOSE��\��
                    JudgeText.text = LoseText;
                }
            }

            //��莞�Ԍ�Ƀ^�C�g���ɖ߂�
            elapsedTime += Time.deltaTime;
            if (EndTime < elapsedTime)
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    //�L������HP���m�F��HP���[���������瑊��̏���
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


    //�L�������͈͊O�ɍs���Ȃ��悤����
    public void CharaMoveLimit(GameObject chara)
    {
        //�ǉ��@���݂̃|�W�V������ێ�����
        Vector3 currentPos = chara.transform.position;

        //�ǉ��@Mathf.Clamp��X,Y�̒l���ꂼ�ꂪ�ŏ��`�ő�͈͓̔��Ɏ��߂�B
        //�͈͂𒴂��Ă�����͈͓��̒l��������
        currentPos.x = Mathf.Clamp(currentPos.x, -gameData.xLimit, gameData.xLimit);
        currentPos.z = Mathf.Clamp(currentPos.z, -gameData.zLimit, gameData.zLimit);

        //�ǉ��@position��currentPos�ɂ���
        chara.transform.position = currentPos;
        //Debug.Log(chara + "Mathf.Clamp" + currentPos);
    }

    //�m�b�N�o�b�N�̕������v�Z
    public Vector3 KnockBackDirection(float knockBackPower , bool CharaType)
    {
        Vector3 distination;
        if (CharaType)//TRUE�Ńv���C���[�@FALSE�œG
        {
            // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
            distination = (playerChara.transform.position - enemyChara.transform.position).normalized;
        }
        else
        {
            // �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
            distination = (enemyChara.transform.position - playerChara.transform.position).normalized;
        }
        
        return distination * knockBackPower;
    }
}
