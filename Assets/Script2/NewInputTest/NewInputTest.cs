using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputTest : MonoBehaviour
{

    public PlayerInputAction playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction fire;


    bool isis = false;
    Vector2 moveDirection = Vector2.zero;
    Vector2 dir;
    private void Awake()
    {
        playerControls = new PlayerInputAction();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        Debug.Log("move Set");
        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;
    }
    private void OnDisable()
    {
        move.Disable();
        fire.Disable();
        jump.Disable();

    }


    // Update is called once per frame

    void Update()
    {
        dir = move.ReadValue<Vector2>();
        Debug.Log(dir);


        //Debug.Log(" ddddddd" + fire.ReadValue<bool>());
        //if (fire.ReadValue<bool>())
        //{
        //    Debug.Log("Fire ON" + fire.ReadValue<bool>());
        //    Debug.Log("Fire ON" + fire.ReadValue<bool>());

        //}
        //if (dir == Vector2.zero && moveDirection == Vector2.zero)
        //    return;
        //moveDirection = Vector2.Lerp(moveDirection, dir, 0.15f);
        //moveDirection.x = MYCut(moveDirection.x);
        //moveDirection.y = MYCut(moveDirection.y);


        //Debug.Log(moveDirection);
    }
    private float MYCut(float _float)
    {
        if (Mathf.Abs(_float) > 0.9f)
            _float = 1 * _float/ Mathf.Abs(_float);
        else if (Mathf.Abs(_float) < 0.15)
            _float = 0;
        return _float;
    }

    //public void OnMove(InputValue value)
    //{
    //    Debug.Log("move È£Ãâ");
    //    dir = value.Get<Vector2>();

    //}
    //public void OnJump()
    //{
    //    Debug.Log("Jump Jump Jump");

    //}
    public void Fire(InputAction.CallbackContext context)
    {
        Debug.Log("Fire Fire Fire");

    }
    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Jump Jump");

    }
}
