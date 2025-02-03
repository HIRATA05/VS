using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    //

    //�Q�[���}�l�[�W���[
    GameManager gameManager;
    //�Q�[���}�l�[�W���[���������v���C���[�̃f�[�^
    CharaData charaData;

    private Animator animator;

    private Rigidbody rb;

    int Hp, Atk;

    //����
    Vector3 RotRight = new Vector3(180, 0, 0);
    Vector3 RotLeft = new Vector3(-180, 0, 0);
    //���n���
    private bool isGround = false;
    private string groundTag = "Ground";
    private bool isGroundEnter, isGroundStay, isGroundExit;

    //�U������
    float AttackDistance = 1;

    //��莞�Ԃ��Ƃɍs����ω�
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
        //�Q�[���}�l�[�W���[����G�̃f�[�^��Ⴄ
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        charaData = gameManager.enemyData;

        //�v���C���[�L�����̃A�j���[�^�[��ݒ�
        animator = GetComponent<Animator>();

        //Rigidbody���擾
        rb = GetComponent<Rigidbody>();

        //�L�����̏����擾
        Hp = charaData.Hp;
        Atk = charaData.Atk;
    }

    void Update()
    {
        if(gameManager.battleScene == GameManager.BattleSceneMode.BattleScene)
        {
            //�X�e�[�g�ɂ���čs�����ω�
            if(enemyState == EnemyState.Move)
            {
                Vector3 dis = (gameManager.playerChara.transform.position - gameManager.enemyChara.transform.position).normalized;
                Debug.Log("dis " + dis);
                
                if (dis.x > 0)
                {
                    //�L�����̌�����ς���
                    transform.LookAt(RotRight);
                }
                else if (dis.x < 0)
                {
                    //�L�����̌�����ς���
                    transform.LookAt(RotLeft);
                }

                transform.position += new Vector3(dis.x, 0, 0) * charaData.MoveSpeed * Time.deltaTime;
                //�ړ��A�j���[�V��������
                animator.SetBool("Move", true);

                //Debug.Log("elapsedTime " + elapsedTime);
                //���̋߂��̏ꍇ�U�����[�V��������
                if (Vector3.Distance(gameManager.playerChara.transform.position, gameManager.enemyChara.transform.position) < AttackDistance)
                {
                    //���̃X�e�[�g�Ɉڍs
                    enemyState = EnemyState.Attack;
                }

                elapsedTime += Time.deltaTime;
                if (StateMoveTime < elapsedTime)
                {
                    elapsedTime = 0;
                    //���̃X�e�[�g�Ɉڍs
                    enemyState = EnemyState.Escape;
                }
            }
            else if(enemyState == EnemyState.Attack)
            {
                //�ړ��A�j���[�V������~
                animator.SetBool("Move", false);
                
                //�U�����[�V��������
                animator.SetTrigger("Attack");

                //�U����Escape�ɕω�
                enemyState = EnemyState.Escape;

            }
            else if (enemyState == EnemyState.Escape)
            {
                Vector3 dis = (gameManager.enemyChara.transform.position - gameManager.playerChara.transform.position).normalized;
                Debug.Log("dis " + dis);

                if (dis.x > 0)
                {
                    //�L�����̌�����ς���
                    transform.LookAt(RotLeft);
                }
                else if (dis.x < 0)
                {
                    //�L�����̌�����ς���
                    transform.LookAt(RotRight );
                }

                transform.position += new Vector3(dis.x, 0, 0) * charaData.MoveSpeed * Time.deltaTime;
                //�ړ��A�j���[�V��������
                animator.SetBool("Move", true);

                //Debug.Log("elapsedTime " + elapsedTime);
                elapsedTime += Time.deltaTime;
                if (StateEscapeTime < elapsedTime)
                {
                    elapsedTime = 0;
                    if (IsGround())
                    {
                        //�W�����v�A�j���[�V��������
                        animator.SetBool("IsJamp", true);
                        rb.AddForce(Vector3.up * charaData.JampPower, ForceMode.Impulse);
                    }
                    //���̋߂��̏ꍇ�U�����[�V��������
                    if (Vector3.Distance(gameManager.playerChara.transform.position, gameManager.enemyChara.transform.position) < AttackDistance)
                    {
                        //���̃X�e�[�g�Ɉڍs
                        enemyState = EnemyState.Attack;
                    }

                    //���̃X�e�[�g�Ɉڍs
                    enemyState = EnemyState.Move;
                }
            }
            //Debug.Log("enemyState "+ enemyState);

            //�L�������͈͊O�ɍs���Ȃ��悤����
            gameManager.CharaMoveLimit(gameObject);

            //���͂ŃA�j���[�V�����𔭐�
            if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("�G��N_M");
                //�U���A�j���[�V��������
                animator.SetTrigger("Attack");
            }
        }
    }

    //�ڒn�����Ԃ�
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
            //�W�����v�A�j���[�V�����I��
            animator.SetBool("IsJamp", false);

            isGroundEnter = true;
        }

        //�U���̓����蔻��
        //�����������̂��ΐ푊�肩�m�F
        if (collision.CompareTag("PlayerAttack"))
        {
            //�Q�[���}�l�[�W���[���瑊��̏����擾
            CharaData playerData = gameManager.playerData;
            
            //�U���̃f�[�^���擾
            Hp -= (playerData.Atk + playerData.N_Skill.Power);
            if (Hp < 0)
            {
                Hp = 0;
            }
            gameManager.enemyHpSlider.value = Hp;
            //HP�𔻒�
            gameManager.WinerCharaCheck(Hp, gameObject);
            //�_���[�W���[�V����
            animator.SetTrigger("Damage");

            //�m�b�N�o�b�N
            //���x������
            rb.velocity = Vector3.zero;
            //�m�b�N�o�b�N�����Ɨ͂��v�Z
            Vector3 distination = gameManager.KnockBackDirection(charaData.KnockBackPower, false);
            //�m�b�N�o�b�N����
            rb.AddForce(distination, ForceMode.VelocityChange);
            //Debug.Log("KnockBack:" + distination);
            Debug.Log("HP:"+ Hp + " �_���[�W:" + playerData.Atk + "+" + playerData.N_Skill.Power);
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
