using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("HeadBob Parameters")]
    [SerializeField]
    PlayerMotor characterController;
    [SerializeField] float walkBobSpeed = 14f;
    [SerializeField] float walkBobAmount = 0.05f;

    float defaultYPos = 0;
    float timer = 0;
    [SerializeField]
    bool canUseHeadBob = true;
    Camera playerCam;
    private void Awake()
    {
        playerCam = GetComponent<Camera>();
        defaultYPos = playerCam.transform.localPosition.y;
    }
    private void Update()
    {
        if (canUseHeadBob)
            HandleHeadBob(); 

    }
    void HandleHeadBob()
    {
        if (!characterController.isGrounded) return;
        if (Mathf.Abs(characterController.moveDirection.x) > 0.01f || Mathf.Abs(characterController.moveDirection.z) > 0.01f)
        {
            timer += Time.deltaTime * walkBobSpeed;
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * walkBobAmount, playerCam.transform.localPosition.z);
        }
    }
}

