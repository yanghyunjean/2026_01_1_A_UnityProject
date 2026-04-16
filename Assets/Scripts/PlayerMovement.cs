using Unity.Hierarchy;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float turnSpeed = 10.0f;

    [Header("점프 개선 설명")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrounded = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimerLeft;
    public bool isGliding = false;


    public Rigidbody rb;

    public bool isGrounded = true;

    public int coinCount = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coyoteTimeCounter = 0;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        gliderTimerLeft = gliderMaxTime;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateGroundedState();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        //이동 방향 벡터
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation , targetRotation, turnSpeed * Time.deltaTime);
        }

        //G키로 글라이더 제어
        if(Input.GetKey(KeyCode.G) && !isGrounded && gliderTimerLeft > 0)
        {
            if(!isGliding)
            {
                EnableGlider();
            }

            gliderTimerLeft -= Time.deltaTime;

            if(gliderTimerLeft<= 0)
            {
                DisableGlider();
            }
        }
        else if(isGliding)
        {
            DisableGlider();
        }

        if(isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);
        }
        else
        {
            rb.linearVelocity = new Vector3(moveHorizontal * moveSpeed, rb.linearVelocity.y, moveVertical * moveSpeed);

            //착시 점프 높이 구현
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

           




        //점프입력
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGrounded = false;
            coyoteTimeCounter = 0;
        }

        if(isGrounded)
        {
            if(isGliding)
            {
                DisableGlider();
            }
            gliderTimerLeft = gliderMaxTime;
        }

    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            realGrounded = true;
        }

    }

    void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            realGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            realGrounded = false;
        }

    }

        void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
        }
    }

    

    void UpdateGroundedState()
    {
        if (realGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    void EnableGlider()
    {
        isGliding = true;

        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, -gliderFallSpeed, rb.linearVelocity.z);
    }

    void DisableGlider()
    {
        isGliding = false;

        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    }

    //글라이더 이동 적용
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        Vector3 gliderVelocity = new Vector3(horizontal * gliderMoveSpeed, -gliderFallSpeed, vertical * gliderMoveSpeed);

        rb.linearVelocity = gliderVelocity;
    }
}



