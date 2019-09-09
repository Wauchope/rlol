using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterData playerData;

    private CameraController cameraController;

    private Rigidbody rigidBody;

    private Vector3 movement = Vector3.zero;

    private Timer jumpTimer = new Timer(TimerType.FIXED);
    private Timer dodgeTimer = new Timer(TimerType.FIXED);

    private Coroutine dodgeRoutine;

    private bool isGrounded;

    bool isDodging;
    [SerializeField]
    float dodgeDuration = 0.4f;

    private Animator playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        AttachCamera();

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.maxAngularVelocity = 120f;

        playerAnimator = GetComponentInChildren<Animator>();

        StartCoroutine(AnimControl());

        //rigidBody.ResetInertiaTensor();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void FixedUpdate()
    {
        CheckMovement();
        jumpTimer.IncrementTimer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    //Called in Update
    private void CheckInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!playerAnimator.GetBool("isAttacking"))
            {
                playerAnimator.Play("SwordSlash_01");
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isDodging)
            {
                isDodging = true;
            }
        }
    }

    //Called in FixedUpdate to ensure smooth physics updates
    //Dont check for keydown here, use CheckInput
    private void CheckMovement()
    {
        rigidBody.angularVelocity = Vector3.zero;

        float x, y, z, jump = 0;
        Vector3 movement;

        //Forwards/Backwards
        z = Input.GetAxis("Vertical") * Time.deltaTime * playerData.Speed;

        //Left/Right
        x = Input.GetAxis("Strafe") * Time.deltaTime * playerData.Speed;

        //Rotation
        y = Input.GetAxis("Horizontal") * Time.deltaTime * playerData.TurnRate;

        jump = Input.GetAxis("Jump");

        if (isGrounded && jump != 0 && jumpTimer.GetTime() >= 0.1f)
        {
            jumpTimer.ResetTimer();
            rigidBody.AddForce(gameObject.transform.up * playerData.JumpForce, ForceMode.Impulse);
        }

        if (x != 0 && z != 0)
        {
            movement = new Vector3(x * 0.63f, y, z * 0.63f);
        }
        else
        {
            movement = new Vector3(x, y, z);
        }

        if (isDodging)
        {
            Dodge(new Vector3(movement.x, 0, movement.z));
        }

        //If movement is not 0 and the player isnt moving vertically
        if (movement.x != 0 || movement.z != 0 || movement.y != 0|| !Mathf.Approximately(rigidBody.velocity.y, 0f))
        {
            MovePlayer(movement);
        }
        else // If the player is not intending to move and they're not falling
        {
            //Set the velocity of the player to zero
            rigidBody.velocity = Vector3.zero;
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        //If x & z are both not 0 player moves ~40% faster -- POTENTIALLY FIXED
        if (!CheckPlayerPath(new Vector3(direction.x, 0, direction.z)))
        {
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
            transform.Translate(direction.x, 0, direction.z);
        }
        transform.Rotate(0, direction.y, 0);
        cameraController.Move(direction);

    }
    
    private bool CheckPlayerPath(Vector3 direction)
    {
        RaycastHit raycast;
        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out raycast, direction.magnitude * 2))
        {
            return true;
        }
        return false;
    }

    private CameraController GetCamera()
    {
        return Camera.main.GetComponent<CameraController>();
    }

    private void AttachCamera()
    {
        cameraController = GetCamera();
        cameraController.SetFocus(gameObject);
    }

    private IEnumerator AnimControl()
    {
        playerAnimator.SetBool("isAttacking", true);
        yield return new WaitWhile(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwordSlash_01"));
        playerAnimator.SetBool("isAttacking", false);
    }

    public bool GetAttacking()
    {
        return playerAnimator.GetBool("isAttacking");
    }
    
    private void Dodge(Vector3 direction)
    {
        if (dodgeTimer.GetTime() <= dodgeDuration)
        {
            MovePlayer(direction);
            dodgeTimer.IncrementTimer();
        }
        else
        {
            isDodging = false;
            dodgeTimer.ResetTimer();
        }
    }
}
