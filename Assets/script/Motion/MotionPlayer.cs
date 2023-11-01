using Live2D.Cubism.Framework.Motion;
using UnityEngine;
public class MotionPlayer : MonoBehaviour
{
    /// <summary>
    /// 動かしたいLive2DモデルにCubismMotionControllerと一緒にアタッチすること！
    /// 基本このスクリプトはいじらないこと！
    /// </summary>
    CubismMotionController _motionController;
    public AnimationClip idle_animation;

    private void Start()
    {
        _motionController = GetComponent<CubismMotionController>();
    }


    public void Play_roopMotion(AnimationClip animation)///このメソッドの引数にAnimationClipを渡せば動くよ
    {
        if ((_motionController == null) || (animation == null))//モーションココントローラーやアニメが未指定ならそのまま返す
        {
            return;
        }

        _motionController.PlayAnimation(animation, isLoop: true);//isloop=trueならループ再生にする
    }

    public void PlayMotion(AnimationClip animation)///このメソッドの引数にAnimationClipを渡せば動くよ
    {
        if ((_motionController == null) || (animation == null))
        {
            return;
        }
        _motionController.PlayAnimation(animation, isLoop: false);
    }

    public void StopMotion()
    {
        Debug.Log("モーション止める");
        _motionController.StopAllAnimation();
        Debug.Log("モーション止めた");
    }
}