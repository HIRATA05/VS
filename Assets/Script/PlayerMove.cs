using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //�v���C���[�ɑ΂������ŃA�^�b�`����
    //�v���C���[�̑�����Ǘ�����
    //�L�����ɂ͏d�͂�ݒ肵�Ȃ��Ŏ����ō��

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

    float y_temp, y_prev;

    void Start()
    {
        //�Q�[���}�l�[�W���[����v���C���[�̃f�[�^��Ⴄ
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        charaData = gameManager.playerData;

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
        //
        if(gameManager.battleScene == GameManager.BattleSceneMode.BattleScene)
        {
            //�ړ�
            CharaMove();

        }
    }

    //�퓬���̑���
    private void CharaMove()
    {
        //�������̈ړ�
        /*
        float yokoyajirushi = Input.GetAxis("Horizontal");
        pos.x += yokoyajirushi;
        transform.position = pos;*/
        //gameObject.transform.position = new Vector3(gameObject.transform.position.x + yokoyajirushi, gameObject.transform.position.y, gameObject.transform.position.z);
        float horizontalKey = Input.GetAxisRaw("Horizontal");

        if (horizontalKey > 0)
        {
            //�ړ��A�j���[�V��������
            animator.SetBool("Move", true);
            //xSpeed = charaData.MoveSpeed;
            //rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized * charaData.MoveSpeed;
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * charaData.MoveSpeed * Time.deltaTime;

            //�L�����̌�����ς���
            transform.LookAt(RotRight);
            //transform.Rotate(new Vector3(0, 180, 0));
        }
        else if (horizontalKey < 0)
        {
            //�ړ��A�j���[�V��������
            animator.SetBool("Move", true);
            //xSpeed = -charaData.MoveSpeed;
            //rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized * charaData.MoveSpeed;
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0) * charaData.MoveSpeed * Time.deltaTime;

            //�L�����̌�����ς���
            transform.LookAt(RotLeft);
            //transform.Rotate(new Vector3(0, 180, 0));
        }
        else
        {
            //�ړ��A�j���[�V������~
            animator.SetBool("Move", false);
        }
        //rb.velocity = new Vector3(xSpeed, rb.velocity.y, rb.velocity.z).normalized;

        //�L�������͈͊O�ɍs���Ȃ��悤����
        gameManager.CharaMoveLimit(gameObject);

        //�ڒn���ăX�y�[�X�L�[�ŃW�����v
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGround())
            {
                Debug.Log("�W�����v_Space");
                //�W�����v�A�j���[�V��������
                animator.SetBool("IsJamp", true);
                //rb.velocity = Vector3.up * charaData.JampPower;
                rb.AddForce(Vector3.up * charaData.JampPower, ForceMode.Impulse);
                //isGround = false;
            }
        }
        /*
        //�n��ɂ���ƃK�[�h���ł���
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Guard_X");

            //�K�[�h�A�j���[�V��������
            animator.SetBool("IsGuard", true);
        }
        else if (Input.GetKeyUp(KeyCode.X))
        {
            Debug.Log("Guard_X����");
            //�K�[�h�A�j���[�V��������
            animator.SetBool("IsGuard", false);
        }
        */

        //�U���A�j���[�V�����ɃA�j���[�V�����C�x���g�����Ă���ɂ���čU���̏���������
        //�U���̔���𐶐�����
        //�ߋ����̓R���C�_�[���I���I�t�@�������̓R���C�_�[���������I�u�W�F�N�g�𐶐�
        //���͂ŃA�j���[�V�����𔭐�
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Debug.Log("N_Z");
            //�U���A�j���[�V��������
            animator.SetTrigger("Attack");
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
        Debug.Log("OnTriggerEnter "+collision.tag);
        if (collision.tag == groundTag)
        {
            //�W�����v�A�j���[�V�����I��
            animator.SetBool("IsJamp", false);

            isGroundEnter = true;
        }

        //�U���̓����蔻��
        //�����������̂��ΐ푊�肩�m�F
        if (collision.CompareTag("EnemyAttack"))
        {
            //�Q�[���}�l�[�W���[���瑊��̏����擾
            CharaData enemyData = gameManager.enemyData;

            //�U���̃f�[�^���擾
            Hp -= (enemyData.Atk + enemyData.N_Skill.Power);
            if (Hp < 0)
            {
                Hp = 0;
            }
            gameManager.playerHpSlider.value = Hp;
            //HP�𔻒�
            gameManager.WinerCharaCheck(Hp, gameObject);
            //�_���[�W���[�V����
            animator.SetTrigger("Damage");

            //�m�b�N�o�b�N
            //���x������
            rb.velocity = Vector3.zero;
            //�m�b�N�o�b�N�����Ɨ͂��v�Z
            Vector3 distination = gameManager.KnockBackDirection(charaData.KnockBackPower, true);
            //�m�b�N�o�b�N����
            rb.AddForce(distination, ForceMode.VelocityChange);
            //Debug.Log("KnockBack:" + distination);
            Debug.Log("HP:" + Hp + " �_���[�W:" + enemyData.Atk + "+" + enemyData.N_Skill.Power);
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

    //�A�j���[�V�����C�x���g
    //�����蔻��̔���

    //�������U���̔���

}
