using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Insta : MonoBehaviour
{
    [SerializeField] GameObject reproductionObj;
    [SerializeField] RayCastCS RC;
    [SerializeField] Text rCount;
    public static int count = 5;
    private int limited = 0;

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
            if (count > limited)
            {
                rCount.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                if(Input.GetKeyDown(KeyCode.C))
                {
                    GameObject newObjct = Instantiate(reproductionObj, posi, transform.rotation);
                    count--;
                }
            }

            if (count == limited)
            {
                count = 0;
                rCount.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }

        }
    
    }
}
