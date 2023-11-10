using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;


public class NetworkRunnerHandler : MonoBehaviour
{
    public NetworkRunner networkRunnerPrefab;
    


    NetworkRunner networkRunner;
    void Start()
    {
        //���� ��ȯ
        networkRunner = Instantiate(networkRunnerPrefab, Vector3.up*10f,Quaternion.identity);
        //�̸� �ٲ�
        networkRunner.name = "Network Runner";
        //��Ʈ��ũ ���� ���� 
        var clienTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
        Debug.Log("sever networkRunner started.");
        // 
        
    }


    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address ,SceneRef scene,Action<NetworkRunner> initialized)
    {
        //���� �Լ���� �����ϰ� �Ѿ 
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
        if(sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        runner.ProvideInput = true;
        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "TestRoom",
            Initialized = initialized,
            SceneManager = sceneManager
        }) ;
    }

}