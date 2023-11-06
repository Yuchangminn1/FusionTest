using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

/// <summary>
/// �÷��̾� ��ȯ�� �� ���� �߰��ϰ������ ����ٰ� �߰��ϼ���
/// </summary>
public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public TextMeshProUGUI playerNickNameTM;
    public static NetworkPlayer Local { get; set; }
    public int mySNum = 0;

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName { get; set; }



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

            RPC_SetNickName(PlayerPrefs.GetString("PlayerNickname"));
            Debug.Log("Spawned local player");
        }
        else
        {
            //RPC_SetNickNameTOIn(PlayerPrefs.GetString("PlayerNickname"));
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;

            AudioListener audioListner = GetComponentInChildren<AudioListener>();
            audioListner.enabled = false;
            Canvas[] localCanvas = GetComponentsInChildren<Canvas>();
            foreach(Canvas c in localCanvas)
            {
                if(c.gameObject.tag == "Nickname")
                {
                    continue;
                }
                c.enabled = false;
            }

            Debug.Log("spawned remote player");
        }
        //�̸� Clone���� ��ȣ�� �ϴ¹�
        //string Q = $"{Object.Id}";
        //transform.name = PlayerPrefs.GetString("PlayerNickname");
        //Q =$"{Q[4]}";
        //mySNum = int.Parse(Q);
        //Debug.Log(mySNum);

        //ü�¹� ��Ƽ�� ���̰� �ҷ��� ��ȣ �Ѱǵ� �� �����ε� �𸣰ڴ� �𸣰� ��
    }
    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"{Time.time} OnHPChanged value {changed.Behaviour.nickName}");
        changed.Behaviour.OnNickNameChanged();
    }
    private void OnNickNameChanged()
    {
        Debug.Log($"Nickname chaged for player to {nickName} for player {gameObject.name}");

        playerNickNameTM.text = nickName.ToString();
    }

    [Rpc(RpcSources.InputAuthority,RpcTargets.StateAuthority)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickname : {nickName}");
        this.nickName = nickName;
    }
    //[Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    //public void RPC_SetNickNameTOIn(string nickName, RpcInfo info = default)
    //{
    //    Debug.Log($"[RPC] SetNickname : {nickName}");
    //    this.nickName = nickName;
    //}
}
