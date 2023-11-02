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

    // Checks if there is anything entered into the input field.


    public void Start()
    {
        mainInputField.characterLimit = 1024;
        if (Object.HasInputAuthority)
        {
            mainInputField.enabled = true;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority )
        {
            if (GetInput(out NetworkInputData networkInputData))
            {

                if (networkInputData.isRightEnterPressed)
                {
                    Debug.Log("Enter µé¾î¿È");

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
            Debug.Log("Send");
            chatDisplay.PushChatLog(transform, mainInputField.text);
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