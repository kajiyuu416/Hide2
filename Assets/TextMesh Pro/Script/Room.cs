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

        //ルーム情報の取得、格納
        this.info = info;
        //UI表示
        buttonText.text = this.info.Name;
    }

    //ルームボタンが管理しているルームへ参加する
    //ルーム参加の関数を呼び出す
    public void Open_Room()
    {
        PhotonManager.instance.JoinRoom(info);
    }


}
