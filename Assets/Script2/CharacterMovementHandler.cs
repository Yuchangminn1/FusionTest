using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public class CharacterMovementHandler : NetworkBehaviour
{
    public bool isRespawnRequsted = false;

    Camera localCamera;
    NetworkCharacterControllerPrototypeCustom networkCharacterControllerPrototypeCustom;
    HPHandler hpHandler;
    void Awake()
    {
        hpHandler = GetComponent<HPHandler>();
        networkCharacterControllerPrototypeCustom = GetComponent<NetworkCharacterControllerPrototypeCustom>();
        localCamera = GetComponentInChildren<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }


    //그냥 update 는 로컬로만 움직임 
    //서버에서 움직일려면 fixedUpdateNetwork
    public override void FixedUpdateNetwork()
    {
        //Don't update the clients position when they ard dead
        if (Object.HasInputAuthority)
        {
            if (isRespawnRequsted)
            {
                Respawn();
                return;
            }

            if (hpHandler.isDead)
                return;
        }
        
        //플레이어 이동 
        //get the input form the network
        if (GetInput(out NetworkInputData networkInputData))
        {
            //Rotate the transform according to the client aim vector
            transform.forward = networkInputData.aimFowardVector;
            //cancel out rotation on X axis as we don't want our chracter to tilt
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0,rotation.eulerAngles.y,rotation.eulerAngles.z);
            transform.rotation = rotation;
            
            //move
            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();
            
            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //Jump 
            if (networkInputData.isJumpButtonPressed)
            {
                networkCharacterControllerPrototypeCustom.Jump();
            }
            CheckFallRespawn();

        }

    }


    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
        {
            if (Object.HasInputAuthority)
            {
                Debug.Log("CheckFallRespawn() 함수로 호출되어 리스폰");

                Respawn();
            }
            
        }
    }

    public void RequestRespawn()
    {
        isRespawnRequsted = true;
    }

    void Respawn()
    {
        networkCharacterControllerPrototypeCustom.TeleportToPosition(Utils.GetRandomSpawnPoint());

        hpHandler.OnRespawned();

        isRespawnRequsted = false;
    }

    public void SetCharacterControllerEnabled(bool isEnabled)
    {
        //통제권 박탈 
        networkCharacterControllerPrototypeCustom.Controller.enabled = isEnabled;
    }


}
