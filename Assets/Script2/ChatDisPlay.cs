using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
public class ChatDisplay: NetworkBehaviour
{

    //���� 1 ���� �Է°� ������� ä���Է� 
    //     2 ������ �ʿ��� �޼����� �Ⱥ��� 
    //     3 ���̵� �ڿ� ���� ������ ���̵�� ���� 
    [Networked(OnChanged = nameof(OnChangeChatLog))]
    public string nowString { get; set; }

    public string lastPushName;


    TMP_Text TMPText;

    [SerializeField] Scrollbar scrollbar;

    // Start is called before the first frame update
    void Start()
    {
        TMPText = GetComponentInChildren<TMP_Text>();
    }

    public override void FixedUpdateNetwork()
    {
        if (scrollbar.value != 0) scrollbar.value = 0;

    }

    static void OnChangeChatLog(Changed<ChatDisplay> changed)
    {
        if (changed.Behaviour.nowString == "") //""�� �ʱ�ȭ �ϱ� ������ �Ƿ���� ü������ ���� 
        {
            return;
        }
        changed.Behaviour.TMPText.text += "\n" + $"{changed.Behaviour.lastPushName}" + changed.Behaviour.nowString;
    }



    public void PushChatLog(Transform _name, string _chat)
    {
        nowString = _chat;
        lastPushName = _name.name;
    }
}
