using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightBeam : MonoBehaviour
{
    public LineRenderer beamRenderer; // Assign your line renderer in the inspector
    public float beamLength = 10f; // Length of the beam

    private Flashlight flashlight; // Reference to the flashlight script
    

    void Start()
    {
        flashlight = GetComponent<Flashlight>();
    }

    void Update()
    {
        // Update the beam length and position
        beamRenderer.SetPosition(0, transform.position);
        beamRenderer.SetPosition(1, transform.position + transform.forward * beamLength);

        // Enable or disable the beam based on the flashlight state
        beamRenderer.enabled = flashlight.IsFlashlightOn;
    }
}