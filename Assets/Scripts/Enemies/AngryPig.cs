using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPig : MonoBehaviour
{

    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public Transform[] moveSpots;
    private bool playerInSight = false;
    public GameObject playerObject;

    // components
    private SpriteRenderer enemySr;
    private Animator enemyAnimator;
    public int spotIndex = 0;
    private bool movingRight = true;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        enemySr = gameObject.GetComponent<SpriteRenderer>();
        enemyAnimator = gameObject.GetComponent<Animator>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // determine if in range and looking
        playerInSight = IsPlayerInSight();

        float speed = playerInSight ? runSpeed : walkSpeed;
        Debug.Log("Speed is: " + speed);
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
                DetermineSpotIndex();
            }
        }
    }

    bool IsPlayerInSight()
    {
        if (Mathf.Abs(Vector2.Distance(playerObject.transform.position, gameObject.transform.position)) < 7.0f
            && ((movingRight && playerObject.transform.position.x > gameObject.transform.position.x)
            || (!movingRight && playerObject.transform.position.x < gameObject.transform.position.x))
            && (Mathf.Abs(playerObject.transform.position.y - gameObject.transform.position.y) < 2.0f)
            )
        {
            return true;
        }
        else
        {
            return false;
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
                enemySr.flipX = false;
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
                enemySr.flipX = true;
            }
            else
            {
                spotIndex--;
            }
        }
    }
}
