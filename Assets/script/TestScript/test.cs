using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using say;

public class test : MonoBehaviour
{

    public GameObject targetObject; // �G�f�B�^�[����擾�ł���悤��public�ɂ��Ă���
    // Use this for initialization
    void Start()
    {
        sayHello say = targetObject.GetComponent<sayHello>(); //�R���|�[�l���g���擾
        say.Hello(); // �Ώۂ̃R���|�[�l���g�̃X�N���v�g�����s
    }

    // Update is called once per frame
    void Update()
    {

    }
}
