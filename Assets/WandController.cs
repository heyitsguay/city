using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {

    // Reference to one of the controller wands
    public SteamVR_Controller.Device controller;

    // The grip button
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    // The trigger button
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    // The trackpad
    public Valve.VR.EVRButtonId touchpadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    // The menu button
    private Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;

    // Use this for initialization
    void Start () {
        // Get a reference to the SteamVR object invoking this script
        var trackedObj = GetComponent<SteamVR_TrackedObject>();

        // Get trackedObj's Device ID
        var objID = (int)trackedObj.index;

        controller = SteamVR_Controller.Input(objID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
