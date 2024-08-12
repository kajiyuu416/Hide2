using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireworkSC : MonoBehaviour
{
    public ParticleSystem particle_System;
    public AudioSource audioSource;
    private bool readyflag;
    private void Update()
    {
        if(particle_System.particleCount == 0 &&!readyflag)
        {
            audioSource.PlayOneShot(AudioManager.Instance.blast_fireworkSe);
            readyflag = true;
        }
        else if(particle_System.particleCount == 1 && readyflag)
        {
            audioSource.PlayOneShot(AudioManager.Instance.launch_fireworkSe);
            readyflag = false;
        }
    }
}
