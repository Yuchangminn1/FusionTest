using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

public class CharacterMovementHandler : NetworkBehaviour
{

    //[Networked(OnChanged = nameof(OnFireNumChanged))]
    //�� ��Ʈ��ũ Ŭ������ 
    //public NetworkedAttribute()
    //{
    //    OnChangedTargets = OnChangedTargets.All;
    //}
    //�� ���� �������ִ°� 
    //������Ƽ ����� ���� ������ 
    //int fireNum { get; set; }

    //TextMeshPro textMeshPro;


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
        //textMeshPro = GetComponentInChildren<TextMeshPro>();

    }


    //�׳� update �� ���÷θ� ������ 
    //�������� �����Ϸ��� fixedUpdateNetwork
    public override void FixedUpdateNetwork()
    {
        
        if (HasStateAuthority)
        {
            //��ȣ�ۿ� ������ ������
            if (isRespawnRequsted)
            {
                //�������� Ʈ���̸� ���������Ѷ�
                Respawn();
                return;
            }
            //���� ���¸� ���� ������ ���� ���� 
            if (hpHandler.isDead)
                return;
        }
        



        //Don't update the clients position when they ard dead
        

        //�÷��̾� �̵� 
        //get the input form the network

        //InputHandler ���� ������ ���� ���⼭ �޴°����� ���� 

        //�� ��Ʈ��ũ���� ���� ������ ������� ������ �ϴ� �κ� 
        //"���� �ùķ��̼� ƽ�� ���� ��ȿ�� Fusion.INetworkInput�� ã�� �� �ִٸ� true�� ��ȯ�մϴ�.
        //��ȯ�� �Է� ����ü�� Fusion.NetworkObject.InputAuthority���� �����ϸ�,
        // ��ȿ�� ��� ���� �ùķ��̼� ƽ�� ���� �ش� Fusion.PlayerRef�� ������ �Է��� �����մϴ�
        //GetInput >> NetworkBehavior �� �ִ� �Լ�
        if (GetInput(out NetworkInputData networkInputData))
        {

            //Debug.Log($" has ���ñ�{transform.name} = " + networkCharacterControllerPrototypeCustom.Object.HasInputAuthority);w

            //Rotate the transform according to the client aim vector
            transform.forward = networkInputData.aimFowardVector;
            //cancel out rotation on X axis as we don't want our chracter to tilt
            //transform >> ������ ĳ���� ����


            //x�� ȸ���� �����ϴ� ��� rigidbody �����̶� ���ٰ� ����������

            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z);
            transform.rotation = rotation;


            //move input�� �޾ƿ°ɷ� ���

            Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            moveDirection.Normalize();

            networkCharacterControllerPrototypeCustom.Move(moveDirection);

            //Debug.Log("moveDirection = " + moveDirection);
            //Jump 
            if (networkInputData.isJumpButtonPressed)
            {
                networkCharacterControllerPrototypeCustom.Jump();
            }
            //if (networkInputData.isFireButtonPressed)
            //{
            //    ++fireNum;
            //    networkInputData.fireNum = fireNum;
            //}
            CheckFallRespawn();

            //�̻��ϰ� �������� ���������� ������

        }

    }

    /// <summary>
    /// �̻��ϰ� �������� ���������� ������
    /// </summary>
    void CheckFallRespawn()
    {
        if (transform.position.y < -12)
        {
            if (Object.HasStateAuthority)
            {
                Debug.Log("CheckFallRespawn() �Լ��� ȣ��Ǿ� ������");

                Respawn();
            }

        }
    }

    /// <summary>
    /// isRespawnRequsted = ture
    /// </summary>

    public void RequestRespawn()
    {
        isRespawnRequsted = true;

    }
    /// <summary>
    /// �����̵� ��
    /// hpHandler.OnRespawned();
    /// isRespawnRequsted = false;
    /// </summary>
    void Respawn()
    {
        SetCharacterControllerEnabled(true);
        networkCharacterControllerPrototypeCustom.TeleportToPosition(Utils.GetRandomSpawnPoint());

        hpHandler.OnRespawned();

        isRespawnRequsted = false;
    }
    /// <summary>
    /// CharacterController bool������ Ű�� ���� 
    /// </summary>
    public void SetCharacterControllerEnabled(bool isEnabled)
    {
        networkCharacterControllerPrototypeCustom.Controller.enabled = isEnabled;
    }

    //ONchage ������ ����ƽ���� ����ؾ���
    //�� �Լ��� ��� �׻� ����ǰ� �ִ°Ű� �� ��ȭ����  �ٸ��� if���� Ÿ�� �Ʒ� �Լ��� �����Ű�°��� ���� 
    //static void OnFireNumChanged(Changed<CharacterMovementHandler> changed)
    //{

    //    int fireNumCurrent = changed.Behaviour.fireNum;
    //    //Load the old value
    //    changed.LoadOld();
    //    int fireNumOld = changed.Behaviour.fireNum;

    //    if (fireNumCurrent != fireNumOld)
    //        changed.Behaviour.ChangeUItextFireNum(fireNumCurrent);

    //}

    //void ChangeUItextFireNum(int num)
    //{
    //    if (textMeshPro != null)
    //        textMeshPro.text = $"{num}";
    //}

}
