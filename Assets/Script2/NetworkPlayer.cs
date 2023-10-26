using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }


    //Ư�� �κ� �Ⱥ��̰�
    public Transform playerModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority) // ���ӿ��� ������ �Ǹ� ? ������  
        {
            Local = this;

            //Disable main camera ���� ī�޶� ����
            //Camera.main.gameObject.SetActive(false);

            Debug.Log("Spawned local player");
        }
        else
        {
            
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;

            AudioListener audioListner = GetComponentInChildren<AudioListener>();
            audioListner.enabled = false;

            Debug.Log("spawned remote player");
        }
        //�̸� Clone���� ��ȣ�� �ϴ¹�

        transform.name = $"P_{Object.Id}";
    }
    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

}
