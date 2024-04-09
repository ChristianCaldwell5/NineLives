using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : MonoBehaviour
{

    public float speed = 3.0f;
    private bool targetInRange = false;
    public Transform[] moveSpots;
    public int spotIndex = 0;

    private bool movingRight = false;
    private SpriteRenderer enemySr;
    private GameManager gameManager;
    private bool isOnCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        enemySr = gameObject.GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isActive)
        {
            if (!targetInRange)
            {
                // transform object towards current move spot
                transform.position = Vector2.MoveTowards(transform.position, moveSpots[spotIndex].position, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, moveSpots[spotIndex].position) < 0.2f)
                {
                    DetermineSpotIndex();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isOnCooldown)
            {
                targetInRange = true;
                transform.position = Vector2.MoveTowards(transform.position, collision.gameObject.transform.position, speed * Time.deltaTime);
            } else
            {
                targetInRange = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetInRange = false;
            DetermineSpotIndex();
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