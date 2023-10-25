using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class EmotionAnalizer : MonoBehaviour
{
    #region �K�v�ȃN���X�̒�`�Ȃ�
    [System.Serializable]
    public class EmotionAnalize/// �`���b�g���b�Z�[�W�̃��f����\���ϐ��̒�`
    {
        public string role;
        public string content;
    }
    [System.Serializable]
    public class CompletionRequestModel
    {
        public string model;
        public List<EmotionAnalize> analysis;///�����܂ł̉�b���O���i�[���Ă������߂̃��X�g
    }

    [System.Serializable]
    public class ChatGPTsubModel
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
            public EmotionAnalize message;
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

    private EmotionAnalize subModel = new()///GPT�̃��[���v���C�̏����ݒ�assistantModel
    {
        role = "system",
        content =
        "���̃��[���ɏ]���ē����Ă��������B" +
        "�^����ꂽ���͂𕪐͂��A���̔��������Ă���l���ǂ̂悤�Ȋ���������Ă��邩�����Ă��������B" +
        "����́u�{��v�u�߂��݁v�u�����v�u�p���������v�u�������v�̂T��ނ����ꂩ�œ����Ă��������B" +
        "������ۂ͒P��̂݊���������T��ނ̒P��̂��������ꂩ�̒P��݂̂𔭌����Ă��������B"
    };
    private string apiKey;/// GPT��API�L�[
    private List<EmotionAnalize> communicationHistory = new();///����܂ł̃��b�Z�[�W���i�[���Ă������߂̃��X�g

    public GameObject ChatSystemReturnMessage;

    void Start()
    {
        communicationHistory.Add(subModel);///�����ݒ��communicationHistory�ɓn��
        apiKey = Environment.GetEnvironmentVariable("APIkey", EnvironmentVariableTarget.User);
    }

    private void Analize(string newMessage, Action<EmotionAnalize> result)
    ///
    {
        ///Debug.Log("���[�U�[�F"+ newMessage);///�R���\�[���ɐV�������b�Z�[�W�����O�o�͂����
        communicationHistory.Add(new EmotionAnalize()///communicationHistory���X�g�ɐV�������b�Z�[�W��ǉ�����irole��content�j
        {
            role = "user",
            content = newMessage
        });

        var apiUrl = "https://api.openai.com/v1/chat/completions";
        var jsonOptions = JsonUtility.ToJson(
            new CompletionRequestModel()
            {
                model = "gpt-3.5-turbo",
                analysis = communicationHistory
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
                var responseObject = JsonUtility.FromJson<ChatGPTsubModel>(responseString);
                communicationHistory.Add(responseObject.choices[0].message);

            }
            request.Dispose();
        };
    }

    public void MoodDetect(string sendMessage)///GPT����̕ԐM������͂���i������string�^�̃��b�Z�[�W���e�ɂȂ�j
    {
        communicationHistory.Add(new EmotionAnalize
        {
            role = "user",
            content = sendMessage
        });

        Analize(sendMessage, (result) =>
        {
            Debug.Log("���݂̊���́F" + result.content+"�@�ł�");
        });
    }
}
