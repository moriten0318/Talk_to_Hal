using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public GameObject button;

    private void Start()
    {
        if (button == null)
        {
            Debug.LogError("Button reference is not set. Please assign the button in the Inspector.");
        }

    }

    public void OnButtonClick()
    {
        if (button != null)
        {
            button.gameObject.SetActive(false); // ボタンのUIを非表示にする
        }
    }
}
