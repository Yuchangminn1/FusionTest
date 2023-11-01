using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
public class ChatDisPlay : NetworkBehaviour
{

    //����                                            1 ���� �Է°� ������� ä���Է� 
    //     2 ������ �ʿ��� �޼����� �Ⱥ��� 
    //     3 ���̵� �ڿ� ���� ������ ���̵�� ���� 
    [Networked(OnChanged = nameof(OnChangeChatLog))]
    public string chatLog { get; set; }

    TMP_Text TMPText;

    [SerializeField] Scrollbar scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        TMPText = GetComponentInChildren<TMP_Text>();
    }

    public override void FixedUpdateNetwork()
    {
        if (scrollbar.value != 0)
        {
            scrollbar.value = 0;
        }
    }

    static void OnChangeChatLog(Changed<ChatDisPlay> changed)
    {
        string newchatLog = changed.Behaviour.chatLog;
        if (newchatLog == "") //""�� �ʱ�ȭ �ϱ� ������ �Ƿ���� ü������ ���� 
        {
            return;
        }
        //Load the old value
        changed.LoadOld();

        string oldchatLog = changed.Behaviour.chatLog;

        changed.Behaviour.TMPText.text += newchatLog;
        changed.Behaviour.RPC(changed.Behaviour.TMPText);

    }

    public void RPC(TMP_Text text)
    {
        TMPText.text = text.text;
    }

    public void PushChatLog(string _name, string _chat)
    {
        chatLog = _name + _chat + "\n";

    }
}
