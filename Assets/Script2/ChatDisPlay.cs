//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Fusion;
//using TMPro;
//using UnityEngine.UI;
//public class ChatDisPlay : NetworkBehaviour
//{

//    //����                                            1 ���� �Է°� ������� ä���Է� 
//    //     2 ������ �ʿ��� �޼����� �Ⱥ��� 
//    //     3 ���̵� �ڿ� ���� ������ ���̵�� ���� 
//    [Networked(OnChanged = nameof(OnChangeChatLog))]
//    public string chatLog { get; set; }
//    public bool logChange = false;

//    TMP_Text TMPText;

//    [SerializeField] Scrollbar scrollbar;

//    // Start is called before the first frame update
//    void Start()
//    {
//        TMPText = GetComponentInChildren<TMP_Text>();
//    }

//    public override void FixedUpdateNetwork()
//    {
//        if (logChange)
//        {
//            TMPText.text += chatLog;
//            logChange = false;
//        }

//        if (scrollbar.value != 0)
//        {
//            scrollbar.value = 0;
//        }
        
//    }

//    static void OnChangeChatLog(Changed<ChatDisPlay> changed)
//    {
//        Debug.Log("ChatDis is Change ");


//        string newchatLog = changed.Behaviour.chatLog;
//        if (newchatLog == "") //""�� �ʱ�ȭ �ϱ� ������ �Ƿ���� ü������ ���� 
//        {
//            return;
//        }
//        //Load the old value
        


//    }

    

//    public void PushChatLog(Transform _name, string _chat)
//    {
//        chatLog = _name.name + _chat + "\n";
//        logChange = true;
//    }
//}
