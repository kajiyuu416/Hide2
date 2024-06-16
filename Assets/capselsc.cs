using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capselsc : MonoBehaviour
{
    public CapsuleCollider capsulecol;
    public cubesc cubesc;
    public BoxCollider boxcol;
    // Start is called before the first frame update
    void Start()
    {
        capsulecol = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(capsulecol);
        Debug.Log(cubesc);
        Debug.Log(boxcol);
    }
}
