using UnityEngine;
using System.Collections;
using System;

public class Mover : MonoBehaviour {
    // Script for moving around using a wand

    // If true, motion along the vertical axis of the trackpad is mapped to
    // the speed of motion. If false, motion along the horizontal axis is used
    private bool useVertical = true;

    // Maximum motion speed
    private float maxSpeed = 0.6f;
    // Minimum motion speed
    private float minSpeed = 0.004f;
    // Current motion speed
    private float currentSpeed = 0.1f;

    // If true, touchpad motion maps to both backward and forward motion along
    // the Mover's current heading. May cause nausea?
    private bool allowBackwardMotion = true;

    // If true, Mover heading is determined by the HMD forward direction. Else,
    // use the forward direction of the WandController this Mover is attached
    // to
    private bool useHMDHeading = true;

    // Returns the SteamVR_Camera associated to the HMD
    SteamVR_Camera headCamera {
        get {
            SteamVR_Camera top = SteamVR_Render.Top();
            return (top != null) ? top : null;
        }
    }

    // The WandController this Mover is attached to
    private WandController wand;


    // Use this for initialization
    void Start() {
        // Get the wand this Mover is attached to
        wand = GetComponent<WandController>();
        if (!wand) {
            Debug.Log("Error getting Teleporter WandController");
            return;
        }
    }

    // Update is called once per frame
    void Update() {
        // If the touchpad is pressed down, move forward in the direction you're looking
        if (wand.controller.GetPress(wand.touchpadButton)) {
            // Get the current head position
            SteamVR_Camera hmdCam = headCamera;

            // Get the touchpad press location
            Vector2 touchLocation = wand.controller.GetAxis();

            // Get the position (in range [-1,1]) chosen axis
            float touchVal = useVertical ? touchLocation[1] : touchLocation[0];

            // Get the heading to use for motion
            Vector3 heading = useHMDHeading ? hmdCam.head.forward : transform.forward;
            // Ignore motion along the Y axis
            heading[1] = 0f;

            // Updated speed along the heading
            float newSpeed;

            // Contact on the rim of the touchpad produces a GetAxis() value of
            // (0,0). In this case, keep using the same speed used in the
            // previous frame
            if (touchLocation[0] == 0 && touchLocation[1] == 0) {
                newSpeed = currentSpeed;

            }
            else {
                // If allowBackwardMotion is true, touchVal maps [0,1] to
                // [minSpeed,maxSpeed] and [-1,0] to [-maxSpeed,-minSpeed]. Else,
                // [-1,1] is mapped to [minSpeed,maxSpeed]
                if (allowBackwardMotion) {
                    // Get the magnitude of touchVal
                    float touchMagnitude = Mathf.Abs(touchVal);

                    // Map [0,1] to [minSpeed,maxSpeed], then apply the same
                    // sign as touchVal
                    newSpeed = Mathf.Sign(touchVal) * Mathf.Max(minSpeed, touchMagnitude * maxSpeed);

                }
                else {
                    // Rescale the [-1,1] input to [0,1]
                    float touchRescaled = 0.5f * (touchVal + 1);

                    // Map [0,1] to [minSpeed,maxSpeed]
                    newSpeed = Mathf.Max(minSpeed, touchRescaled * maxSpeed);
                }
            }
            // Update currentSpeed
            currentSpeed = newSpeed;

            // Update the HMD camera position
            Vector3 newPosition = hmdCam.origin.position + currentSpeed * heading;

            hmdCam.origin.position = newPosition;
        }
    }
}
