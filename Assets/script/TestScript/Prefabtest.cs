using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prefabtest : MonoBehaviour
{
    public Canvas _testcanvas;
    public GameObject _testprefab;/// ��������Prefab�̂��߂̕ϐ��錾
    public TMP_InputField _userTextBox;

    void Start()
    {
    Instantiate(_testprefab, _testcanvas.transform);///Prefab����
    }

    // Update is called once per frame
    void prefabcreate(string message)
    {

    Instantiate(_testprefab, _testcanvas.transform);///Prefab����

    }
}
