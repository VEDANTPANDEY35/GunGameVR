using UnityEngine;
using UnityEngine.InputSystem;

public class VRCrouch : MonoBehaviour
{
    public InputActionProperty crouchButton;

    public float crouchHeight = 1.0f;
    public float standingHeight = 1.8f;
    public float crouchSpeed = 6f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float targetHeight = crouchButton.action.IsPressed() ? crouchHeight : standingHeight;

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            Time.deltaTime * crouchSpeed
        );
    }
}