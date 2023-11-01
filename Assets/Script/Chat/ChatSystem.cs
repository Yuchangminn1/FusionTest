using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using TMPro;
using Fusion;
using Unity.VisualScripting;
using System.Xml;

public class ChatSystem : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnChangeChatLog))]
    public string chatLogString { get; set; }

    //public string chatLog { get; set; }

    //public TMP_Text TMPText;
    public bool logChange = false;


    [SerializeField] GameObject scrollbarGO;
    [SerializeField] Scrollbar scrollbar;

    [SerializeField] InputField mainInputField;

    [SerializeField] GameObject disPlayGO;
    [SerializeField] TMP_Text disPlay;


    bool isSummit = false;
    bool repit = false;

    // Checks if there is anything entered into the input field.


    public void Start()
    {
        scrollbarGO = GameObject.FindWithTag("ScrollV");
        scrollbar = scrollbarGO.GetComponent<Scrollbar>();
        disPlayGO = GameObject.FindWithTag("ChatDisplay");
        disPlay = disPlayGO.GetComponent<TMP_Text>();


        mainInputField.characterLimit = 1024;
        if (Object.HasInputAuthority)
        {
            mainInputField.enabled = true;

        }

    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            if (GetInput(out NetworkInputData networkInputData))
            {

                if (networkInputData.isRightEnterPressed)
                {
                    Debug.Log("Enter 들어옴");

                    if (repit) SummitOff();

                    else SummitOn();

                }
            }
            if (scrollbar.value != 0)
            {
                scrollbar.value = 0;
            }
        }

    }

    private void SummitOff()
    {
        if (mainInputField.text != "" && mainInputField.text != " ")
        {
            chatLogString = mainInputField.text;
            Debug.Log($"chatLogString = {chatLogString}");
            Debug.Log("Send");
        }
        else
        {
            Debug.Log("null Send");
        }
        mainInputField.text = "";
        mainInputField.Select();
        repit = false;

    }

    private void SummitOn()
    {
        repit = true;
        mainInputField.Select();
        Debug.Log("Write");
        Debug.Log(Time.realtimeSinceStartupAsDouble);

    }

    /// <summary>
    /// //////////////////////////////////////////
    /// </summary>
    public void PushChatLog(Transform _name, string _chat)
    {
            if (disPlay != null)
            {
                disPlay.text += _name.name + _chat + "\n";
            }
        
    }
    static void OnChangeChatLog(Changed<ChatSystem> changed)
    {

        Debug.Log("ChatDis is Change ");

        string newchatLog = changed.Behaviour.chatLogString;
        if (newchatLog == "") //""로 초기화 하기 떄문에 피료없는 체인지는 리턴 
        {
            return;
        }
        else
        {

            changed.Behaviour.PushChatLog(changed.Behaviour.transform, newchatLog);
            changed.Behaviour.chatLogString = "";
        }

    }




}