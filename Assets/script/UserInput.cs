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
        //ユーザーがメッセージを入力するInputField(TMP)の宣言
        _userTextBox = _userTextBox.GetComponent<TMPro.TMP_InputField>();

    }
    public void GPTChat_MessageSubmit()///InputFieldへの入力が完了したらこの関数を呼び出してGPTにメッセージを送る
    {
        userMessage = _userTextBox.text;
        if (string.IsNullOrEmpty(userMessage))
        {
            return;
        }
        _gptChat.MessageSubmit(userMessage);
        Debug.Log("ユーザーのリクエストを送信しました");
    }
}
