using Photon.Pun;
using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    private void Start()
    {
        GameObject player = PhotonNetwork.Instantiate("character", Vector3.zero, Quaternion.identity, 0);
        GameObject playercamera = PhotonNetwork.Instantiate("camera", Vector3.zero, Quaternion.identity, 0);
        PlayerController playerController = player.GetComponent<PlayerController>();
        RayCastCS rayCastCS = player.GetComponent<RayCastCS>();
        Transform playerTransform = player.GetComponent<Transform>();
        playerController.enabled = true;
        rayCastCS.enabled = true;
        PlayerCamera camera = playercamera.GetComponent<PlayerCamera>();
        Camera camera1 = playercamera.GetComponent<Camera>();
        rayCastCS.cam = camera1;
        camera.player = playerTransform;
        camera.enabled = true;
        Debug.Log("aaa");

    }

}
