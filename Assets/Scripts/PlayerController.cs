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

    // components
    private Animator playerAnimator;
    private SpriteRenderer playerSr;
    private Rigidbody2D playerRb;
    private AudioSource playerAs;
    // state
    private float leftBound = -8.0f;
    private bool isGrounded = true;
    //private bool isIdle = true;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        playerSr = gameObject.GetComponent<SpriteRenderer>();
        playerAs = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
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
        //playerAnimator.SetInteger("Speed_i", Mathf.Abs(horizontalInput));

        // keep the player in bounds
        if (transform.position.x < leftBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, transform.position.z);
        }

        // check for space bar press to jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
            playerAs.PlayOneShot(jumpAudioClip, 1.0f);
            isGrounded = false;
            playerAnimator.enabled = false;
        }

        // move the player according to input
        transform.Translate(horizontalInput * moveSpeed * Time.deltaTime * Vector2.right);
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
            playerAnimator.enabled = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spike"))
        {
            Debug.Log("Hit enemy!!!");
            playerAs.PlayOneShot(hitClip, 1.0f);
            StartCoroutine(DisableHitIndicator());
            // todo: add some kind of knockback
            //playerRb.AddForce(Vector2.up * 1.0f, ForceMode2D.Impulse);
        } else if (collision.gameObject.CompareTag("Fruit"))
        {
            // add particle thing
            playerAs.PlayOneShot(collectionClip, 20.0f);
            StartCoroutine(AnimateThenDestroy(collision.gameObject));
        }
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
