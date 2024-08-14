using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSE : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ground_footstepSound;
    [SerializeField] AudioClip grassy_footstepSound;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform origin;
    [SerializeField] LayerMask LayerMasks;
    private string layerNameToCompare1 = "groundLayer";
    private string layerNameToCompare2 = "grassyLayer";
    private float raydis = 1.0f;
    private int currentLayer;
    private void FixedUpdate()
    {
        if(Physics.Raycast(origin.transform.position, Vector3.down, out RaycastHit hit, raydis, LayerMasks))
        {
            if(currentLayer!= hit.collider.gameObject.layer)
            {
                currentLayer = hit.collider.gameObject.layer;
            }
        }
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
                else if(currentLayer == grassyLayer)
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
