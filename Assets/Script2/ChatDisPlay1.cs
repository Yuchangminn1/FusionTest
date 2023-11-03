using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
public class ChatDisPlay1 : NetworkBehaviour
{

    ////문제 1 엔터 입력과 상관없이 채팅입력 
    ////     2 참여자 쪽에서 메세지가 안보임 
    ////     3 아이디가 뒤에 들어온 참가자 아이디로 변경 
    //[Networked(OnChanged = nameof(OnChangeChatLog))]
    //public NetworkString<_16> nowString { get; set; }

    //public string lastPushName;


    //public TMP_Text TMPText;

    //[SerializeField] Scrollbar scrollbar;



    //Start is called before the first frame update
    //void Start()
    //{

    //}

    //public override void FixedUpdateNetwork()
    //{
    //    if (scrollbar.value != 0) scrollbar.value = 0;

    //}

    //static void OnChangeChatLog(Changed<ChatDisPlay1> changed)
    //{
    //    if (changed.Behaviour.nowString == "")
    //    {
    //        return;
    //    }
    //    changed.Behaviour.TMPText.text += "\n" + changed.Behaviour.nowString.ToString();
    //}




    //public void PushChatLog(Transform _name, NetworkString<_16> _chat)
    //{
    //    nowString = _chat;
    //    RPC_Chat(nowString.ToString());
    //}

    //public void RPC_Chat(string _nowString, RpcInfo info = default)
    //{
    //    this.nowString = _nowString;
    //    Debug.Log($"[RPC] SetNickname : {_nowString}");
    //    TMPText.text += "\n" + nowString.ToString();

    //}
}
