using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInput : MonoBehaviour
{
    [SerializeField] TMP_InputField _userTextBox;
    public GPTChat _gptChat;
    public string userMessage;


    void Start()
    {
        //���[�U�[�����b�Z�[�W����͂���InputField(TMP)�̐錾
        _userTextBox = _userTextBox.GetComponent<TMPro.TMP_InputField>();

    }
    public void GPTChat_MessageSubmit()///InputField�ւ̓��͂����������炱�̊֐����Ăяo����GPT�Ƀ��b�Z�[�W�𑗂�
    {
        userMessage = _userTextBox.text;
        if (string.IsNullOrEmpty(userMessage))
        {
            return;
        }
        _gptChat.MessageSubmit(userMessage);
        Debug.Log("���[�U�[�̃��N�G�X�g�𑗐M���܂���");
    }
}
