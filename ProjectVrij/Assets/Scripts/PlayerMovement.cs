using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;
    public PlayerAnimationmanager animationManager;

    public Transform camera;

    public AudioSource walkSound;
    public AudioSource jumpSound;
    public AudioSource jumpVoiceSound;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private float walkTimer = 0;
    private float jumpTimer = 0;
    bool jump = false;

    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.6f;
    public LayerMask groundMask;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animationManager.GetComponent<PlayerAnimationmanager>();
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            animationManager.IdleAnimation();
            //walkSound.Stop();
            //jumpSound.Stop();
            //jumpVoiceSound.Stop();
        }


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            if (isGrounded)
            {
                if(!walkSound.isPlaying)
                    walkSound.GetComponent<AudioSource>().PlayOneShot(walkSound.clip, 0.7f);
                animationManager.WalkAnimation();
            }
        }
        if (Input.GetButtonDown("Jump") && isGrounded && !jumpSound.isPlaying && !jumpVoiceSound.isPlaying)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            jumpSound.GetComponent<AudioSource>().PlayOneShot(jumpSound.clip, 0.7f);
            jumpVoiceSound.GetComponent<AudioSource>().PlayOneShot(jumpVoiceSound.clip, 0.7f);
            jump = true;
        }
        if (!isGrounded)
        {
            animationManager.JumpAnimation();
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

}
