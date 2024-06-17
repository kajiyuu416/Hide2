using Photon.Pun;
using UnityEngine;
using System.Collections;
public class cube : Photon.Pun.MonoBehaviourPun
{

    Color co = new Color(150, 150, 150);


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeColor(co);
        }
    }

    // Start is called before the first frame update
    public void SetColor(Color newColor)
    {
        // �����ŃJ���[��ݒ肷�郍�W�b�N���L�q����
        GetComponent<Renderer>().material.color = newColor;
    }
    [PunRPC]
    public void RPC_SetColor(Color newColor)
    {
        // RPC ���\�b�h�ŃJ���[��ݒ�
        SetColor(newColor);
    }
    public void ChangeColor(Color newColor)
    {
        // ���[�J���ł̃J���[�ύX
        SetColor(newColor);
        // RPC ���g���đ��̃v���C���[�ɂ��ύX��ʒm
        photonView.RPC("RPC_SetColor", RpcTarget.AllBuffered, newColor);
        Debug.Log("change");
    }
}
