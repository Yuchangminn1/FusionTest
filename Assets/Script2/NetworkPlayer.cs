using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// 플레이어 소환할 때 뭔가 추가하고싶으면 여기다가 추가하세용
/// </summary>
public class NetworkPlayer : NetworkBehaviour,IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }
    public int mySNum = 0;
    //특정 부분 안보이게
    //public Transform playerModel;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void Spawned()
    {
        if (Object.HasInputAuthority) // 게임에서 움직일 권리 ? 같은거   > 오브젝트를 이동시킬 권리?  NetworkObject스크립트를 붙여주면 트루
        {
            Local = this;

            //Disable main camera 메인 카메라 제거
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
        //이름 Clone말고 번호로 하는법
        string Q = $"{Object.Id}";
        transform.name = Q;
        Q =$"{Q[4]}";
        mySNum = int.Parse(Q);
        Debug.Log(mySNum);

        //체력바 파티로 보이게 할려고 번호 한건디 흠 별로인듯 모르겠다 모르겠 어
    }
    public void PlayerLeft(PlayerRef player)
    {
        if(player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }

}
