using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using TMPro;
using Fusion;
using System.Xml;

public class ChatSystem : NetworkBehaviour
{

    [Networked(OnChanged = nameof(OnChangeChatLog))]
    public NetworkString<_16> myChat { get; set; }

    public string sendPlayer;

    //public bool logChange = false;

    [SerializeField] InputField mainInputField;

    bool isEnter = false;
    bool chatOnOff = false;


    //bool repit = false;

    //Debug.Log($"Push {nowString}");
    //TMPText.text += nowString;

    public TMP_Text chatLog;
    public Scrollbar scrollV;

    bool chatDown = false;



    //클라이언트에서 쳇이 onoffonoff반복하는거 방지
    float inputTime = 0f;
    // Checks if there is anything entered into the input field.
    public void Start()
    {
        mainInputField.characterLimit = 1024;
        if (Object.HasInputAuthority)
        {
            mainInputField.enabled = true;
        }
        chatLog = GameObject.FindWithTag("ChatDisplay").GetComponent<TMP_Text>();
        scrollV = GameObject.FindWithTag("ScrollV").GetComponent<Scrollbar>();
        sendPlayer = PlayerPrefs.GetString("PlayerNickname");
    }

    //public override void FixedUpdateNetwork()
    //{
    //    if (Object.HasInputAuthority)
    //    {
    //        if (GetInput(out NetworkInputData networkInputData))
    //        {
    //            if (networkInputData.isRightEnterPressed)
    //            {
                    
    //                chatDown = true;
    //            }

    //        }
    //    }

    //}
    private void Update()
    {
        if (Object.HasInputAuthority)
        {
            if (Input.GetButtonDown("Submit"))
            {
                chatDown = true;
            }

            
            
        }
    }
    private void FixedUpdate()
    {
        if (chatDown)
        {
            Summit();
            Debug.Log("Enter로  Summit 실행");
            chatDown = false;
        }
    }
    private void Summit()
    {

        if (mainInputField.interactable)
        {
            if (mainInputField.text != "" && mainInputField.text != " ")
            {
                myChat = mainInputField.text;
                chatLog.text += $"\n {transform.name} : " + myChat;
                RPC_SetChat(myChat, sendPlayer);
                Debug.Log($"Send MyChat = {myChat}");
                mainInputField.text = "";
            }
            mainInputField.interactable = false;

        }
        else
        {
            Debug.Log("채팅 활성화");
            mainInputField.interactable = true;
            mainInputField.Select();
        }

    }

    //private void fixedUpdate()
    //{

    //    if (chatDown)
    //    {

    //        //scrollV.value = 0; 에를 네트워크로 해서 변경될 떄 함수 호출해보자 ㅇㅇ
    //        Debug.Log("chatDown");
    //        scrollV.value = 0;
    //        chatDown = false;
    //    }
    //}

    static void OnChangeChatLog(Changed<ChatSystem> changed)
    {
        Debug.Log("mychat = " + changed.Behaviour.myChat);
        if(changed.Behaviour.myChat== "")
        {
            return;
        }
        //changed.Behaviour.PushMessageName();

        changed.Behaviour.PushMessage();
        //Debug.Log("이름은 " +changed.Behaviour.transform.name);
    }


    public void PushMessage()
    {
        if (Object.HasInputAuthority)
        {
            return;
        }
        chatLog.text += $"\n {sendPlayer} : {myChat}";

        myChat = "";
    }
    public void PushMessageName()
    {
        if (Object.HasInputAuthority)
        {
            return;
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetChat(NetworkString<_16> mychat,string _sendPlayer, RpcInfo info = default)
    {
        sendPlayer = _sendPlayer;
        Debug.Log($"[RPC] SetNickname : {mychat}");
        this.myChat = mychat;
    }

    //IEnumerator ScrollDown()
    //{
    //    scrollV.value = 0;
    //    yield return null;

    //    while (scrollV.value != 0)
    //    {
    //        scrollV.value = 0;
    //        yield return null;

    //    }
    //}
}