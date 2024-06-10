using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class stairs : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();
            rigid.drag = 10;
            Debug.Log("aaa");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            Rigidbody rigid = collision.gameObject.GetComponent<Rigidbody>();
            rigid.drag = 1;
            Debug.Log("bbb");
        }
    }
}
