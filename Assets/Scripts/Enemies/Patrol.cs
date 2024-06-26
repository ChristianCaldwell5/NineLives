using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float activeSpeed = 5.0f;
    private float speed;
    public Transform[] moveSpots;
    public bool isDynamicWait = true;

    // components
    private SpriteRenderer enemySr;
    private Animator enemyAnimator;
    public int spotIndex = 0;
    private float waitTime;
    private float lowWait = 1.0f;
    private float highWait = 2.0f;
    private bool movingRight = true;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        speed = activeSpeed;
        enemySr = gameObject.GetComponent<SpriteRenderer>() != null ? gameObject.GetComponent<SpriteRenderer>() : null;
        enemyAnimator = gameObject.GetComponent<Animator>() != null ? gameObject.GetComponent<Animator>() : null;
        if (isDynamicWait)
        {
            waitTime = Random.Range(lowWait, highWait);
        } else
        {
            waitTime = 2.0f;
        }  
        //DetermineSpotIndex();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isActive)
        {
            if (enemyAnimator)
            {
                //float horizontalInput = Input.GetAxisRaw("Horizontal");
                enemyAnimator.SetFloat("Speed_f", Mathf.Abs(speed));
            }

            // transform object towards current move spot
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[spotIndex].position, speed * Time.deltaTime);

            // check if object has reach current move spot
            if (Vector2.Distance(transform.position, moveSpots[spotIndex].position) < 0.2f)
            {
                speed = 0;
                // wait the specified time
                if (waitTime <= 0)
                {
                    speed = activeSpeed;
                    // determine next index
                    DetermineSpotIndex();
                    if (isDynamicWait)
                    {
                        waitTime = Random.Range(lowWait, highWait);
                    } else
                    {
                        waitTime = 2.0f;
                    }
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
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
                if (enemySr)
                {
                    enemySr.flipX = true;
                }
                
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
                if (enemySr)
                {
                    enemySr.flipX = false;
                }
            }
            else
            {
                spotIndex--;
            }
        }
    }
}
