using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour {

    public float speed = 1f;
    public float gravity = -9.81f;
    public XRNode inputSource;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;

    private float fallingSpeed;
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;

    void Start() {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    void Update() {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    //for motion sickness
    private void FixedUpdate() {
        CapsuleFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);

        //gravity
        if(!CheckIfGrounded()) {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        } else {
            fallingSpeed = 0;
        }

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    //makes sure the capsule is following the head set when player moves around irl
    void CapsuleFollowHeadset() {
        character.height = rig.cameraInRigSpaceHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    bool CheckIfGrounded() {
        Vector3 rayStart = transform.TransformPoint(character.center);
        //0.01 is a little bit extra just in case
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
