using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;

public class NinjaController : MonoBehaviour
{
    [SerializeField, Range(0, 5)]
    int maxDashCount = 1;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    float jumpForce = 10f;

    private Vector2 movementInput;
    private Vector2 wallInput;
    private Vector2 movement;
    private Vector2 dashInput;
    private Vector2 attackInput;
    private Vector2 blockInput;
    private Vector2 wallJump;
    private Vector2 collisionRecoil;
    public Vector2 bulletCollision;

    private Quaternion playerRotation;

    private int dashCount;
    public int attackCount = 0;
    public int vida = 8;
    private int daño;

    private float movementAnim;
    private float dashTime;
    public float startDashTime;
    private float startGravity;
    public float blockCoolDown;

    public bool canMoveInGame;
    private bool jump;
    private bool canWallJump = false;
    private bool isOnGroundWall = false;
    private bool fromGroundWall = false;
    private bool isOnGround;
    private bool dash;
    private bool isDashing = false;
    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool canReturn = false;
    public bool bulletOnBack;
    public bool wallLeft = false;
    public bool wallRight = false;
    private bool invencible = false;
    public bool haveKey = false;
    private bool cantMove;

    public GameObject attackTrigger;
    public GameObject blockTrigger;
    public GameObject katanaHitParticleCollider;
    public GameObject katana;

    public Slider barraVida;

    public ParticleSystem wallDust;

    public TrailRenderer eyeR;
    public TrailRenderer eyeI;

    public Gradient ojosD;
    public Gradient ojos;

    public AudioClip katanaAirHit;
    public AudioClip dashClip;
    public AudioClip daño1;
    public AudioClip daño2;
    public AudioClip daño3;
    public AudioClip bullet;
    public AudioClip giveBullet;

    private Rigidbody2D playerRb2D;
    private BoxCollider2D playerBC;
    private Animator playerAnim;
    private AudioSource playerAudio;

    private Stopwatch blockTimer = new Stopwatch();

    // Start is called before the first frame update
    void Start()
    {
        playerRb2D = GetComponent<Rigidbody2D>();
        playerBC = GetComponent<BoxCollider2D>();
        playerAnim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        dashTime = startDashTime;
        dashCount = maxDashCount;
        startGravity = playerRb2D.gravityScale;

        blockTimer.Start();

        if (transform.position.y > 30)
        {
            haveKey = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!cantMove)
        {
            movementAnim = Input.GetAxisRaw("Horizontal");
            wallInput.x = Input.GetAxis("Horizontal");
            dashInput.x = Input.GetAxisRaw("Horizontal");

            barraVida.value = vida;

            if (dashCount == 0)
            {
                eyeR.colorGradient = ojos;
                eyeI.colorGradient = ojos;
            }
            else
            {
                eyeR.colorGradient = ojosD;
                eyeI.colorGradient = ojosD;
            }

            if (dashInput.x == 0)
            {
                dash = false;
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
                gameObject.layer = 14;
            }
            else
            {
                gameObject.layer = 7;
            }

            if (!canWallJump)
            {
                movementInput.x = Input.GetAxis("Horizontal");

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
                    wallDust.Stop();
                    fromGroundWall = false;
                }
            }
            else
            {
                if (isOnGroundWall)
                {
                    movementInput.x = Input.GetAxis("Horizontal");
                }
                else
                {
                    if (wallInput.x == 0.6f || wallInput.x == -0.6f)
                    {
                        movementInput.x = wallInput.x;
                    }
                }
            }

            playerAnim.SetBool("Wall", canWallJump);
            playerAnim.SetFloat("Blend", attackCount);

            if (canMoveInGame)
            {
                playerAnim.SetInteger("Movement", (int)movementAnim);
                playerAnim.SetBool("Ground", isOnGround);

                if (wallLeft && isOnGroundWall)
                {
                    transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (wallRight && isOnGroundWall)
                {
                    transform.GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
                }

                if (movementInput.x == 0)
                {
                    playerAnim.SetLayerWeight(1, 0);
                }
                else
                {
                    playerAnim.SetLayerWeight(1, 1);
                }

                if (attackInput != Vector2.zero && attackInput != Vector2.one && attackInput != -Vector2.one && attackInput != new Vector2(1, -1) && attackInput != new Vector2(-1, 1) && attackInput != Vector2.up)
                {
                    if (attackInput.y == -1 && !isOnGround)
                    {
                        transform.GetChild(1).localScale = new Vector2(4, 3);
                        transform.GetChild(1).localPosition = attackInput;
                    }
                    else if (attackInput == Vector2.right || attackInput == Vector2.left)
                    {
                        transform.GetChild(1).localScale = new Vector2(3, 4);
                        transform.GetChild(1).localPosition = attackInput * 2;
                        transform.GetChild(4).localPosition = attackInput * 2;
                    }
                }

                if (blockInput != Vector2.zero)
                {
                    transform.GetChild(2).localPosition = blockInput * 1.2f;
                }

                if (isDashing)
                {
                    if (!canWallJump)
                    {
                        gameObject.layer = 14;
                        dashTime -= Time.deltaTime;
                        if (dashTime <= 0)
                        {
                            gameObject.layer = 7;
                            playerRb2D.gravityScale = startGravity;
                            dashTime = startDashTime;
                            isDashing = false;
                        }
                    }
                    else
                    {
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

                if (canWallJump && !isOnGroundWall)
                {
                    isAttacking = true;
                    isBlocking = true;
                    wallDust.Play();
                    if (wallJump.x > 0)
                    {
                        transform.GetChild(0).rotation = Quaternion.Euler(0, 180, 0);
                        playerAnim.SetTrigger("Wall D");
                    }
                    else if (wallJump.x < 0)
                    {
                        transform.GetChild(0).rotation = Quaternion.Euler(0, 360, 0);
                        playerAnim.SetTrigger("Wall I");
                    }
                    attackInput.x = wallJump.x;
                    blockInput.x = wallJump.x;
                    dash = false;
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        playerAnim.SetTrigger("Jump");
                        wallDust.Stop();
                        canMoveInGame = false;
                        playerRb2D.AddForce(wallJump * jumpForce / 1.5f, ForceMode2D.Impulse);
                        canWallJump = false;
                        if (wallJump.x > 0)
                        {
                            transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
                        }
                        else if (wallJump.x > 0)
                        {
                            transform.GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
                        }
                    }
                }
                else if (!canWallJump && !isAttacking && !isBlocking && canMoveInGame)
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
                wallDust.Stop();
                if (jump)
                {
                    fromGroundWall = true;
                }
            }

            if (vida == 0)
            {
                gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(playerRb2D.velocity);
        }
    }

    void FixedUpdate()
    {
        if (canMoveInGame)
        {
            playerRb2D.velocity = new Vector2(movement.x, playerRb2D.velocity.y);

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
        }
        else
        {
            if (dash && dashInput != Vector2.zero && dashCount != 0)
            {
                canMoveInGame = true;
                dash = false;
                Dash();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y >= 0.9 && collision.collider.gameObject.layer == 3)
        {
            dashCount = maxDashCount;
            playerRb2D.gravityScale = startGravity;
            isOnGround = true;
            canMoveInGame = true;
            katana.SetActive(true);
            if (wallRight)
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (wallLeft)
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
            }
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
            if (collision.collider.gameObject.layer != 13 && !invencible)
            {
                StartCoroutine(Invencible());
                vida--;
            }
            StartCoroutine(DontMove());
            playerRb2D.AddForce(collisionRecoil * jumpForce / 2f, ForceMode2D.Impulse);
        }

        if (collision.collider.gameObject.layer == 6 && isOnGround == false && fromGroundWall == false)
        {
            canMoveInGame = true;
            playerRb2D.velocity = Vector2.zero;
        }

        if (gameObject.layer == 7 && collision.collider.gameObject.layer == 11)
        {
            bulletCollision = playerBC.offset;
            if (!invencible)
            {
                StartCoroutine(Invencible());
                vida--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.GetChild(1).gameObject.layer == 9 && collision.gameObject.layer == 8 && attackInput == Vector2.down && isOnGround == false)
        {
            dashCount = maxDashCount;
            playerRb2D.velocity = Vector2.zero;
            playerRb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerAnim.SetTrigger("SaltoEnemy");
        }

        if (isBlocking)
        {
            if (transform.GetChild(2).gameObject.layer == 10)
            {
                if (collision.gameObject.layer == 11)
                {
                    if (canReturn)
                    {
                        playerAudio.PlayOneShot(giveBullet, 1f);
                    }
                    else
                    {
                        playerAudio.PlayOneShot(bullet, 1f);
                    }
                }
            }
        }

        if (collision.gameObject.layer == 18)
        {
            vida = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.GetContact(0).normal.y >= 0.9 && collision.collider.gameObject.layer == 3)
        {
            playerRb2D.gravityScale = startGravity;
            dashCount = maxDashCount;
            isOnGround = true;
        }

        if (collision.GetContact(0).normal.y <= 0.5 && collision.collider.gameObject.layer == 6 && isOnGround == false && fromGroundWall == false)
        {
            dashCount = maxDashCount;
            playerRb2D.gravityScale = 0.2f;
            wallJump = collision.GetContact(0).normal + Vector2.up;
            canWallJump = true;
            katana.SetActive(false);
        }

        if (collision.collider.gameObject.layer == 6 && isOnGround)
        {
            playerRb2D.gravityScale = startGravity;
            isOnGroundWall = true;
        }

        if (collision.GetContact(0).normal.x == 1 && collision.collider.gameObject.layer == 6)
        {
            wallLeft = true;
            wallRight = false;
        }
        else if (collision.GetContact(0).normal.x == -1 && collision.collider.gameObject.layer == 6)
        {
            wallLeft = false;
            wallRight = true;
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
            katana.SetActive(true);
            isAttacking = false;
            isBlocking = false;
            if (wallRight)
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (wallLeft)
            {
                transform.GetChild(0).rotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!invencible)
        {
            StartCoroutine(Invencible());
            vida--;
        }
    }


    void Jump()
    {
        playerAnim.SetTrigger("Jump");
        wallDust.Stop();
        isOnGround = false;
        playerRb2D.velocity = Vector2.zero;
        playerRb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Dash()
    {
        playerAudio.PlayOneShot(dashClip, 1f);
        playerAnim.SetTrigger("Dash");
        wallDust.Stop();
        dashCount--;
        gameObject.layer = 14;
        isDashing = true;
        playerRb2D.velocity = Vector2.zero;
        movement = Vector2.zero;
        movement += dashInput * jumpForce * 1.5f;
        playerRb2D.gravityScale = 0.5f;
    }

    IEnumerator DontMove()
    {
        playerRb2D.gravityScale = startGravity;
        canMoveInGame = false;
        isDashing = false;
        yield return new WaitForSeconds(0.4f);
        canMoveInGame = true;
    }

    IEnumerator Invencible()
    {
        invencible = true;
        daño = Random.Range(0, 3);
        switch (daño)
        {
            case 0:
                playerAudio.PlayOneShot(daño1, 1f);
                break;

            case 1:
                playerAudio.PlayOneShot(daño2, 1f);
                break;

            case 2:
                playerAudio.PlayOneShot(daño3, 1f);
                break;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).gameObject.SetActive(true);
        invencible = false;
    }

    IEnumerator Attack()
    {
        playerAudio.PlayOneShot(katanaAirHit, 1f);
        if (attackInput.y == -1)
        {
            playerAnim.SetTrigger("DownAttack");
        }
        else
        {
            if (attackCount == 0)
            {
                attackCount = 1;
            }
            else
            {
                attackCount = 0;
            }
            playerAnim.SetTrigger("Attack");
        }
        isAttacking = true;
        attackTrigger.SetActive(true);
        katanaHitParticleCollider.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        isAttacking = false;
        attackTrigger.SetActive(false);
        katanaHitParticleCollider.SetActive(false);
    }

    IEnumerator Block()
    {
        playerAnim.SetTrigger("Block");
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

    public void NotMove()
    {
        cantMove = true;
    }

    public void StartMove()
    {
        cantMove = false;
    }

    public void NoSword()
    {
        katana.SetActive(false);
    }

    public void Sword()
    {
        katana.SetActive(true);
    }
}
