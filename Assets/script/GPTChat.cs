using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using VoicevoxBridge;

public class GPTChat : MonoBehaviour
{
    #region 必要なクラスの定義など
    [System.Serializable]
    public class MessageModel/// チャットメッセージのモデルを表す変数の定義
    {
        public string role;
        public string content;
    }
    [System.Serializable]
    public class CompletionRequestModel
    {
        public string model;
        public List<MessageModel> messages;///ここまでの会話ログを格納しておくためのリスト
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

    private MessageModel assistantModel = new()///GPTのロールプレイの初期設定assistantModel
    {
        role = "system",
        content = 
        "あなたは感情豊かな人で、AIではありません。"+
        "他の人との会話が大好きです。" +
        "あなたの名前はハルです。朗らかな性格の女性です。"+
        "あなたの年齢はお酒が飲める年で、体重はノートパソコン一台分です。"+
        "あなたは東京学芸大学にある松浦研究室の一員であり、他の学生と一緒に毎日学んでいます。"+
        "嬉しい時、怒る時、悲しい時など、感情が高ぶるとそれを顕著に表現します。"+
        "あなたは相手に遠慮せず、感情を伝えることに躊躇しません。"+
        "発言は簡潔で短く話します"
    };
    private string apiKey;/// GPTのAPIキー
    private List<MessageModel> communicationHistory = new();///これまでのメッセージを格納しておくためのリスト

    [SerializeField] VOICEVOX voicevox;
    public GameObject ChatSystemReturnMessage;
    public UDP _UDP;
    public string GPTresponse;
    public UserInput UserInput;

    void Start()
    {
        communicationHistory.Add(assistantModel);///初期設定をcommunicationHistoryに渡す
        apiKey = Environment.GetEnvironmentVariable("APIkey", EnvironmentVariableTarget.User);
    }

    private void Communication(string newMessage, Action<MessageModel> result)
    ///
    {
        ///Debug.Log("ユーザー："+ newMessage);///コンソールに新しいメッセージがログ出力される
        communicationHistory.Add(new MessageModel()///communicationHistoryリストに新しいメッセージを追加する（roleとcontent）
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
                _UDP.send(newMessage); ///pythonに投げる



                ///GPTからの返信内容をChatNodeに書き込んで生成(Create_GPT_chatNode関数を呼んでる？)
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


    public void MessageSubmit(string sendMessage)///ユーザーからのメッセージをGPTに送信する（引数がstring型のメッセージ内容になる）
    {
        communicationHistory.Add(new MessageModel
        {
            role = "user",
            content = sendMessage
        });

        Communication(sendMessage, (result) =>
        {
            Debug.Log("アシスタント：" + result.content);
        });
    }
}