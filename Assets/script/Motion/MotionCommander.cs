using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCommander : MonoBehaviour
{
    // �ϐ��̐錾

    public bool _motion_flag = true;    ///True�������烂�[�V�����Đ��\
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
        int value = 0; // �f�t�H���g�̒l

        switch (emotion)
        {
            case "�{��":
                value = 1;
                break;
            case "�߂���":
                value = 2;
                break;
            case "����":
                value = 3;
                break;
            case "������":
                value = 4+Random.Range(0,3); ;
                break;
            default:
                // �z��O�̊���n���ꂽ�ꍇ�A�G���[���b�Z�[�W��\��������A�f�t�H���g�l��ێ������肵�܂�
                Debug.LogError("�����Ȋ���: " + emotion);
                break;
        }

        return value;
    }

}
