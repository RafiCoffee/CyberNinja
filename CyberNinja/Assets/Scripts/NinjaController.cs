using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class NinjaController : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    int maxDashCount = 1;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    float jumpForce = 10f;

    private Vector2 movementInput;
    private Vector2 movement;
    private Vector2 dashInput;
    private Vector2 attackInput;
    private Vector2 blockInput;
    private Vector2 wallJump;
    private Vector2 collisionRecoil;
    public Vector2 bulletCollision;

    private Quaternion playerRotation;

    private int dashCount;
    public int vida = 5;

    private float movementAnim;
    private float dashTime;
    public float startDashTime;
    private float startGravity;
    public float blockCoolDown;

    public bool canMove;
    private bool jump;
    private bool canWallJump = false;
    public bool isOnGroundWall = false;
    private bool isOnGround;
    private bool dash;
    private bool isDashing = false;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool canReturn = false;
    public bool bulletOnBack;

    public GameObject attackTrigger;
    public GameObject blockTrigger;

    private Rigidbody2D playerRb2D;
    private BoxCollider2D playerBC;
    private Animator playerAnim;

    private Stopwatch blockTimer = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        playerBC = GetComponent<BoxCollider2D>();
        playerAnim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

        dashTime = startDashTime;
        dashCount = maxDashCount;
        startGravity = playerRb2D.gravityScale;

        blockTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        movementAnim = Input.GetAxisRaw("Horizontal");
        movementInput.x = Input.GetAxis("Horizontal");
        dashInput.x = Input.GetAxisRaw("Horizontal");

        if (dashInput.x == 0)
        {
            dash = false;
        }

        if (!canWallJump)
        {
            if (!isAttacking)
            {
                attackInput.x = Input.GetAxisRaw("Horizontal");
                attackInput.y = Input.GetAxisRaw("Vertical");
            }
            else
            {
                playerRotation = transform.GetChild(0).rotation;
                transform.GetChild(0).rotation = playerRotation;
            }

            if (!isBlocking)
            {
                blockInput.x = Input.GetAxisRaw("Horizontal");
            }
            else
            {
                playerRotation = transform.GetChild(0).rotation;
                transform.GetChild(0).rotation = playerRotation;
            }

            if (isOnGround)
            {
                jump |= Input.GetKeyDown(KeyCode.Space);
            }

        }

        if (Input.GetKeyDown(KeyCode.J) && !isBlocking && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        if (Input.GetKeyDown(KeyCode.L) && !isAttacking && !isBlocking && blockTimer.ElapsedMilliseconds / 1000 >= blockCoolDown)
        {
            StartCoroutine(Block());
        }

        if (isBlocking)
        {
            playerBC.enabled = false;
        }
        else
        {
            playerBC.enabled = true;
        }

        if (canMove)
        {
            playerAnim.SetInteger("Movement", (int)movementAnim);

            if (attackInput != Vector2.zero && attackInput != Vector2.one && attackInput != -Vector2.one && attackInput != new Vector2(1, -1) && attackInput != new Vector2(-1, 1))
            {
                if (attackInput.y == -1 && !isOnGround || attackInput.y == 1)
                {
                    transform.GetChild(1).localScale = Vector3.one;
                    transform.GetChild(1).localPosition = attackInput * 3;
                }
                else if (attackInput == Vector2.right || attackInput == Vector2.left)
                {
                    transform.GetChild(1).localScale = new Vector3(1, 4.1f, 1);
                    transform.GetChild(1).localPosition = attackInput;
                }
            }

            if (blockInput != Vector2.zero)
            {
                transform.GetChild(2).localPosition = blockInput / 1.6f;
            }

            if (isDashing && !canWallJump)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    gameObject.layer = 7;
                    playerRb2D.gravityScale = startGravity;
                    dashTime = startDashTime;
                    isDashing = false;
                }
            }
            else if (!isDashing)
            {
                if (movementInput.x != 0)
                {
                    dash |= Input.GetKeyDown(KeyCode.K);
                }
                movement = movementInput * maxSpeed;
            }

            if (canWallJump)
            {
                if (wallJump.x > 0)
                {
                    transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (wallJump.x < 0)
                {
                    transform.GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
                }
                attackInput.x = wallJump.x;
                blockInput.x = wallJump.x;
                dash = false;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    canMove = false;
                    playerRb2D.AddForce(wallJump * jumpForce / 1.5f, ForceMode2D.Impulse);
                    canWallJump = false;
                }
            }
            else if (!canWallJump && !isAttacking && !isBlocking)
            {
                if (movementInput.x > 0)
                {
                    transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (movementInput.x < 0)
                {
                    transform.GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
                }
            }
        }
        else
        {
            if (movementInput.x != 0)
            {
                dash |= Input.GetKeyDown(KeyCode.K);
            }
        }

        if (isOnGroundWall)
        {
            playerRb2D.gravityScale = startGravity;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(playerRb2D.velocity);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (jump && isOnGround)
            {
                jump = false;
                Jump();
            }

            if (dash && dashInput != Vector2.zero && dashCount != 0)
            {
                dash = false;
                Dash();
            }

                playerRb2D.velocity = new Vector2(movement.x, playerRb2D.velocity.y);
        }
        else
        {
            if (dash && dashInput != Vector2.zero && dashCount != 0)
            {
                canMove = true;
                dash = false;
                Dash();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y == 1 & collision.collider.gameObject.layer == 3)
        {
            dashCount = maxDashCount;
            playerRb2D.gravityScale = startGravity;
            isOnGround = true;
            canMove = true;
        }

        if (gameObject.layer == 7 && collision.collider.gameObject.layer == 8 || gameObject.layer == 7 && collision.collider.gameObject.layer == 12 || gameObject.layer == 7 && collision.collider.gameObject.layer == 13)
        {
            playerRb2D.velocity = Vector2.zero;
            collisionRecoil = collision.GetContact(0).normal;
            if (collisionRecoil == Vector2.up)
            {
                if (transform.position.x < collision.collider.transform.position.x)
                {
                    collisionRecoil = collisionRecoil + Vector2.left;
                }
                else
                {
                    collisionRecoil = collisionRecoil + Vector2.right;
                }
            }
            if (collision.collider.gameObject.layer != 13)
            {
                StartCoroutine(Invencible());
            }
            StartCoroutine(DontMove());
            playerRb2D.AddForce(collisionRecoil * jumpForce / 2f, ForceMode2D.Impulse);
        }

        if (collision.collider.gameObject.layer == 6 && isOnGround == false)
        {
            canMove = true;
        }

        if (gameObject.layer == 7 && collision.collider.gameObject.layer == 11)
        {
            bulletCollision = playerBC.offset;
            Debug.Log(bulletCollision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.GetChild(1).gameObject.layer == 9 && collision.gameObject.layer == 8 && attackInput == Vector2.down && isOnGround == false)
        {
            dashCount = maxDashCount;
            playerRb2D.velocity = Vector2.zero;
            playerRb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y == 1 & collision.collider.gameObject.layer == 3)
        {
            playerRb2D.gravityScale = startGravity;
            dashCount = maxDashCount;
            isOnGround = true;
        }

        if (collision.collider.gameObject.layer == 6 && isOnGround == false)
        {
            dashCount = maxDashCount;
            playerRb2D.gravityScale = 1;
            wallJump = collision.GetContact(0).normal + Vector2.up;
            canWallJump = true;
        }

        if (collision.collider.gameObject.layer == 6 && isOnGround)
        {
            playerRb2D.gravityScale = startGravity;
            isOnGroundWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            isOnGround = false;
        }

        if (collision.collider.gameObject.layer == 6)
        {
            playerRb2D.gravityScale = startGravity;
            canWallJump = false;
            isDashing = false;
            isOnGroundWall = false;
        }
    }


    void Jump()
    {
        isOnGround = false;
        playerRb2D.velocity = Vector2.zero;
        playerRb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Dash()
    {
        dashCount--;
        gameObject.layer = 14;
        isDashing = true;
        playerRb2D.velocity = Vector2.zero;
        movement = Vector2.zero;
        movement += dashInput * jumpForce;
        playerRb2D.gravityScale = 0.5f;
    }

    IEnumerator DontMove()
    {
        playerRb2D.gravityScale = startGravity;
        canMove = false;
        isDashing = false;
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }

    IEnumerator Invencible()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        attackTrigger.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
        attackTrigger.SetActive(false);
    }

    IEnumerator Block()
    {
        yield return new WaitForSeconds(0.1f);
        canReturn = true;
        isBlocking = true;
        blockTrigger.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        canReturn = false;
        yield return new WaitForSeconds(0.25f);
        isBlocking = false;
        blockTrigger.SetActive(false);
        blockTimer.Restart();
    }
}
