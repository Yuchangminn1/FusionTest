using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputhandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isFireButtonPressed = false;
    bool isRightEnterPressed = false;
    int fireNum;

    CharacterMovementHandler characterMovementHandler;
    //other components
    LocalCameraHandler localCameraHandler;

    void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!characterMovementHandler.Object.HasInputAuthority)
            return;

        //View input
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") *-1; //Invert the mouse look

        //인풋을 수집  move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");


        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump!!!");

            isJumpButtonPressed = true;
        }
        if (Input.GetButtonDown("Fire1")) 
        {

            isFireButtonPressed = true;
            ++fireNum;
        }
        if (Input.GetButtonDown("Submit"))
        {
            isRightEnterPressed = true;
        }
        localCameraHandler.SetViewInputVector(viewInputVector);

        

    }

    public NetworkInputData GetNetworkInput()
    {
        //플레이어의 전반적인 인풋 전송

        //네트워크 데이터로 전송 
        NetworkInputData networkInputData = new NetworkInputData();

        //View data
         //networkInputData.rotationInput = viewInputVector.x;
        networkInputData.aimFowardVector = localCameraHandler.transform.forward;
        //Move data
        networkInputData.movementInput = moveInputVector;

        networkInputData.isJumpButtonPressed = isJumpButtonPressed;

        networkInputData.isFireButtonPressed = isFireButtonPressed;

        networkInputData.isRightEnterPressed = isRightEnterPressed;


        networkInputData.fireNum = fireNum;

        isJumpButtonPressed = false;
        isFireButtonPressed = false;
        isRightEnterPressed = false;
        return networkInputData;

    }


}
