using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCommander : MonoBehaviour
{
    // 変数の宣言

    public bool _motion_flag = true;    ///Trueだったらモーション再生可能
    public GameObject _haru_model;
    public MotionPlayer _motionplayer;

    public AnimationClip idle_animation;
    public AnimationClip current_animation;

    public List<AnimationClip> motionClips = new List<AnimationClip>();
    public List<AnimationClip> happymotions = new List<AnimationClip>();
    public AnimationClip normal;
    public AnimationClip angry;
    public AnimationClip sad;
    public AnimationClip surprize;
    public AnimationClip happy;
    public AnimationClip happy2;
    public AnimationClip happy3;

    void Start()
    {
        _motionplayer = _haru_model.GetComponent<MotionPlayer>();

        motionClips.Add(normal);
        motionClips.Add(angry);
        motionClips.Add(sad);
        motionClips.Add(surprize);
        motionClips.Add(happy);
        motionClips.Add(happy2);
        motionClips.Add(happy3);
    }

    public void Idle_Motion_Play()
    {
        _motionplayer.Play_roopMotion(idle_animation);
    }

    public void Motion_OnePlay(AnimationClip clip)
    {
        /*_motionplayer.StopMotion();*/
        _motionplayer.PlayMotion(clip);
    }

    public int GetEmotionValue(string emotion)
    {
        int value = 0; // デフォルトの値

        switch (emotion)
        {
            case "怒り":
                value = 1;
                break;
            case "悲しい":
                value = 2;
                break;
            case "驚き":
                value = 3;
                break;
            case "嬉しい":
                value = 4+Random.Range(0,3); ;
                break;
            default:
                // 想定外の感情が渡された場合、エラーメッセージを表示したり、デフォルト値を保持したりします
                Debug.LogError("無効な感情: " + emotion);
                break;
        }

        return value;
    }

}
