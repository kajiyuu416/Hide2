using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Insta : MonoBehaviour
{
    [SerializeField] GameObject reproductionObj;
    [SerializeField] RayCastCS rayCastCS;
    [SerializeField] TextMeshProUGUI duplicate;
    [SerializeField] TextMeshProUGUI rCount;
    private int count = 5;
    private const int Maxcount = 5;
    private const int Mincount = 0;

    private void Start()
    {
        reproductionObj = null;
        rCount.text = count.ToString();
    }

    private void Update()
    {
        var current_GP = Gamepad.current;
        var reproduction = current_GP.buttonWest;
        Vector3 posi = transform.position;
        reproductionObj = rayCastCS.targetObj;
        duplicate.text = "•¡»‰ñ”  5 / " + count.ToString();

        if (reproductionObj!=null)
        { 
            if (count > Mincount)
            {
                rCount.color = Color.blue;
                if(Input.GetKeyDown(KeyCode.C) || reproduction.wasPressedThisFrame)
                {
                    GameObject newObjct = Instantiate(reproductionObj, posi, transform.rotation);
                    count--;
                }
            }

            if (count == Mincount)
            {
                count = 0;
                rCount.color = Color.red;
            }

        }
    
    }
    public int duplicate_Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
        }
    }
}
