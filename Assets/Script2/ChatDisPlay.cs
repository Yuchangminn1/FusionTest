//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Fusion;
//using TMPro;
//using UnityEngine.UI;
//public class ChatDisPlay : NetworkBehaviour
//{

//    //문제                                            1 엔터 입력과 상관없이 채팅입력 
//    //     2 참여자 쪽에서 메세지가 안보임 
//    //     3 아이디가 뒤에 들어온 참가자 아이디로 변경 
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
//        if (newchatLog == "") //""로 초기화 하기 떄문에 피료없는 체인지는 리턴 
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
