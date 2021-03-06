using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    Vector2 movement;
    Vector2 movementInput;
    Vector2 dashInput;
    Vector2 attackInput;
    Vector2 blockInput;
    Vector2 wallJump;
    Vector2 collisionRecoil;

    public int dashCount = 1;
    private int maxDashCount;

    public float movementSpeed = 40f;
    public float jumpForce = 8f;
    private float dashTime;
    public float startDashTime;
    private float startGravity;

    public bool canMove;
    public bool jump;
    private bool isOnGround = true;
    public bool isOnGroundWall = false;
    private bool canWallJump = false;
    private bool isDashing = false;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool canReturn = false;

    private Rigidbody2D playerRb2D;
    private BoxCollider2D playerBc2D;
    private Animator playerAnim;

    public LayerMask enemy;
    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        playerBc2D = GetComponent<BoxCollider2D>();
        playerAnim = GetComponent<Animator>();

        dashTime = startDashTime;
        startGravity = playerRb2D.gravityScale;
        maxDashCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        movementInput.x = Input.GetAxis("Horizontal");

        dashInput.x = Input.GetAxisRaw("Horizontal");

        jump |= Input.GetKeyDown(KeyCode.Space);

        if (!isAttacking)
        {
            attackInput.x = Input.GetAxisRaw("Horizontal");
            attackInput.y = Input.GetAxisRaw("Vertical");
        }

        if (!isBlocking)
        {
            blockInput.x = Input.GetAxisRaw("Horizontal");
        }

        if (canMove)
        {

            if (attackInput != Vector2.zero && attackInput != Vector2.one && attackInput != -Vector2.one && attackInput != new Vector2(1, -1) && attackInput != new Vector2(-1, 1))
            {
                if (attackInput == Vector2.down  || attackInput == Vector2.up)
                {
                    transform.GetChild(1).localScale = Vector3.one;
                    transform.GetChild(1).localPosition = attackInput * 2;
                }
                else
                {
                    transform.GetChild(1).localScale = new Vector3 (1, 4.1f, 1);
                    transform.GetChild(1).localPosition = attackInput;
                }
            }

            if (blockInput != Vector2.zero)
            {
                transform.GetChild(2).localPosition = blockInput / 1.6f;
            }

            movement = movementInput * movementSpeed;

            if (Input.GetKeyDown(KeyCode.J) && !isBlocking)
            {
                playerAnim.SetTrigger("Attack");
            }

            if (Input.GetKeyDown(KeyCode.L) && !isAttacking)
            {
                playerAnim.SetTrigger("Block");
            }
        }

        if (canWallJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRb2D.AddForce(wallJump * jumpForce / 1.3f, ForceMode2D.Impulse);
                canWallJump = false;
            }
        }

        if (isOnGroundWall)
        {
            playerRb2D.gravityScale = startGravity;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
                playerRb2D.velocity = movement;

            if (jump & isOnGround)
            {
                playerRb2D.gravityScale = startGravity;
                isOnGround = false;
                jump = false;
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.K) && dashInput != Vector2.zero && dashCount != 0)
            {
                dashCount--;
                isDashing = true;
                playerRb2D.velocity = Vector2.zero;
                playerRb2D.AddForce(dashInput * jumpForce / 1.8f, ForceMode2D.Impulse);
                playerRb2D.gravityScale = 0.5f;
            }

            if (isDashing)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    playerRb2D.gravityScale = startGravity;
                    dashTime = startDashTime;
                    isDashing = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y == 1 & collision.collider.gameObject.layer == 3)
        {
            dashCount = maxDashCount;
            isOnGround = true;
            canWallJump = false;
        }


        if (gameObject.layer == 7 && collision.collider.gameObject.layer == 8)
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
            playerRb2D.velocity = Vector2.zero;
            StartCoroutine(DontMove());
            playerRb2D.AddForce(collisionRecoil * jumpForce / 1.8f, ForceMode2D.Impulse);
        }

        if (gameObject.layer == 7 && collision.collider.gameObject.layer == 11)
        {
            playerRb2D.velocity = Vector2.zero;
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
            dashCount = maxDashCount;
            isOnGround = true;
            canWallJump = false;
        }

        if (collision.collider.gameObject.layer == 6 && isOnGround == false)
        {
            dashCount = maxDashCount;
            playerRb2D.gravityScale = 1;
            wallJump = collision.GetContact(0).normal * 1.2f + Vector2.up * 1.5f;
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
            movement = movementInput * movementSpeed;
            canWallJump = false;
            isOnGroundWall = false;
        }
    }

    void Jump()
    {
        movement.y += 500;
    }

    IEnumerator DontMove()
    {
        canMove = false;
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }
}
