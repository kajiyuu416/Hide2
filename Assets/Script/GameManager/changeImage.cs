using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeImage : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image Image;
    [SerializeField] private Sprite gamepad_Image;
    [SerializeField] private Sprite keyboard_mouse_Image;
    private void Update()
    {
        if(Image.sprite != null)
        {
            Image.color = Color.white;
        }
        else
        {
            Image.color = Vector4.zero;
        }

        if(gameManager.Duplicate_operation_gamepad)
        {
            Image.sprite = gamepad_Image;
        }
        else if(gameManager.Duplicate_operation_keyboard_mouse)
        {
            Image.sprite = keyboard_mouse_Image;
        }
    }
}
