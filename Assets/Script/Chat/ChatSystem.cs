using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using TMPro;
using Fusion;
using System.Xml;

public class ChatSystem : NetworkBehaviour
{

    [Networked(OnChanged = nameof(OnChangeChatLog))]
    public NetworkString<_16> mychat { get; set; }

    //public bool logChange = false;

    [SerializeField] InputField mainInputField;

    bool isOnoff = false;

    bool chatOn = false;
    bool chatOff = false;


    bool repit = false;

    //Debug.Log($"Push {nowString}");
    //TMPText.text += nowString;

    public TMP_Text chatLog;

    string myChat;

    // Checks if there is anything entered into the input field.


    public void Start()
    {
        mainInputField.characterLimit = 1024;
        if (Object.HasInputAuthority)
        {
            mainInputField.enabled = true;
        }
        chatLog = GameObject.FindWithTag("ChatDisplay").GetComponent<TMP_Text>();

    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            if (GetInput(out NetworkInputData networkInputData))
            {

                if (networkInputData.isRightEnterPressed)
                {
                    Debug.Log("Enter µé¾î¿È");

                    isOnoff = !isOnoff;
                    if (isOnoff) chatOn = true;
                    else chatOff = true;

                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (chatOn) SummitOn();
        else if (chatOff) SummitOff();

    }
    private void SummitOff()
    {
        chatOff = false;
        if (mainInputField.text != "" && mainInputField.text != " ")
        {
            myChat = mainInputField.text;

            RPC_SetChat(myChat);
            Debug.Log($"Send MyChat = {myChat}");

        }
        else
        {
            Debug.Log("null Send");
        }
        mainInputField.text = "";
        mainInputField.Select();
    }

    private void SummitOn()
    {
        chatOn = false;
        mainInputField.Select();
        Debug.Log("Write");

    }

    static void OnChangeChatLog(Changed<ChatSystem> changed)
    {
        changed.Behaviour.PushMessage();
    }


    public void PushMessage()
    {
        chatLog.text += "\n" + mychat;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetChat(string mychat, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickname : {mychat}");
        this.mychat = mychat;
    }
}