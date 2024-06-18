using Photon.Pun;
using UnityEngine;

public class cube : MonoBehaviourPun
{
    // �����������J���[
    private Color syncedColor;

    // �J���[�ύX��RPC���\�b�h
    [PunRPC]
    void ChangeColorRPC(Color newColor)
    {
        // �I�u�W�F�N�g�ɓK�p����Ă���Renderer���擾����
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        // Renderer�����݂��A�}�e���A�������݂���ꍇ�́A�J���[��ύX����
        if(renderer != null && renderer.material != null)
        {
            renderer.material.color = newColor;
        }
    }
    // �O�����炱�̃��\�b�h���Ăяo���ăJ���[�̕ύX��RPC�œ�������
    public void ChangeColor(Color newColor)
    {
        // RPC���g���đS�ẴN���C�A���g�ɃJ���[�ύX��ʒm����
        photonView.RPC("ChangeColorRPC", RpcTarget.All, newColor);
    }
}
