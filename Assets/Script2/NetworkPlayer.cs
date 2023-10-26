using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }


    //특정 부분 안보이게
    public Transform playerModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            Local = this;
            //특정부분 안보이게
           // Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            //Disable main camera 메인 카메라 제거
            //Camera.main.gameObject.SetActive(false);

            Debug.Log("Spawned local player");
        }
        else
        {
            //다른 플레이어가 접속했을 때 카메라 따로 설정
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;

            AudioListener audioListner = GetComponentInChildren<AudioListener>();
            audioListner.enabled = false;

            Debug.Log("spawned remote player");
        }
        //이름 Clone말고 번호로 하는법

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
