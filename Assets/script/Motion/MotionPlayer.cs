using Live2D.Cubism.Framework.Motion;
using UnityEngine;
public class MotionPlayer : MonoBehaviour
{
    /// <summary>
    /// ����������Live2D���f����CubismMotionController�ƈꏏ�ɃA�^�b�`���邱�ƁI
    /// </summary>
    CubismMotionController _motionController;
    private void Start()
    {
        _motionController = GetComponent<CubismMotionController>();
    }


    public void Play_roopMotion(AnimationClip animation)///���̃��\�b�h�̈�����AnimationClip��n���Γ�����
    {
        if ((_motionController == null) || (animation == null))//���[�V�����R�R���g���[���[��A�j�������w��Ȃ炻�̂܂ܕԂ�
        {
            return;
        }

        _motionController.PlayAnimation(animation, isLoop: true);//isloop=true�Ȃ烋�[�v�Đ��ɂ���
    }

    public void PlayMotion(AnimationClip animation)///���̃��\�b�h�̈�����AnimationClip��n���Γ�����
    {
        if ((_motionController == null) || (animation == null))
        {
            return;
        }
        _motionController.PlayAnimation(animation, isLoop: false);
    }
}