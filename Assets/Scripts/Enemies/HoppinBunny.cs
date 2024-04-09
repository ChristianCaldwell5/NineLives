using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HoppinBunny : MonoBehaviour
{

    public float runSpeed = 5.0f;
    public Transform[] moveSpots;
    public float jumpAmount = 7.0f;

    // components
    private SpriteRenderer enemySr;
    //private Animator enemyAnimator;
    private Rigidbody2D enemyRb;
    public int spotIndex = 0;
    private bool movingRight = false;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        enemySr = gameObject.GetComponent<SpriteRenderer>();
        //enemyAnimator = gameObject.GetComponent<Animator>();
        enemyRb = gameObject.GetComponent<Rigidbody2D>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isActive)
        {
            //if (enemyAnimator)
            //{
            //    //float horizontalInput = Input.GetAxisRaw("Horizontal");
            //    enemyAnimator.SetFloat("Speed_f", Mathf.Abs(speed));
            //}
            // transform object towards current move spot
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[spotIndex].position, runSpeed * Time.deltaTime);

            // check if object has reach current move spot
            if (Vector2.Distance(transform.position, moveSpots[spotIndex].position) < 0.2f)
            {
                DetermineSpotIndex();
                if (enemyRb.simulated)
                {
                    enemyRb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle")
            || collision.gameObject.CompareTag("Platform")) && !enemyRb.simulated)
        {
            enemyRb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        }
            if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisableJumpAbility());
        }

    }

    void DetermineSpotIndex()
    {
        if (movingRight)
        {
            if (spotIndex + 1 >= moveSpots.Length)
            {
                spotIndex--;
                movingRight = false;
                enemySr.flipX = true;
            }
            else
            {
                spotIndex++;
            }
        }
        else
        {
            if (spotIndex - 1 < 0)
            {
                spotIndex++;
                movingRight = true;
                enemySr.flipX = false;
            }
            else
            {
                spotIndex--;
            }
        }
    }

    IEnumerator DisableJumpAbility()
    {
        enemyRb.simulated = false;
        yield return new WaitForSeconds(0.5f);
        enemyRb.simulated = true;
    }
}
