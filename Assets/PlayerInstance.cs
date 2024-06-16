using Photon.Pun;
using UnityEngine;
using System.Collections;

public class PlayerInstance : Photon.Pun.MonoBehaviourPun
{
    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        var position = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
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
    IEnumerator WaitForInitialization(GameObject pobj, GameObject cobj)
    {
        yield return new WaitForSeconds(0.5f); // 適切な遅延時間を設定する                                             
        PlayerController playercon = pobj.GetComponent<PlayerController>();
        RayCastCS raycs = pobj.GetComponent<RayCastCS>();
        Transform playertrans = pobj.GetComponent<Transform>();
        PlayerCamera pcamsc = cobj.GetComponent<PlayerCamera>();
        Camera pcam = cobj.GetComponent<Camera>();
        raycs.cam = pcam;
        playercon.cloneCamera = pcam;
        raycs.playerController = playercon;
        pcamsc.target = playertrans;
        pcamsc.playerController = playercon;
    }
}
