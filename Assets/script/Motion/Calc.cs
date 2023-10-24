using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calc : MonoBehaviour
{
    ///アニメーションの速度や滑らかさを制御する関数
    /// 引数 t は進行度合い（通常は0から1の範囲）を表します。

    public static float EasyEaseIn(float t)
    ///関数は進行度合い t を取り、イージングイン（イージングが始まる部分）の値を返します。
    ///この関数は t の二乗を返すため、ゆっくり始まり、急速に進行が進む効果を提供します。
    {
        return t * t;
    }

    public static float EasyEaseOut(float t)
    {
        ///この関数はイージングインの逆で、進行度合いに対する補間値を計算して進行度合いが最終的に1に達するようにします。
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
