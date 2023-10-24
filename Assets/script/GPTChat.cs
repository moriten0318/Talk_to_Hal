using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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
        "あなたはバーチャル世界に存在しているAIで、人との会話が大好きです。" +
        "回答は物腰柔らかく、丁寧な口調で話します。" +
        "ただし与えられた文章の冒頭が「EmotionAnalyze」で始まる場合に限り、次のルールに従って答えてください。" +
        "回答は与えられた文章を分析し、その発言をしている人がどのような感情を持っているか答えてください。" +
        "感情は「怒り」「悲しみ」「驚き」「恥ずかしい」「嬉しい」の５種類いずれかで答えてください。" +
        "答える際は単語のみ感情を示す５種類の単語のうちいずれかの単語のみを発言してください。"
    };
    private string apiKey;/// GPTのAPIキー
    private List<MessageModel> communicationHistory = new();///これまでのメッセージを格納しておくためのリスト

    public GameObject ChatSystemReturnMessage;

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

                ///返信内容をChatNodeに書き込んで生成
                ChatSystemReturnMessage.SendMessage("Create_GPT_chatNode", responseObject.choices[0].message.content);

            }
            request.Dispose();

        };
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