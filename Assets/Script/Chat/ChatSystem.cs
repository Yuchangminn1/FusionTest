using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using TMPro;
using Fusion;
using Unity.VisualScripting;
using System.Xml;

public class ChatSystem : NetworkBehaviour
{
    public bool logChange = false;

    [SerializeField] InputField mainInputField;

    [SerializeField] TMP_Text nickNameText;

    [SerializeField] ChatDisPlay1 chatDisplay;

    bool isSummit = false;
    bool repit = false;
    [Networked(OnChanged = nameof(ONCQ))]
    public NetworkString<_16> myChat { get; set; }


    // Checks if there is anything entered into the input field.


    public void Start()
    {
        mainInputField.characterLimit = 1024;
        if (Object.HasInputAuthority)
        {
            mainInputField.enabled = true;
        }


        chatDisplay = GameObject.FindGameObjectWithTag("ChatDisplay").GetComponent<ChatDisPlay1>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority )
        {
            if (GetInput(out NetworkInputData networkInputData))
            {

                if (networkInputData.isRightEnterPressed)
                {
                    Debug.Log("Enter ����");

                    if (repit) SummitOff();

                    else SummitOn();

                }
            }
        }
    }
    private void SummitOff()
    {
        repit = false;
        if (mainInputField.text != "" && mainInputField.text != " ")
        {
            myChat = mainInputField.text ;
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
        repit = true;
        mainInputField.Select();
        Debug.Log("Write");
        Debug.Log(Time.realtimeSinceStartupAsDouble);

    }
    static void ONCQ(Changed<ChatSystem> changed)
    {
        if (changed.Behaviour.myChat == "")
        {
            return;
        }
        NetworkString<_16> newM = changed.Behaviour.myChat;
        changed.LoadOld();
        NetworkString<_16> oldM = changed.Behaviour.myChat;
        changed.LoadNew();
        if (newM != oldM)
        {
            changed.Behaviour.PushMessage();

        }
    }
    public void PushMessage()
    {
        chatDisplay.PushChatLog(transform, myChat);
        Debug.Log($"Push {myChat}");
    }
    /// <summary>
    /// //////////////////////////////////////////
    /// </summary>


    //public void PushChatLog()
    //{
    //    if (disPlay != null)
    //    {
    //        disPlay.text += transform.name + chatLogString + "\n";
    //        nickNameText.text  = transform.name + chatLogString + "\n";
    //    }
    //}

}