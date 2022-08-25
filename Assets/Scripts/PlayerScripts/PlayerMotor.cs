using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
   
    private CharacterController character;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrounch;
    private bool crounching;
    private bool sprinting;
    private float crounchTimer;
    [SerializeField]
    private float gravity = -9.8f;
    [SerializeField]
    private float jumpHeight = 3f;
    [SerializeField]
    private float sprintingSpeed = 8;
    [SerializeField]
    private float defaultSpeed =5;
    [SerializeField]
    private float crounchingSpeed= 2.5f;
    private float speed; // main speed that is using for everything
    [HideInInspector]
    public Vector3 currentVelocity;

    void Start()
    {
        character= GetComponent<CharacterController>();
        speed = defaultSpeed;
        sprinting = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = character.isGrounded;
        if (lerpCrounch)
        {
           
            crounchTimer += Time.deltaTime;
            float p = crounchTimer / 0.5f;
            p *= 2*p;
            if (crounching)
            {
                character.height = Mathf.Lerp(character.height, 1, p);
                speed = crounchingSpeed;
            }         
            else
            {
                character.height = Mathf.Lerp(character.height, 2, p);
                speed = defaultSpeed;
            }
                
            if (p>1)
            {
                lerpCrounch = false;
                crounchTimer = 0f;
            }
        }
        
        
    }
    public void Crounch()
    {
        crounching = !crounching;
        crounchTimer = 0;
        lerpCrounch = true;

    }
    public void Sprint()
    {
        if (!crounching)
        {
            sprinting = !sprinting;
            if (sprinting) speed = sprintingSpeed;
            else speed = defaultSpeed;
        }
       

    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        character.Move(transform.TransformDirection(moveDirection)*speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
       
        character.Move(playerVelocity * Time.deltaTime);
        if (isGrounded&&playerVelocity.y<0)
        {
            playerVelocity.y = -2f;
        }
        currentVelocity = (transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
       

    }
    public void Jump()
    {
        if(isGrounded)
        {
            
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }    
    }
}
