using Photon.Pun;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    private void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        GameObject player = PhotonNetwork.Instantiate("character",Vector3.zero,Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();
        RayCastCS rayCastCS = player.GetComponent<RayCastCS>();
        Transform playerTransform = player.GetComponent<Transform>();
        GameObject camera = PhotonNetwork.Instantiate("camera",Vector3.zero,Quaternion.identity);
        PlayerCamera pcamera = camera.GetComponent<PlayerCamera>();
        Camera camera1 = camera.GetComponent<Camera>();
        rayCastCS.cam = camera1;
        rayCastCS.playerController = playerController;
        pcamera.target = playerTransform;
        pcamera.playerController = playerController;
        playerController.enabled = true;
        rayCastCS.enabled = true;
        pcamera.enabled = true;
    }

}
