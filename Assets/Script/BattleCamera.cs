using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    //�v���C���[�Ƒΐ푊����J�����Ɏ��߂�

    //�Q�[���}�l�[�W���[
    GameManager gameManager;

    float camX = 0;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        camX = gameManager.playerChara.transform.position.x;

        transform.position = new Vector3(camX, transform.position.y, transform.position.z);
    }
}
