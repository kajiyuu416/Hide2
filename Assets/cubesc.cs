using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubesc : MonoBehaviour
{
    public BoxCollider boxcol;
    public capselsc capselsc;
    public CapsuleCollider capsulecol;
    // Start is called before the first frame update
    void Start()
    {
        boxcol = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(boxcol);
        Debug.Log(capselsc);
        Debug.Log(capsulecol);
    }
}
