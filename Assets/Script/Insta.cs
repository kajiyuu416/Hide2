using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insta : MonoBehaviour
{
    [SerializeField] GameObject reproductionObj;
    [SerializeField] float limited = 5;
    [SerializeField] float repCount = 0;
    [SerializeField] RayCastCS RC;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 posi = this.transform.position;
        reproductionObj = null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posi = this.transform.position;
        reproductionObj = RC.targetObj;

        if(reproductionObj!=null)
        {
            if (Input.GetKeyDown(KeyCode.C) && limited > repCount)
            {
                GameObject newObjct = Instantiate(reproductionObj, posi, transform.rotation);
                repCount++;
            }
        }
    }
}
