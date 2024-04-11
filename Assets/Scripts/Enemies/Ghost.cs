using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public float speed = 2.0f;
    public Transform[] moveSpots;
    public float visibilityDurr = 3.0f;
    public float invisibilityDurr = 1.0f;

    private SpriteRenderer enemySr;
    private Animator enemyAnimator;
    private BoxCollider2D enemyBc;
    public int spotIndex = 0;
    private bool movingRight = true;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        enemySr = gameObject.GetComponent<SpriteRenderer>();
        enemyBc = gameObject.GetComponent<BoxCollider2D>();
        enemyAnimator = gameObject.GetComponent<Animator>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        StartCoroutine(GhostVisibility());
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
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[spotIndex].position, speed * Time.deltaTime);

            // check if object has reach current move spot
            if (Vector2.Distance(transform.position, moveSpots[spotIndex].position) < 0.2f)
            {
                DetermineSpotIndex();
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

    IEnumerator GhostVisibility()
    {
        while(gameManager.isActive)
        {
            yield return new WaitForSeconds(visibilityDurr);
            enemyAnimator.SetBool("isAwake", false);
            enemyAnimator.SetBool("isFading", true);
            yield return new WaitForSeconds(0.333f); // fade animation dur
            enemySr.enabled = false;
            enemyBc.enabled = false;
            yield return new WaitForSeconds(invisibilityDurr);
            enemyAnimator.SetBool("isFading", false);
            enemySr.enabled = true;
            enemyBc.enabled = true;
            enemyAnimator.SetBool("isAppearing", true);
            yield return new WaitForSeconds(0.333f); // appear animation dur
            enemyAnimator.SetBool("isAppearing", false);
            enemyAnimator.SetBool("isAwake", true);
        }
    }

}
