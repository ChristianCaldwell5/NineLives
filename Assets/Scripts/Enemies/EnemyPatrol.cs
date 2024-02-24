using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    const float DEFAULT_SPEED = 5.0f;
    private float speed = DEFAULT_SPEED;
    public Transform[] moveSpots;
    // components
    private SpriteRenderer enemySr;
    private Animator enemyAnimator;
    private int spotIndex = 0;
    private float waitTime;
    private float lowWait = 1.0f;
    private float highWait = 2.0f;
    private bool movingRight = true;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        enemySr = gameObject.GetComponent<SpriteRenderer>();
        enemyAnimator = gameObject.GetComponent<Animator>();
        waitTime = Random.Range(lowWait, highWait);
        spotIndex++;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isActive)
        {
            //float horizontalInput = Input.GetAxisRaw("Horizontal");
            enemyAnimator.SetFloat("Speed_f", Mathf.Abs(speed));

            // transform object towards current move spot
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[spotIndex].position, speed * Time.deltaTime);

            // check if object has reach current move spot
            if (Vector2.Distance(transform.position, moveSpots[spotIndex].position) < 0.2f)
            {
                speed = 0;
                // wait the specified time
                if (waitTime <= 0)
                {
                    speed = DEFAULT_SPEED;
                    // determine next index
                    DetermineSpotIndex();
                    waitTime = Random.Range(lowWait, highWait);
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
                enemySr.flipX = true;
            }
            else
            {
                spotIndex++;
            }
        }
        else
        {
            if (spotIndex - 1 <= 0)
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
}
