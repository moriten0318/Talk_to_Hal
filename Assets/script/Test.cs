using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;/// <summary>
/// TextMeshPro���g�����߂ɕK�v�I�I

public class Test : MonoBehaviour
{

    //�I�u�W�F�N�g�ƌ��т���(�ϐ��̐錾)
    [SerializeField] TMP_InputField inpF;
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        //Component���A�^�b�`�ł���悤�ɂ���
        inpF = inpF.GetComponent<TMPro.TMP_InputField>();
        text = text.GetComponent<TMPro.TextMeshProUGUI> ();

    }

    public void InputText()
    {
        //�e�L�X�g��inputField�̓��e�𔽉f
        text.text = inpF.text;
        Debug.Log("hello");

    }
}
