using Photon.Pun;
using UnityEngine;
using System.Collections;

public class PlayerInstance : Photon.Pun.MonoBehaviourPun
{

    IEnumerator WaitForInitialization(GameObject pobj,GameObject cobj)
    {
        yield return new WaitForSeconds(0.5f); // 適切な遅延時間を設定する
                                               // GetComponentを実行する
        PlayerController playercon = pobj.GetComponent<PlayerController>();
        RayCastCS raycs = pobj.GetComponent<RayCastCS>();
        Transform playertrans = pobj.GetComponent<Transform>();

        PlayerCamera pcamsc = cobj.GetComponent<PlayerCamera>();
        Camera pcam = cobj.GetComponent<Camera>();

        raycs.cam = pcam;
        raycs.playerController = playercon;
        pcamsc.target = playertrans;
        pcamsc.playerController = playercon;

        if(playercon != null)
        {
            // 成功した場合の処理
            Debug.Log("playerconcomponent");
        }
        else
        {
            // 失敗した場合の処理
            Debug.Log("Noncomponent");
        }
        if(raycs != null)
        {
            // 成功した場合の処理
            Debug.Log("playerconcomponent");
        }
        else
        {
            // 失敗した場合の処理
            Debug.Log("Noncomponent");
        }
        if(playertrans != null)
        {
            // 成功した場合の処理
            Debug.Log("playerconcomponent");
        }
        else
        {
            // 失敗した場合の処理
            Debug.Log("Noncomponent");
        }

    }

    private void Start()
    {
        var position = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        //GameObject player = PhotonNetwork.Instantiate("character", position, Quaternion.identity);
        //PlayerController playerController = player.GetComponent<PlayerController>();
        //RayCastCS rayCastCS = player.GetComponent<RayCastCS>();
        //Transform playerTransform = player.GetComponent<Transform>();
        //GameObject camera = PhotonNetwork.Instantiate("camera", Vector3.zero, Quaternion.identity);
        // PlayerCamera pcamera = camera.GetComponent<PlayerCamera>();
        //Camera playercamera = camera.GetComponent<Camera>();
        //rayCastCS.cam = playercamera;
        //rayCastCS.playerController = playerController;
        //pcamera.target = playerTransform;
        //pcamera.playerController = playerController;
        PhotonNetwork.IsMessageQueueRunning = true;
        GameObject pobj = PhotonNetwork.Instantiate("character", position, Quaternion.identity);
        GameObject cobj = PhotonNetwork.Instantiate("camera", Vector3.zero, Quaternion.identity);
        StartCoroutine(WaitForInitialization(pobj,cobj));

        if(pobj.GetComponent<PhotonView>().IsMine)
        {
            pobj.SetActive(true);
        }

        if(cobj.GetComponent<PhotonView>().IsMine)
        {
            cobj.SetActive(true);
        }
    }

    private void Update()
    {
        foreach(var players in PhotonNetwork.PlayerList)
        {
            Debug.Log(players);
        }
    }

}
