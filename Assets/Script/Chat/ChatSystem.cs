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
    [SerializeField] TextMeshProUGUI chatDisPlay;
    [SerializeField] Scrollbar scroll;

    bool isEnter = false;
    bool isSummit = false;
    bool repit = false;

    [Networked(OnChanged = nameof(ChatUpdate))]
    public string chatLogString { get; set; }
    // Checks if there is anything entered into the input field.


    public void Start()
    {
        mainInputFieldGO = gameObject;
        mainInputField = mainInputFieldGO.GetComponent<InputField>();

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
    public void FixedUpdate()
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
            chatLogString = $"{Object.name}" + mainInputField.text + "\n";
            mainInputField.text = "";
        }
        mainInputField.ActivateInputField();
        isEnter = false;
        mainInputField.enabled = false;
    }

    private void SummitOn()
    {
        mainInputField.enabled = true;
        isEnter = true;
        mainInputField.characterLimit = 1024;

        mainInputField.Select();
    }


    static void ChatUpdate(Changed<ChatSystem> changed)
    {
        string NewString = changed.Behaviour.chatLogString;
        changed.LoadOld();
        string OldString = changed.Behaviour.chatLogString;

        if (NewString != OldString)
        {
            changed.LoadNew();
            changed.Behaviour.ChatON();
        }

    }
    void ChatON()
    {
        chatDisPlay.text += chatLogString;
        scroll.value = 0;
    }
}