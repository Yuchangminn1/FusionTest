using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// �÷��̾� ��ȯ�� �� ���� �߰��ϰ������ ����ٰ� �߰��ϼ���
/// </summary>
public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    public int mySNum = 0;
    //Ư�� �κ� �Ⱥ��̰�
    //public Transform playerModel;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority) // ���ӿ��� ������ �Ǹ� ? ������   > ������Ʈ�� �̵���ų �Ǹ�?  NetworkObject��ũ��Ʈ�� �ٿ��ָ� Ʈ��
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
            Canvas localCanvas = GetComponentInChildren<Canvas>();
            localCanvas.enabled = false;
            Debug.Log("spawned remote player");
        }
        //�̸� Clone���� ��ȣ�� �ϴ¹�
        string Q = $"{Object.Id}";
        transform.name = Q;
        Q =$"{Q[4]}";
        mySNum = int.Parse(Q);
        Debug.Log(mySNum);

        //ü�¹� ��Ƽ�� ���̰� �ҷ��� ��ȣ �Ѱǵ� �� �����ε� �𸣰ڴ� �𸣰� ��
    }
    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

}
