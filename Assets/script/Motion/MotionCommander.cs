using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCommander : MonoBehaviour
{
    // •Ï”‚ÌéŒ¾

    public bool _motion_flag = true;    ///True‚¾‚Á‚½‚çƒ‚[ƒVƒ‡ƒ“Ä¶‰Â”\
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
    }

    public void Motion_OnePlay()
    {
        _motionplayer.PlayMotion(current_animation);
    }

}
