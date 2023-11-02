using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;
public class ChatDisPlay1 : NetworkBehaviour
{

    //���� 1 ���� �Է°� ������� ä���Է� 
    //     2 ������ �ʿ��� �޼����� �Ⱥ��� 
    //     3 ���̵� �ڿ� ���� ������ ���̵�� ���� 
    [Networked(OnChanged = nameof(OnChangeChatLog))]
    public string nowString { get; set; }

    public string lastPushName;


    [SerializeField] TMP_Text TMPText;

    [SerializeField] Scrollbar scrollbar;



    // Start is called before the first frame update
    void Start()
    {

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
        changed.Behaviour.TMPText.text += "\n" +  changed.Behaviour.nowString;
    }




    public void PushChatLog(Transform _name, string _chat)
    {
        nowString = _chat;
        lastPushName = _name.name;
    }
}
