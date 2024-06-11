using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class stairs : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Rigidbody rigid = other.gameObject.GetComponent<Rigidbody>();
            rigid.drag = 5;
            Debug.Log("aaa");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Rigidbody rigid = other.gameObject.GetComponent<Rigidbody>();
            rigid.drag = 1;
            Debug.Log("bbb");
        }
    }
}
