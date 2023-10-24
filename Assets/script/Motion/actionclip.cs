using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

public class actionclip : MonoBehaviour
{
    string ease = "Linear";
    public float startValue = 0;
    public float endValue = 0;
    public float duration = 0;
    public float delay = 0;

    private float currentDurationTime = 0;
    private float currentDelayTime = 0;

    public Calc Calc;
    public param param;

    public actionclip(float StartValue, float EndValue, float Duration, float NextDelay = 0, string Ease = "Linear")
    {
        startValue = StartValue;
        endValue = EndValue;
        duration = Duration < 0 ? 0 : Duration;
        delay = NextDelay < 0 ? 0 : NextDelay;
        ease = Ease;

        currentDurationTime = currentDelayTime = 0;
    }

    public void Reflesh()
    {
        currentDurationTime = currentDelayTime = 0;
    }

    public float Value
    {
        get
        {
            float p = endValue;
            if (currentDurationTime <= duration)
            {
                currentDurationTime += Time.deltaTime;
                float t = 1.0f;
                if (duration > 0)
                {
                    t = currentDurationTime / duration;
                }
                else
                {
                    t = 1.0f;
                }
                t = t > 1.0f ? 1.0f : t;
                switch (ease.ToUpper())
                {
                    case "LINEAR":
                        break;
                    case "EASEIN":
                        t = Calc.EasyEaseIn(t);
                        break;
                    case "EASEOUT":
                        t = Calc.EasyEaseOut(t);
                        break;
                    case "EASEINOUT":
                        t = Calc.EasyEaseInOut(t);
                        break;
                }
                p = Mathf.Lerp(startValue, endValue, t);

            }
            else
            {
                currentDelayTime += Time.deltaTime;
            }
            return p;
        }
    }
    public bool isAnimationEnd
    {
        get
        {
            return (Value == endValue && currentDelayTime > delay);
        }
    }
}


