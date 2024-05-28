using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
public class Room : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI buttonText;

    private RoomInfo info;

    public void RegisterRoomDetails(RoomInfo info)
    {

        //���[�����̎擾�A�i�[
        this.info = info;
        //UI�\��
        buttonText.text = this.info.Name;
    }

    //���[���{�^�����Ǘ����Ă��郋�[���֎Q������
    //���[���Q���̊֐����Ăяo��
    public void Open_Room()
    {
        PhotonManager.instance.JoinRoom(info);
    }


}
