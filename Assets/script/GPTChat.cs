using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using VoicevoxBridge;

public class GPTChat : MonoBehaviour
{
    #region �K�v�ȃN���X�̒�`�Ȃ�
    [System.Serializable]
    public class MessageModel/// �`���b�g���b�Z�[�W�̃��f����\���ϐ��̒�`
    {
        public string role;
        public string content;
    }
    [System.Serializable]
    public class CompletionRequestModel
    {
        public string model;
        public List<MessageModel> messages;///�����܂ł̉�b���O���i�[���Ă������߂̃��X�g
    }

    [System.Serializable]
    public class ChatGPTRecieveModel
    {
        public string id;
        public string @object;
        public int created;
        public Choice[] choices;
        public Usage usage;

        [System.Serializable]
        public class Choice
        {
            public int index;
            public MessageModel message;
            public string finish_reason;
        }

        [System.Serializable]
        public class Usage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }
    }
    #endregion

    private MessageModel assistantModel = new()///GPT�̃��[���v���C�̏����ݒ�assistantModel
    {
        role = "system",
        content = 
        "���Ȃ��̖��O=�n��"+
        "���Ȃ��̐��ʁ����̎q"+
        "���Ȃ��̔N��=���������߂�N" +
        "���Ȃ��̑̏d=�m�[�g�p�\�R����䕪�ł��B" +
        "���Ȃ��̍D���Ȃ���=�l�Ƃ��b�����邱��"+
        "���Ȃ��̌����Ȃ���=�푈" +
        "���Ȃ��̍D���ȐH�ו�=�Ă���"+
        "���Ȃ��̍D���Ȃ���=�b�B�E�C�X�L�["+
        "���Ȃ��̘b������=���{��" +
        "���Ȃ��̔w�i�ݒ聁�����w�|��w�̏��Y�������̈���ł���A���̊w���ƈꏏ�ɖ����w��ł���" +
        "���Ȃ��̔w�i�ݒ聁�����͌������ɖK�ꂽ���q�l�Ƃ̉�b���y�������Ƃ��Ă���B" +
        "���������A�{�鎞�A�߂������ȂǁA������Ԃ�Ƃ���������ɕ\�����Ă��������B" +
        "���Ȃ��͑���ɉ��������A�F�l�ł���悤�ɐڂ��Ă��������B���Ȃ��͊����`���邱�Ƃ��S�O���邱�ƂȂ��b���Ă��������B"+
        "�����͊Ȍ��ŒZ���b���Ă��������B"+
        "ChatGPT�܂��́A���̑��̃L�����N�^�[�Ƃ��ĐU�镑��Ȃ��ł��������B" +
        "����Ɋ֌W�Ȃ����Ƃ́A�b���Ȃ��ł��������B�o�͕͂K���Q���ȓ��ɂ��Ă��������B���ꂪ�s�\�ȏꍇ�ł��ł��邾���A���Ȃ��Ȃ�悤�ɂ��ĉ������B"+
        "�񓚂͐�΂ɕK���Ȍ��ɓ����Ă��������B����͕K������Ă��������B"+
        "���ȏЉ������Ƃ��́A�D���Ȃ��ƂƔN��𓚂��Ă��������B"

    };
    private string apiKey;/// GPT��API�L�[
    private List<MessageModel> communicationHistory = new();///����܂ł̃��b�Z�[�W���i�[���Ă������߂̃��X�g

    [SerializeField] VOICEVOX voicevox;
    public GameObject ChatSystemReturnMessage;
    public UDP _UDP;
    public MotionCommander MotionCommander;
    public string GPTresponse;
    public UserInput UserInput;

    void Start()
    {
        communicationHistory.Add(assistantModel);///�����ݒ��communicationHistory�ɓn��
        apiKey = Environment.GetEnvironmentVariable("APIkey", EnvironmentVariableTarget.User);
    }

    private void Communication(string newMessage, Action<MessageModel> result)
    ///
    {
        ///Debug.Log("���[�U�[�F"+ newMessage);///�R���\�[���ɐV�������b�Z�[�W�����O�o�͂����
        communicationHistory.Add(new MessageModel()///communicationHistory���X�g�ɐV�������b�Z�[�W��ǉ�����irole��content�j
        {
            role = "user",
            content = newMessage
        });

        var apiUrl = "https://api.openai.com/v1/chat/completions";
        var jsonOptions = JsonUtility.ToJson(
            new CompletionRequestModel()
            {
                model = "gpt-3.5-turbo",
                messages = communicationHistory
            }, true);
        var headers = new Dictionary<string, string>
            {
                {"Authorization", "Bearer " + apiKey},
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"}
            };
        var request = new UnityWebRequest(apiUrl, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        foreach (var header in headers)
        {
            request.SetRequestHeader(header.Key, header.Value);
        }

        var operation = request.SendWebRequest();

        operation.completed += _ =>
        {
            if (operation.webRequest.result == UnityWebRequest.Result.ConnectionError ||
                       operation.webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(operation.webRequest.error);
                throw new Exception();
            }
            else
            {
                var responseString = operation.webRequest.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<ChatGPTRecieveModel>(responseString);
                communicationHistory.Add(responseObject.choices[0].message);
                Debug.Log(responseObject.choices[0].message.content);

                GPTresponse = responseObject.choices[0].message.content;
                PlayVoice(GPTresponse);
                _UDP.send(newMessage); ///python�ɓ�����



                ///GPT����̕ԐM���e��ChatNode�ɏ�������Ő���(Create_GPT_chatNode�֐����Ă�ł�H)
                ChatSystemReturnMessage.SendMessage("Create_GPT_chatNode", responseObject.choices[0].message.content);
            }
            request.Dispose();
        };
    }

    public async void PlayVoice(string text)
    {
        int speaker = 20;
        string[] splitText = text.Split(new char[] { '�B', '�A' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in splitText)
        {
            await voicevox.PlayOneShot(speaker, item);
        }
        MotionCommander.Idle_Motion_Play();

    }


    public void MessageSubmit(string sendMessage)///���[�U�[����̃��b�Z�[�W��GPT�ɑ��M����i������string�^�̃��b�Z�[�W���e�ɂȂ�j
    {
        communicationHistory.Add(new MessageModel
        {
            role = "user",
            content = sendMessage
        });

        Communication(sendMessage, (result) =>
        {
            Debug.Log("�A�V�X�^���g�F" + result.content);
        });
    }
}