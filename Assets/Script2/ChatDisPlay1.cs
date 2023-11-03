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
    public NetworkString<_16> nowString { get; set; }

    public string lastPushName;


    public TMP_Text TMPText;

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
        changed.Behaviour.TMPText.text += "\n" +  changed.Behaviour.nowString.ToString();
    }


    // [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Chat(string nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] Chat : {nowString}");
        this.nowString = nowString;
    }

    public void PushChatLog(Transform _name, NetworkString<_16> _chat)
    {
        nowString = _chat;
        lastPushName = _name.name;
        RPC_Chat(_chat.ToString());
    }
}
