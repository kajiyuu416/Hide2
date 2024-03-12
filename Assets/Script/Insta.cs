using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Insta : MonoBehaviour
{
    [SerializeField] GameObject reproductionObj;
    [SerializeField] float limited = 5;
    [SerializeField] RayCastCS RC;
    [SerializeField] Text rCount;
    public static int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 posi = this.transform.position;
        reproductionObj = null;
        rCount.text = count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posi = this.transform.position;
        reproductionObj = RC.targetObj;
        rCount.text = count.ToString();
        if (reproductionObj!=null)
        {
            if (Input.GetKeyDown(KeyCode.C) && limited > count)
            {
                GameObject newObjct = Instantiate(reproductionObj, posi, transform.rotation);
                count++;
            }
        }
    }
}
