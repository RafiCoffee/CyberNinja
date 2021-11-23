using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    Vector2 movement;
    Vector2 movementInput;
    Vector2 dashInput;
    Vector2 attackInput;
    Vector2 wallJump;
    Vector2 collisionRecoil;

    public int dashCount = 1;
    private int maxDashCount;

    public float movementSpeed = 30f;
    public float jumpForce = 8f;
    private float dashTime;
    public float startDashTime;

    public bool canMove;
    private bool isOnGround = true;
    private bool canWallJump = false;
    private bool isDashing = false;

    private Rigidbody2D playerRb2D;
    private BoxCollider2D playerBc2D;
    private Animator playerAnim;
    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        playerBc2D = GetComponent<BoxCollider2D>();
        playerAnim = GetComponent<Animator>();

        dashTime = startDashTime;
        maxDashCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movementInput.x = Input.GetAxis("Horizontal");

            dashInput.x = Input.GetAxisRaw("Horizontal");

            attackInput.x = Input.GetAxisRaw("Horizontal");
            attackInput.y = Input.GetAxisRaw("Vertical");

            transform.GetChild(1).localPosition = attackInput;

            movement = movementInput * movementSpeed * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) & isOnGround)
            {
                playerRb2D.gravityScale = 8;
                playerRb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isOnGround = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && dashInput != Vector2.zero && dashCount != 0)
            {
                dashCount--;
                isDashing = true;
                playerRb2D.AddForce(dashInput * jumpForce / 1.8f, ForceMode2D.Impulse);
                playerRb2D.gravityScale = 0.5f;
            }

            if (isDashing)
            {
                dashTime -= Time.deltaTime;
                if (dashTime <= 0)
                {
                    playerRb2D.gravityScale = 8;
                    dashTime = startDashTime;
                    isDashing = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (attackInput == Vector2.down && isOnGround == false)
                {
                    playerAnim.SetTrigger("Attack");
                    playerRb2D.velocity = Vector2.zero;
                    playerRb2D.AddForce(Vector2.up * jumpForce / 2, ForceMode2D.Impulse);
                }
                else
                {
                    playerAnim.SetTrigger("Attack");
                }
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
    }

    private void FixedUpdate()
    {
        playerRb2D.transform.Translate(movement);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y == 1 & collision.collider.gameObject.layer == 3)
        {
            dashCount = maxDashCount;
            isOnGround = true;
            canWallJump = false;
        }

        if (collision.collider.gameObject.layer == 6)
        {
            playerRb2D.velocity = new Vector2(0, 0);
        }

        if (collision.collider.gameObject.layer == 8)
        {
            collisionRecoil = collision.GetContact(0).normal;
            playerRb2D.AddForce(-collisionRecoil * jumpForce, ForceMode2D.Impulse);
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

        if (collision.collider.gameObject.layer == 6 & isOnGround == false)
        {
            dashCount = maxDashCount;
            playerRb2D.gravityScale = 1;
            wallJump = collision.GetContact(0).normal / 1.5f + Vector2.up * 1.5f;
            canWallJump = true;
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
            playerRb2D.gravityScale = 8;
            movement = movementInput * movementSpeed * Time.deltaTime;
            canWallJump = false;
        }
    }
}
