using Live2D.Cubism.Framework.MouthMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //�����t�@�C���̍Đ��ƃ��f���̃��b�v�V���N���Ǘ�����N���X

    [SerializeField]
    CubismAudioMouthInput audioMouthInput;
    //�Đ����鉹���\�[�X�t�@�C����ݒ肷��ϐ�

    public AudioClip AudioClip1;
    public AudioClip AudioClip2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame(���t���[���X�V)
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            // Z�������ꂽ���AAudioClip1���Đ�����
            playAudio(AudioClip1, stopAudio);
        }
        else if (Input.GetKey(KeyCode.Y))
        {
            // Y�������ꂽ���AAudioClip2���Đ�����
            playAudio(AudioClip2, stopAudio);
        }
    }

    public delegate void functionType();
    private IEnumerator Checking(functionType callback)
    {
        if (audioMouthInput?.AudioInput == null)
        {
            Debug.LogError("audioMouthInput.AudioInput no set");
            yield return null;
        }

        while (true)
        {
            // 1�t���[���҂�
            yield return new WaitForFixedUpdate();

            if (!audioMouthInput.AudioInput.isPlaying)
            {
                callback();
                break;
            }
        }
    }

    private void playAudio(AudioClip audioClip, functionType callback)
    {
        if (audioMouthInput?.AudioInput == null)
        {
            Debug.LogError("audioMouthInput.AudioInput�ݒ�Ȃ�");
            return;
        }

        audioMouthInput.AudioInput.clip = audioClip; // �����t�@�C����ύX
        audioMouthInput.AudioInput.Play();
        StartCoroutine(Checking(callback));
        Debug.Log("play sound");
    }

    // Checking���\�b�h��callback���\�b�h�Ŏ��s�����
    private void stopAudio()
    {
        Debug.Log("stop sound");
    }
}