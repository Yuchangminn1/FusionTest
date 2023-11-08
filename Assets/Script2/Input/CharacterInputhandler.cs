using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputhandler : MonoBehaviour
{
    //New Input System
    public PlayerInputAction playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction fire;
    private InputAction look;

    Vector2 moveDirection = Vector2.zero;
    Vector2 dir;
    Vector2 lookVec = Vector2.zero;


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
        playerControls = new  PlayerInputAction();

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
        lookVec = Vector2.Lerp(lookVec, look.ReadValue<Vector2>(), 0.2f);

        Debug.Log($"lookVec  = " + lookVec);
        //View input camera 
        //viewInputVector.x = Input.GetAxis("Mouse X");
        //viewInputVector.y = Input.GetAxis("Mouse Y") * -1; //Invert the mouse look
        viewInputVector.x = lookVec.x;
        viewInputVector.y = lookVec.y * -1; //Invert the mouse look
        ////��ǲ�� ����  move input
        //moveInputVector.x = Input.GetAxis("Horizontal");
        //moveInputVector.y = Input.GetAxis("Vertical");


        //if (Input.GetButtonDown("Jump"))
        //{
        //    Debug.Log("Jump!!!");

        //    isJumpButtonPressed = true;
        //}
        //if (Input.GetButtonDown("Fire1"))
        //{

        //    isFireButtonPressed = true;
        //    ++fireNum;
        //}
        //if (Input.GetButtonDown("Submit"))
        //{
        //    Debug.Log("Right Enter!!!");

        //    isRightEnterPressed = true;
        //}
        localCameraHandler.SetViewInputVector(viewInputVector);



    }
    void FixedUpdate()
    {
        if (!characterMovementHandler.Object.HasInputAuthority)
            return;
        dir = Vector2.Lerp(dir, move.ReadValue<Vector2>(), 0.2f);

        dir.x = MYCut(dir.x);
        dir.y = MYCut(dir.y);
        moveInputVector.x = dir.x;
        moveInputVector.y = dir.y;
        //Debug.Log(dir);

    }
    public NetworkInputData GetNetworkInput()
    {
        //�÷��̾��� �������� ��ǲ ����

        //��Ʈ��ũ �����ͷ� ���� 
        NetworkInputData networkInputData = new NetworkInputData();

        //View data
         //networkInputData.rotationInput = viewInputVector.x;
        networkInputData.aimFowardVector = localCameraHandler.transform.forward;
        //Move data
        networkInputData.movementInput = moveInputVector;

        networkInputData.isJumpButtonPressed = isJumpButtonPressed;

        networkInputData.isFireButtonPressed = isFireButtonPressed;

        


        networkInputData.fireNum = fireNum;

        isJumpButtonPressed = false;
        isFireButtonPressed = false;
        isRightEnterPressed = false;
        return networkInputData;

    }
    //New Input System
    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        look = playerControls.Player.Look;
        look.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();

        fire = playerControls.Player.Fire;
        fire.Enable();

        jump.performed += Jump;
        fire.performed += Fire;
    }
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        jump.Disable();
        look.Disable();


    }
    public void Fire(InputAction.CallbackContext context)
    {
        isFireButtonPressed = true;
        ++fireNum;

    }
    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump!!!");

        isJumpButtonPressed = true;
    }

    private float MYCut(float _float)
    {
        //input xy ���� getaxisȭ �����ٷ��� �غ��� �ʹ� ���� ��ȭ�� �׳� ���� ������� 
        if (Mathf.Abs(_float) > 0.9f)
            _float = 1 * _float / Mathf.Abs(_float);
        else if (Mathf.Abs(_float) < 0.1f&& move.ReadValue<Vector2>() == Vector2.zero) 
            _float = 0;
        return _float;
    }
}
