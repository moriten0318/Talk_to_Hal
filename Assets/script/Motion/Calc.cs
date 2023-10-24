using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    ///�A�j���[�V�����̑��x�⊊�炩���𐧌䂷��֐�
    /// ���� t �͐i�s�x�����i�ʏ��0����1�͈̔́j��\���܂��B

    public static float EasyEaseIn(float t)
    ///�֐��͐i�s�x���� t �����A�C�[�W���O�C���i�C�[�W���O���n�܂镔���j�̒l��Ԃ��܂��B
    ///���̊֐��� t �̓���Ԃ����߁A�������n�܂�A�}���ɐi�s���i�ތ��ʂ�񋟂��܂��B
    {
        return t * t;
    }

    public static float EasyEaseOut(float t)
    {
        ///���̊֐��̓C�[�W���O�C���̋t�ŁA�i�s�x�����ɑ΂����Ԓl���v�Z���Đi�s�x�������ŏI�I��1�ɒB����悤�ɂ��܂��B
        return -1 * t * (t - 2.0f);
    }
    public static float EasyEaseInOut(float t)
    {
        t = t * 0.5f;
        if (t < 1) { return 0.5f * t * t; }
        t = t - 1;
        return -0.5f * (t * (t - 2.0f) - 1);
    }
}
