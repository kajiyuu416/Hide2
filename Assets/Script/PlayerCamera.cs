using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float DistanceToPlayerM = 2.0f;    // �J�����ƃv���C���[�Ƃ̋���[m]
    [SerializeField] float SlideDistanceM = 0.0f;       // �J���������ɃX���C�h������G�v���X�̎��E�ցC�}�C�i�X�̎�����[m]
    [SerializeField] float HeightM = 1.2f;            // �����_�̍���[m]
    [SerializeField] float RotationSensitivity = 100.0f;// ���x
    [SerializeField] float UpperLimit = 0.3f;
    [SerializeField] float LowerLimit = -0.8f;
    [SerializeField] RayCastCS RC;

    GameObject targetObj;
    Vector3 targetPos;


    void Start()
    {
        if (Target == null)
        {
            Debug.LogError("�^�[�Q�b�g���ݒ肳��Ă��Ȃ�");
            Application.Quit();
        }
        targetObj = GameObject.Find("Player1");
        targetPos = targetObj.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;
        var rotX = Input.GetAxis("Mouse X") * Time.deltaTime * RotationSensitivity;
        var rotY = Input.GetAxis("Mouse Y") * Time.deltaTime * RotationSensitivity;
        var lookAt = Target.position + Vector3.up * HeightM;
        // ��]
        transform.RotateAround(lookAt, Vector3.up, rotX);
        // �J�������v���C���[�̐^���^���ɂ���Ƃ��ɂ���ȏ��]�����Ȃ��悤�ɂ���
        if (transform.forward.y > UpperLimit && rotY < 0)
        {
            rotY = 0;
        }
        if (transform.forward.y < LowerLimit && rotY > 0)
        {
            rotY = 0;
        }
        transform.RotateAround(lookAt, transform.right, rotY);

        // �J�����ƃv���C���[�Ƃ̊Ԃ̋����𒲐�
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // �����_�̐ݒ�
        transform.LookAt(lookAt);

        // �J���������ɂ��炵�Ē������J����
        transform.position = transform.position + transform.right * SlideDistanceM;
        if (RC.metamorphosisFlag)
        {
            DistanceToPlayerM = 8.0f;
        }

    }
}
