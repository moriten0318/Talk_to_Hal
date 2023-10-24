using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

public class AnimationController : MonoBehaviour
{
    public CubismModel live2DModel; // Live2Dモデルをアサイン
    public param[] parameters; // モデルのパラメーターをアサイン

    public param param;

    // アニメーションクリップの設定
    public actionclip[] animationClips;

    private void Start()
    {
        // パラメーターの初期化
        foreach (var param in parameters)
        {
            param.InitializeParameters(live2DModel);
        }

        // アニメーションの開始
        foreach (var clip in animationClips)
        {
            clip.Reflesh(); // 各アニメーションを初期化
            clip.duration = 2.0f; // アニメーションの継続時間を設定
            clip.delay = 1.0f; // アニメーションのディレイを設定
        }
    }

    private void Update()
    {
        // アニメーションクリップを更新
        foreach (var clip in animationClips)
        {
            if (clip.isAnimationEnd) // アニメーションが終了したら
            {
                clip.Reflesh(); // アニメーションを初期化
            }
            else
            {
                foreach (var param in parameters)
                {
                    param.SetAnimation(new actionclip[] { clip }, false);
                }
            }
        }

        // パラメーターの更新
        foreach (var param in parameters)
        {
            param.LateUpdate();
        }
    }
}
