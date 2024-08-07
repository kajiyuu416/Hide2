using UnityEngine;
using UnityEngine.UI;
public class ChangeUI : MonoBehaviour
{
    [SerializeField] public RayCastCS raycastCS;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image guidImage;
    [SerializeField] private Image cursolImage;
    [SerializeField] private Sprite lockOn_Image_gamepad;
    [SerializeField] private Sprite lockOn_Image_keyboard_mouse;
    [SerializeField] private Sprite lockOff_Image_gamepad;
    [SerializeField] private Sprite lockOff_Image_keyboard_mouse;
    private void Update()
    {
        if(guidImage.sprite != null)
        {
            guidImage.color = Color.white;
        }
        else
        {
            guidImage.color = Vector4.zero;
        }

        if(gameManager.Duplicate_operation_gamepad)
        {
            if(raycastCS.Duplicate_morphable)
            {
                guidImage.sprite = lockOn_Image_gamepad;
                cursolImage.color = Color.red;
            }
            else
            {
                guidImage.sprite = lockOff_Image_gamepad;
                cursolImage.color = Color.blue;
            }

        }
        else if(gameManager.Duplicate_operation_keyboard_mouse)
        {
            if(raycastCS.Duplicate_morphable)
            {
                guidImage.sprite = lockOn_Image_keyboard_mouse;
                cursolImage.color = Color.red;
            }
            else
            {
                guidImage.sprite = lockOff_Image_keyboard_mouse;
                cursolImage.color = Color.blue;
            }
        }
    }
}
