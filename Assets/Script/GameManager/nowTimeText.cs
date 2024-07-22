using UnityEngine;
using TMPro;

public class nowTimeText : MonoBehaviour
{
    private TextMeshProUGUI timetext;

    private void Start()
    {
        timetext = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        timetext.text = System.DateTime.Now.ToString();
    }
}
