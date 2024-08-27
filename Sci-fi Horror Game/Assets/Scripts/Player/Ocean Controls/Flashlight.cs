using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlightLight; // Assign your light component in the inspector
    public float lightRange = 10f; // Range of the light
    public float lightIntensity = 8f; // Intensity of the light

    private bool isFlashlightOn = false;
    public bool IsFlashlightOn { get { return isFlashlightOn; } }

    void Update()
    {
        // Toggle flashlight on and off with the 'F' key
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightLight.enabled = isFlashlightOn;
        }

        // Update light range and intensity
        flashlightLight.range = lightRange;
        flashlightLight.intensity = lightIntensity;
    }

    void FixedUpdate()
    {
        // Rotate the light to match the player's rotation
        flashlightLight.transform.rotation = transform.rotation;
    }
}