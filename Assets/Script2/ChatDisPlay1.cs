using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
public class ChatDisPlay1 : NetworkBehaviour
{

    //문제 1 엔터 입력과 상관없이 채팅입력 
    //     2 참여자 쪽에서 메세지가 안보임 
    //     3 아이디가 뒤에 들어온 참가자 아이디로 변경 
    [Networked(OnChanged = nameof(OnChangeChatLog))]
    public string nowString { get; set; }

    public string lastPushName;


    [SerializeField] TMP_Text TMPText;

    [SerializeField] Scrollbar scrollbar;


    TMP_Text[] TMPTexts;

    // Start is called before the first frame update
    void Start()
    {
        AAAA();
    }

    public override void FixedUpdateNetwork()
    {
        if (scrollbar.value != 0) scrollbar.value = 0;

    }

    static void OnChangeChatLog(Changed<ChatDisPlay1> changed)
    {
        if(changed.Behaviour.nowString == "")
        {
            return;
        }
        string newM = changed.Behaviour.nowString;
        changed.LoadOld();
        string oldM = changed.Behaviour.nowString;
        changed.LoadNew();
        if(newM != oldM)
        {
            changed.Behaviour.BBBB(newM);
        }
    }

    public void BBBB(string Q)
    {
        int i = 0;
        foreach(var B in TMPTexts)
        {
             B.text += "\n" + $"{lastPushName}" + Q;
        }
    }

    public void AAAA()
    {
        GameObject[] Q = GameObject.FindGameObjectsWithTag("ChatChat");
        TMPTexts = new TMP_Text[Q.Length];
        int i = 0;
        foreach(GameObject B in Q)
        {
            TMPTexts[i] = B.GetComponent<TMP_Text>();
        }
    }

    public void PushChatLog(Transform _name, string _chat)
    {
        nowString = _chat;
        lastPushName = _name.name;
    }
}
