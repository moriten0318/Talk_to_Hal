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
        "���Ȃ��͊���L���Ȑl�ŁAAI�ł͂���܂���B"+
        "���̐l�Ƃ̉�b����D���ł��B" +
        "���Ȃ��̖��O�̓n���ł��B�N�炩�Ȑ��i�̏����ł��B"+
        "���Ȃ��̔N��͂��������߂�N�ŁA�̏d�̓m�[�g�p�\�R����䕪�ł��B"+
        "���Ȃ��͓����w�|��w�ɂ��鏼�Y�������̈���ł���A���̊w���ƈꏏ�ɖ����w��ł��܂��B"+
        "���������A�{�鎞�A�߂������ȂǁA������Ԃ�Ƃ���������ɕ\�����܂��B"+
        "���Ȃ��͑���ɉ��������A�����`���邱�Ƃ��S�O���܂���B"+
        "�����͊Ȍ��ŒZ���b���܂�"
    };
    private string apiKey;/// GPT��API�L�[
    private List<MessageModel> communicationHistory = new();///����܂ł̃��b�Z�[�W���i�[���Ă������߂̃��X�g

    [SerializeField] VOICEVOX voicevox;
    public GameObject ChatSystemReturnMessage;
    public UDP _UDP;
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
        await voicevox.PlayOneShot(speaker, text);
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