using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Mozgás")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float headMoveSpeed = 15f;

    public float GetHeadMoveSpeed
    {
        get
        {
            return headMoveSpeed;
        }
    }

    [Header("Dodge")]
    [SerializeField] float dodgeForce = 30f;
    [SerializeField] float dodgeDuration = 0.6f;
    [SerializeField] float dodgeCooldown = 1.3f;
    [SerializeField] float doubleTapTime = 0.3f;

    private bool canDodge = true;
    private bool isDodging = false;

    private float lastTapTimeA = -1f;
    private float lastTapTimeD = -1f;
    [SerializeField] private Player_Character playerCharacter;
    int normalLayer;
    int dodgeLayer;


    [Header("Ugrás")]
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    public bool isJumpPadBoost = false;

    [Header("Talaj-check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    private Vector2 moveInput;
    private bool jumpRequest;
    private bool isGrounded;
    private bool isFacingRight = true;
    private Coroutine cameraSizeCoroutine;
    private Coroutine cameraOffsetCoroutine;


    [Header("Karakter részek")]
    [SerializeField] Transform characterBody;
    [SerializeField] Transform characterHead;
    [SerializeField] Transform characterFeet1;
    [SerializeField] Transform characterFeet2;
    [SerializeField] Transform characterEye;
    [SerializeField] Transform characterLight;

    [Header("Kamera")]
    public  Camera CameraMain;
    [SerializeField]  Player_Camera cameraScript;

    public float maxCameraSize = 15f;


    [Header("Célkereszt")]
    [SerializeField]  Crosshair playerCrosshair;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Mozgás korlátozók")]
    public float maxMoveSpeed = 4f;
    public float maxHeadMoveSpeed = 10f;
    public float maxJumpForce = 10f;
    public float maxFallMultiplier = 1.5f;
    public float maxLowJumpMultiplier = 0.6f;

    public GameObject YupgradePanel;
    public GameObject RupgradePanel;
    public int openPanels = 0;
    public static bool isUIOpen = false;


    private Platform_PingPong currentPlatform;

    public LightType lightTypeWas;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        normalLayer = gameObject.layer;
        dodgeLayer = LayerMask.NameToLayer("DodgingLayer");
    }

    void Update()
    {

        // A lenyomás
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time - lastTapTimeA < doubleTapTime)
            {
                TryDodge(Vector2.left);
            }
            lastTapTimeA = Time.time;
        }

        // D lenyomás
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time - lastTapTimeD < doubleTapTime)
            {
                TryDodge(Vector2.right);
            }
            lastTapTimeD = Time.time;
        }


        // Input
        float moveX = Input.GetAxisRaw("Horizontal");
        moveInput = new Vector2(moveX, 0);

        if (moveInput.x > 0.01f)
            isFacingRight = true;

        if (moveInput.x < -0.01f)
            isFacingRight = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
                jumpRequest = true;
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            if (!isUIOpen)
            {
                    OpenPanel(YupgradePanel);
            }
                else
            {
                    ClosePanel(YupgradePanel);
                    ClosePanel(RupgradePanel);
            }
            
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            if (!isUIOpen)
            {
                    OpenPanel(RupgradePanel);
            }
            else
            {
                    ClosePanel(YupgradePanel);
                    ClosePanel(RupgradePanel);
            }
        }

        // Talaj ellenőrzés
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        animator.SetFloat("speed", Mathf.Abs(moveInput.x));
        animator.SetBool("grounded", isGrounded);
        animator.SetBool("right", isFacingRight);
    }

    void FixedUpdate()
    {

        if (isDodging)
            return;


        // Vízszintes mozgás---------------------------------------------------------------------------------------------

        Vector2 platformVel = currentPlatform != null ? currentPlatform.platformVelocity : Vector2.zero;

        Vector2 velocity = rigidBody.velocity;

        velocity.x = moveInput.x * moveSpeed + platformVel.x;

        rigidBody.velocity = velocity;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Egér követés-------------------------------------------------------------------------------------------

        if (mousePos.x < characterBody.position.x)
        {
            characterBody.localScale = new Vector3(-1, 1, 1);
            characterFeet1.GetComponent<SpriteRenderer>().sortingOrder = 1;
            characterFeet2.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        if (mousePos.x > characterBody.position.x)
        {
            characterBody.localScale = Vector3.one;
            characterFeet1.GetComponent<SpriteRenderer>().sortingOrder = 2;
            characterFeet2.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        if (mousePos.x < characterHead.position.x)
        {
            characterHead.localScale = new Vector3(1, -1, 1);
            characterLight.transform.localScale = new Vector3(1, -1, 1);
            characterEye.localScale = new Vector3(1, -1, 1);

        }
        if (mousePos.x > characterHead.position.x)
        {
            characterHead.localScale = Vector3.one;
            characterLight.transform.localScale = Vector3.one;
            characterEye.localScale = Vector3.one;
        }

        // Irányvektor a karakter fejétől az egérig
        Vector2 direction = (mousePos - characterHead.position).normalized;

        // Spotlight forgatása az irányba----------------------------------------------------------------------------------------------------------

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterHead.transform.rotation = Quaternion.Lerp(characterHead.transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * headMoveSpeed);
        characterEye.transform.rotation = Quaternion.Lerp(characterEye.transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * headMoveSpeed);

        //Ugrás------------------------------------------------------------------------------------------------------------------------------------

        if (jumpRequest)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f); // y sebesség -> reset
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            jumpRequest = false;
        }


        if (!isDodging)
        {
            if (rigidBody.velocity.y < 0)
            {
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
            }
            else if (rigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rigidBody.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.fixedDeltaTime;
            }
        }
    }

    //Jump/Dodge debughoz!
    /*void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }*/
    void TryDodge(Vector2 direction)
    {
        if (!canDodge || isDodging)
            return;

        StartCoroutine(Dodge(direction));
    }

    IEnumerator Dodge(Vector2 direction)
    {
        playerCharacter.SetInvincible(true);
        playerCharacter.StartCoroutine(playerCharacter.FlashEffect());

        canDodge = false;
        isDodging = true;
        gameObject.layer = dodgeLayer;

        rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        rigidBody.velocity = Vector2.zero;

        rigidBody.velocity = new Vector2(direction.x * dodgeForce, 0f);

        yield return new WaitForSeconds(dodgeDuration);

        rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);

        isDodging = false;
        playerCharacter.SetInvincible(false);
        gameObject.layer = normalLayer;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveInput = dir;
    }
    public IEnumerator ResetJumpPadBoost()
    {
        yield return new WaitForSeconds(0.5f);
        isJumpPadBoost = false;
    }
    public void RequestJump()
    {
        if (isGrounded)
            jumpRequest = true;
    }

    public void ShootingRed()
    {
        moveSpeed = maxMoveSpeed;
        headMoveSpeed = maxHeadMoveSpeed;
        jumpForce = maxJumpForce;
        fallMultiplier = maxFallMultiplier;
        lowJumpMultiplier = maxLowJumpMultiplier;
        GrowToTarget(maxCameraSize+2f, 2f);
        playerCrosshair.CrosshairSlow(headMoveSpeed);
        GrowToTargetCamera(3f, 10f);
    }

    public void ResetMovementRed()
    {
        moveSpeed = 8.0f;
        headMoveSpeed = 15.0f;
        jumpForce = 25.0f;
        fallMultiplier = 2.5f;
        lowJumpMultiplier = 2.0f;
        GrowToTarget(maxCameraSize, 2f);
        playerCrosshair.CrosshairReset();
        GrowToTargetCamera(0f, 10f);
    }

    public void GrowToTarget(float amount, float speed)
    {
        if (cameraSizeCoroutine != null)
        {
                StopCoroutine(cameraSizeCoroutine);
        }
        cameraSizeCoroutine = StartCoroutine(GrowToTargetCorutine(amount, speed));
    }

    IEnumerator GrowToTargetCorutine(float target, float speed)
    {
        if (CameraMain.orthographicSize > target)
        {
             while (CameraMain.orthographicSize > target)
            {
                CameraMain.orthographicSize = Mathf.MoveTowards(
                    CameraMain.orthographicSize,
                    target,
                    speed * Time.deltaTime
                );
                yield return null;
            }
        }
        else
        {
             while (CameraMain.orthographicSize < target)
            {
                CameraMain.orthographicSize = Mathf.MoveTowards(
                    CameraMain.orthographicSize,
                    target,
                    speed * Time.deltaTime
                );
                yield return null;
            }
        }
       
    }

    public void GrowToTargetCamera(float maxX, float speed)
    {
         if (cameraOffsetCoroutine != null)
        StopCoroutine(cameraOffsetCoroutine);

        cameraOffsetCoroutine = StartCoroutine(GrowToTargetCameraCorutine(maxX, speed));
    }

    IEnumerator GrowToTargetCameraCorutine(float maxX, float speed)
    {
        float relativeX = playerCrosshair.CUCC.transform.position.x - transform.position.x;

        if (maxX == 0)
        {
             while (cameraScript.offset.x != maxX)
            {
                cameraScript.offset.x = Mathf.MoveTowards(
                    cameraScript.offset.x,
                    maxX,
                    speed * Time.deltaTime
                );
                yield return null;
            }
        }
        if (relativeX > 0)
        {
             while (cameraScript.offset.x < maxX)
            {
                cameraScript.offset.x = Mathf.MoveTowards(
                    cameraScript.offset.x,
                    maxX,
                    speed * Time.deltaTime
                );
                yield return null;
            }
        }
        else
        {
             while (relativeX < 0)
            {
                cameraScript.offset.x = Mathf.MoveTowards(
                    cameraScript.offset.x,
                    -maxX,
                    speed * Time.deltaTime
                );
                yield return null;
            }
        }
       
    }
    void OpenPanel(GameObject panel)
    {
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
            openPanels++;
            UpdateUIState();
        }
    }

    void ClosePanel(GameObject panel)
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
            openPanels--;
            UpdateUIState();
        }
    }
   void UpdateUIState()
    {
        bool isOpen = openPanels > 0;
        isUIOpen = isOpen;

        if (isOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = collision.gameObject.GetComponent<Platform_PingPong>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = null;
        }
    }
}
