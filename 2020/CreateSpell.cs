using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CreateSpell : MonoBehaviour {

    public XRNode inputSource;
    public GameObject fireBall;
    public Transform castPos;
    public float speed = 70;

    private bool isPressed;
    private float gripValue;
    private XRController controller;
    private bool isSpawned = false;
    private GameObject spawnedFireBall;

    public bool isHoldingSomething { get; set; } = false;

    void Start() {
        controller = GetComponent<XRController>();
    }


    //checks to see if a button and grip are held down, drops whatever the players currently holding, spawns a fireball, and then when they release it goes shooting off
    void Update() {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed);
        device.TryGetFeatureValue(CommonUsages.grip, out gripValue);

        if (isPressed && gripValue > 0.7) {
            controller.enableInteraction = false;
            if (!isSpawned) {
                spawnedFireBall = Instantiate(fireBall, castPos.position, castPos.rotation, transform);
                isSpawned = true;
            }

        } else if (!isPressed && gripValue < 0.4 && isSpawned) {
            spawnedFireBall.transform.parent = null;
            spawnedFireBall.GetComponent<Rigidbody>().velocity = speed * castPos.forward;
            spawnedFireBall = null;
            isSpawned = false;
        } else {
            controller.enableInteraction = true;
        }
    }

    void SpawnSpell() {
        if (isHoldingSomething) {

        }
    }
}
