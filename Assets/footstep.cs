using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstep : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ground_footstepSound;
    [SerializeField] AudioClip grassy_footstepSound;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform reg;
    [SerializeField] LayerMask LayerMasks;
    private string layerNameToCompare1 = "groundLayer";
    private string layerNameToCompare2 = "grassyLayer";
    private float raydis = 1.0f;
    int currentLayer;
    private void Start()
    {
        int groundLayer = LayerMask.NameToLayer(layerNameToCompare1);
        currentLayer = groundLayer;
    }
    private void FixedUpdate()
    {
        if(Physics.Raycast(reg.transform.position, Vector3.down, out RaycastHit hit, raydis, LayerMasks))
        {
            currentLayer = hit.collider.gameObject.layer;
            Debug.Log(currentLayer + "ÉåÉCÉÑÅ[Ç…ïœçX");
        }
        Debug.Log("currentLayer" + currentLayer);
        Debug.DrawRay(reg.transform.position, Vector3.down * raydis, Color.red);
    }

    public void PlayFootstepSound()
    {
        int groundLayer = LayerMask.NameToLayer(layerNameToCompare1);
        int grassyLayer = LayerMask.NameToLayer(layerNameToCompare2);

        if(playerController.Duplicate_playerMove_input != Vector3.zero && playerController.Duplicate_state == (int) PlayerController.player_state.defaultMode)
        {
            if(playerController.Duplicate_characterController.isGrounded)
            {
                if(currentLayer == groundLayer)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(ground_footstepSound);
                }

                if(currentLayer == grassyLayer)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(grassy_footstepSound);
                }

            }

        }
        else
        {
            audioSource.Stop();
        }
    }
}
