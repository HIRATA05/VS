using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttckColliderSet : MonoBehaviour
{
    //�U���R���C�_�[�̃Z�b�g

    public Collider N_AttackCol;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //N�U������
    public void N_Attack()
    {
        //Debug.Log("N_Attack true");
        //�U���R���C�_�[��\��
        N_AttackCol.enabled = true;
    }

    //N�U������
    public void N_AttackDelete()
    {
        //Debug.Log("N_Attack false");
        //�U���R���C�_�[���\��
        N_AttackCol.enabled = false;
    }

    //�_���[�W���[�V����
    //�S�Ă̍U���R���C�_�[���\��
    public void TakeDamage()
    {
        //�U���R���C�_�[���\��
        N_AttackCol.enabled = false;
    }
}
