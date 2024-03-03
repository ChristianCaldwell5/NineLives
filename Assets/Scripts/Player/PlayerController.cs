using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpAmount = 5.0f;
    public AudioClip jumpAudioClip;
    public AudioClip hitClip;
    public AudioClip collectionClip;
    public GameObject hitIndicator;
    public List<RuntimeAnimatorController> catAnimationControllers;
    public List<Sprite> jumpSprites;

    private GameManager gameManager;

    // components
    private Animator playerAnimator;
    private SpriteRenderer playerSr;
    private Rigidbody2D playerRb;
    private AudioSource playerAs;
    private Renderer playerRenderer;
    // state
    private int selectedCat;
    private bool isGrounded = true;
    private bool isImmune = false;

    // Start is called before the first frame update
    void Start()
    {
        //selectedCat = MainManager.Instance.SelectedCat;
        selectedCat = 0;
        playerAnimator = gameObject.GetComponent<Animator>();
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        playerSr = gameObject.GetComponent<SpriteRenderer>();
        playerAs = gameObject.GetComponent<AudioSource>();
        playerRenderer = gameObject.GetComponent<Renderer>();
        // set player animator by selected cat
        playerAnimator.runtimeAnimatorController = catAnimationControllers[selectedCat];

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        // check for space bar press to jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && playerRb.velocity.magnitude <= 1)
        {
            Debug.Log("Jumping: " + playerRb.velocity.magnitude);
            playerAnimator.enabled = false;
            playerSr.sprite = jumpSprites[selectedCat];
            playerRb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
            playerAs.PlayOneShot(jumpAudioClip, 1.0f);
            isGrounded = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isActive)
        {
            // get horizontal input (Associated keys with horizontal movement in Input Manager)
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            playerAnimator.SetFloat("Speed_f", Mathf.Abs(horizontalInput));
            if (horizontalInput > 0)
            {
                playerSr.flipX = false;
            }
            if (horizontalInput < 0)
            {
                playerSr.flipX = true;
            }

            // move the player according to input
            transform.Translate(horizontalInput * moveSpeed * Time.deltaTime * Vector2.right);
        }
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle")
            || collision.gameObject.CompareTag("Platform"))
        {
            // if player is above the ground, then set isGrounded to true
            if (transform.position.y > collision.gameObject.transform.position.y + collision.gameObject.GetComponent<Collider2D>().bounds.size.y - 0.04f)
            {
                Debug.Log("Player is grounded");
                isGrounded = true;
                playerAnimator.enabled = true;
            }
        }

        if (collision.gameObject.CompareTag("Bullet") && !isImmune)
        {
            Debug.Log("Bullet HIT!");
            HandleHit();
        }

    }

    private void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO: create a function that uses a switch on basis of tag - then do unique sounds, knowckback etc.
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spike")
            || collision.gameObject.CompareTag("Flame")) && !isImmune)
        {
            Debug.Log("Enemy HIT!");
            HandleHit();
        } else if (collision.gameObject.CompareTag("Fruit"))
        {
            gameManager.IncrementFruitCount();
            playerAs.PlayOneShot(collectionClip, 20.0f);
            StartCoroutine(AnimateThenDestroy(collision.gameObject));
        }
    }

    private void HandleHit()
    {
        gameManager.UpdateLivesCount(-1);
        playerAs.PlayOneShot(hitClip, 1.0f);
        CheckForGameOver();
        StartCoroutine(DisableHitIndicator());
        StartCoroutine(IsImmune());
    }

    private void CheckForGameOver()
    {
        if (gameManager.GetLivesCount() <= 0)
        {
            playerAnimator.enabled = false;
            gameManager.InitiateGameOver();
        }
    }

    IEnumerator IsImmune()
    {
        isImmune = true;
        Color hitIndicationColor = new Color32(255, 255, 255, 100);
        for (var n = 0; n < 5; n++)
        {
            playerRenderer.material.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            playerRenderer.material.color = hitIndicationColor;
            yield return new WaitForSeconds(0.1f);
        }
        isImmune = false;
        playerRenderer.material.color = Color.white;
    }


    IEnumerator AnimateThenDestroy(GameObject obj)
    {
        obj.GetComponent<Animator>().SetBool("isCollected", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }

    IEnumerator DisableHitIndicator()
    {
        hitIndicator.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        hitIndicator.SetActive(false);
    }

}
