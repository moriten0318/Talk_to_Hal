using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

public class param : MonoBehaviour
{

    private param[] Live2DParams;
    private CubismModel Model;

    public CubismParameter dat;
    //Animation
    private actionclip[] animations;
    public bool isRepeat = false;

    private int currentAnimCnt = 0;
    private float startValue = 0;

    public string ID
    {
        get
        {
            return dat.Id;
        }
    }

    public void SetAnimation(actionclip[] Animations, bool Repeat = false)
    {
        currentAnimCnt = 0;
        animations = Animations;
        isRepeat = Repeat;

        startValue = dat.Value;
    }
    public void StopAnimation()
    {
        isRepeat = false;
        animations = null;
    }

    public bool isRunning
    {
        get
        {
            return animations != null;
        }
    }

    public void Update()
    {
        if (animations != null)
        {
            dat.Value = animations[currentAnimCnt].Value;
            if (animations[currentAnimCnt].isAnimationEnd)
            {
                animations[currentAnimCnt].Reflesh();
                currentAnimCnt++;
                if (currentAnimCnt >= animations.Length)
                {
                    if (isRepeat)
                    {
                        currentAnimCnt = 0;
                    }
                    else
                    {
                        StopAnimation();
                    }
                }
            }
        }
    }

    public void InitializeParameters(CubismModel model)
    {
        Model = model;
        Live2DParams = new param[model.Parameters.Length];

        for (int i = 0; i < model.Parameters.Length; i++)
        {
            Live2DParams[i] = new param();
            Live2DParams[i].dat = model.Parameters[i];
        }
    }

    public void LateUpdate()
    {
        for (int i = 0; i < Live2DParams.Length; i++)
        {
            Live2DParams[i].Update();
        }
    }
}
