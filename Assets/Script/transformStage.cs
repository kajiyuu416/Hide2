using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class transformStage : MonoBehaviour
{
    public Transform transformPos;
    [SerializeField] Material Stage_skybox;
    [SerializeField] AudioSource select_bgm;
    public void PlayMusic()
    {
        RenderSettings.skybox = Stage_skybox;
        AudioManager.Instance.StopBGM_SE();
        select_bgm.Play();
    }
}
