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

    void Start()
    {
        _motionplayer = _haru_model.GetComponent<MotionPlayer>();
    }

    public void Idle_Motion_Play()
    {

        _motionplayer.Play_roopMotion(idle_animation);
        Debug.Log("�A�C�h�����O���E�E�E");
    }

    public void Motion_OnePlay()
    {
        _motionplayer.PlayMotion(current_animation);
    }

}
