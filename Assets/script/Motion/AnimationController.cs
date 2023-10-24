using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

public class AnimationController : MonoBehaviour
{
    public CubismModel live2DModel; // Live2D���f�����A�T�C��
    public param[] parameters; // ���f���̃p�����[�^�[���A�T�C��

    public param param;

    // �A�j���[�V�����N���b�v�̐ݒ�
    public actionclip[] animationClips;

    private void Start()
    {
        // �p�����[�^�[�̏�����
        foreach (var param in parameters)
        {
            param.InitializeParameters(live2DModel);
        }

        // �A�j���[�V�����̊J�n
        foreach (var clip in animationClips)
        {
            clip.Reflesh(); // �e�A�j���[�V������������
            clip.duration = 2.0f; // �A�j���[�V�����̌p�����Ԃ�ݒ�
            clip.delay = 1.0f; // �A�j���[�V�����̃f�B���C��ݒ�
        }
    }

    private void Update()
    {
        // �A�j���[�V�����N���b�v���X�V
        foreach (var clip in animationClips)
        {
            if (clip.isAnimationEnd) // �A�j���[�V�������I��������
            {
                clip.Reflesh(); // �A�j���[�V������������
            }
            else
            {
                foreach (var param in parameters)
                {
                    param.SetAnimation(new actionclip[] { clip }, false);
                }
            }
        }

        // �p�����[�^�[�̍X�V
        foreach (var param in parameters)
        {
            param.LateUpdate();
        }
    }
}
