using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using TMPro;
using Fusion;
using Unity.VisualScripting;

public class ChatSystem : NetworkBehaviour
{
    [SerializeField] GameObject mainInputFieldGO;
    [SerializeField] InputField mainInputField;

   // [SerializeField] Scrollbar scroll;

    [SerializeField] ChatDisPlay chatDisPlay;
    bool isEnter = false;
    bool isSummit = false;
    bool repit = false;
    
    public string chatLogString { get; set; }
    // Checks if there is anything entered into the input field.


    public void Start()
    {
        mainInputFieldGO = gameObject;
        mainInputField = mainInputFieldGO.GetComponent<InputField>();
        chatDisPlay = GameObject.FindGameObjectWithTag("ChatDisplay").GetComponent<ChatDisPlay>();
        mainInputField.characterLimit = 1024;

    }
    public void Update()
    {

        if (!isSummit && Input.GetButtonDown("Submit"))
        {
            isSummit = true;
            repit = false;
        }
        else if (isSummit && Input.GetButtonDown("Submit"))
        {
            isSummit = false;
            repit = false;

        }

    }
    public override void FixedUpdateNetwork()
    {
        if (repit)
            return;
        if (isSummit)
        {
            SummitOn();
            repit = true;
        }
        if (!isSummit)
        {
            SummitOff();
            repit = true;
        }
    }

    private void SummitOff()
    {
        if (mainInputField.text != "")
        {
            chatDisPlay.PushChatLog(Object.name,mainInputField.text);
        }
        mainInputField.text = "";
        chatLogString = "";
        mainInputField.ActivateInputField();
        isEnter = false;
        mainInputField.enabled = false;
    }

    private void SummitOn()
    {
        mainInputField.enabled = true;
        isEnter = true;
        mainInputField.Select();
    }


    //static void ChatUpdate(Changed<ChatSystem> changed)
    //{

    //    string NewString = changed.Behaviour.chatLogString;
    //    if(NewString == "")
    //    {
    //        return;
    //    }
    //    changed.LoadOld();
    //    string OldString = changed.Behaviour.chatLogString;

    //    if (NewString != OldString)
    //    {
    //        changed.LoadNew();
    //        changed.Behaviour.chatDisPlay.PushChatLog(NewString);
    //        changed.Behaviour.chatLogString = "";
    //    }

    //}
    //void ChatPush()
    //{
    //    chatDisPlay.text += chatLogString;
    //    chatLogString = "";
    //}
}