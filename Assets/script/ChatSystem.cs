using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    public class ChatSystem : MonoBehaviour
    {

        /// ������GPT�Ԃ̃`���b�g���e��\������UI��Prefab�𐶐����邽�߂̃N���X

        public GameObject _myChatNode;/// ��������Prefab�̂��߂̕ϐ��錾
        public GameObject _returnChatNode;
        public GameObject _ChatContent_parent;///chatnode�̐e�v�f�p�ϐ�
        public TMP_InputField _TextBox;
        public Scrollbar _ScrollBar;

        public GameObject _motionCommanderObject;///animation����p
        private MotionCommander _motionCommander;

        string userMessage;
        string returnMessage;

        void Start()
        {
            //���[�U�[�����b�Z�[�W����͂���InputField(TMP)�̐錾
            _TextBox = _TextBox.GetComponent<TMPro.TMP_InputField>();
            _ScrollBar = _ScrollBar.GetComponent<Scrollbar>();
            _motionCommander = _motionCommanderObject.GetComponent<MotionCommander>();
        }

        public void Create_Mine_chatNode()
        {
            if (_TextBox.text == "")
            {
                Debug.LogError("�e�L�X�g�{�b�N�X�ɉ������͂���Ă��܂���");
            }

            if (_TextBox.text != "")
                {
                GameObject instantiatedNode_Prefab = Instantiate(_myChatNode, _ChatContent_parent.transform);///Prefab����

                Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
                Transform parent02Transform = parent01Transform.Find("ChatBoard");
                Transform childTransform = parent02Transform.Find("ChatText");

                TextMeshProUGUI _myChatNode_text = childTransform.GetComponent<TextMeshProUGUI>();

                userMessage = _TextBox.text;
                _myChatNode_text.text = userMessage;

                _TextBox.text = "";///InputField���󗓂ɂ���
                ///Debug.Log("�t�B�[���h����ɂ��܂���");
                }
                _ScrollBar.value = 0f;
        }

        public void Create_GPT_chatNode(string GPTmessage)
        {
            GameObject instantiatedNode_Prefab = Instantiate(_returnChatNode, _ChatContent_parent.transform);///GPT����̕ԐM��\������Prefab
            Transform parent01Transform = instantiatedNode_Prefab.transform;///��������Prefab��Text�̐e�v�f�Ȃ̂ŁA�e��Transform�Ƃ��Ď擾
            Transform parent02Transform = parent01Transform.Find("ChatBoard");
            Transform childTransform = parent02Transform.Find("ChatText");

            TextMeshProUGUI _returnChatNode_text = childTransform.GetComponent<TextMeshProUGUI>();
            returnMessage = GPTmessage;
             _returnChatNode_text.text = returnMessage;
            _ScrollBar.value = 0f;

        }

    }

