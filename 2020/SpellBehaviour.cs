using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SpellBehaviour : MonoBehaviour {

    public XRNode inputSource;
    public float speed = 70f;

    private bool isPressed = false;
    private Rigidbody rb;

    public bool isBeingHeld { get; set; } = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);

        if(isPressed && isBeingHeld) {
            FireSpell();
        }
    }

    void FireSpell() {
        //Gets direction of thrown fireball
        Vector3 direction = rb.velocity.normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
