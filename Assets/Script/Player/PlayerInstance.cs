using Photon.Pun;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Photon.Realtime;

public class PlayerInstance : Photon.Pun.MonoBehaviourPun
{
    private GameManager gameManager;

    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        var position = new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y, transform.position.z + Random.Range(-3f, 3f));
        Quaternion currentRotation = transform.rotation;
        GameObject pobj = PhotonNetwork.Instantiate("character", position, currentRotation);
        GameObject cobj = PhotonNetwork.Instantiate("camera", Vector3.zero, currentRotation);
        gameManager = FindObjectOfType<GameManager>();
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

    IEnumerator WaitForInitialization(GameObject pobj, GameObject cobj)
    {
        yield return new WaitForSeconds(0.5f); // 遅延時間を設定する                                           
        PlayerController playercon = pobj.GetComponent<PlayerController>();
        RayCastCS raycs = pobj.GetComponent<RayCastCS>();
        Transform playertrans = pobj.GetComponent<Transform>();
        PlayerCamera pcamsc = cobj.GetComponent<PlayerCamera>();
        Camera pcam = cobj.GetComponent<Camera>();
        pobj.GetPhotonView().RPC("SetName", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        gameManager.playerController = playercon;
        raycs.cam = pcam;
        playercon.cloneCamera = pcam;
        raycs.playerController = playercon;
        pcamsc.target = playertrans;
        pcamsc.playerController = playercon;
    }
}
