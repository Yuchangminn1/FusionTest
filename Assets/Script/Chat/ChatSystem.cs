using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class ChatSystem : NetworkBehaviour
{

    [Header("Objects")]
    public GameObject chatEntryCanvas;
    public TMP_InputField chatEntryInput;
    public TextMeshProUGUI chatBody;
    public GameObject chatDisplay;
    [HideInInspector] public static TextMeshProUGUI MyChatBody;


    [Header("Action References")]
    public InputActionReference startChat;
    public InputActionReference sendChat;

    [Header("Networked")]
    private GameObject placeholder;
    [Networked(OnChanged = nameof(LastPublicChatChanged))] public NetworkString<_256> LastPublicChat { get; set; }
    [Networked(OnChanged = nameof(LastPublicChatChanged))] public NetworkString<_256> LastPrivateChat { get; set; }

    private string thisPlayerName;

    private void Start()
    {
        if (HasStateAuthority)
        {
            startChat.action.performed += StartChat;
            sendChat.action.performed += SendChat;
            chatDisplay.gameObject.SetActive(true);
            MyChatBody = chatBody;
        }
        thisPlayerName = transform.root.GetComponent<NetworkCharacterControllerPrototypeCustom>().name;
        //이름 연결
    }

    protected static void LastPublicChatChanged(Changed<ChatSystem> change)
    {
        
        change.Behaviour.chatBody.text +="\n" + change.Behaviour.thisPlayerName +" : "+ change.Behaviour.LastPublicChat.ToString();


    }
    protected static void LastPrivateChatChanged(Changed<ChatSystem> change)
    {

    }

    private void SendChat(InputAction.CallbackContext obj)
    {
        LastPublicChat = chatEntryInput.text;
        chatEntryCanvas.SetActive(false);

    }

    private void StartChat(InputAction.CallbackContext obj)
    {
        chatEntryCanvas.SetActive(true);
        chatEntryInput.Select();
    }






}
