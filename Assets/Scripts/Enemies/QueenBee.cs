using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBee : MonoBehaviour
{

    public float speed = 4.0f;
    public GameObject bulletPrefab;
    private GameManager gameManager;
    private Animator beeAnimator;
    private SpriteRenderer beeSr;
    public Transform[] moveSpots;
    public int spotIndex = 0;
    private bool movingRight = true;
    public GameObject playerObject;
    private bool isFiring = false;
    private bool isShotCoolDown = false;

    private float animationDurr = 0.667f;

    // Start is called before the first frame update
    void Start()
    {
        beeSr = gameObject.GetComponent<SpriteRenderer>();
        beeAnimator = gameObject.GetComponent<Animator>();

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.isActive)
        {
            //if (beeAnimator)
            //{

            //}
            if (!isFiring && !isShotCoolDown)
            {
                FireIfInSight();
            }
            

            if (!isFiring)
            {
                transform.position = Vector2.MoveTowards(transform.position, moveSpots[spotIndex].position, speed * Time.deltaTime);
            }

            // check if object has reach current move spot
            if (Vector2.Distance(transform.position, moveSpots[spotIndex].position) < 0.2f)
            {
                DetermineSpotIndex();
            }
        }
    }

    void FireIfInSight()
    {
        if (Mathf.Abs(transform.position.x - playerObject.transform.position.x) < 0.2f
            && transform.position.y > playerObject.transform.position.y
            && Mathf.Abs(transform.position.y - playerObject.transform.position.y) < 8.0f)
        {
            StartCoroutine(ShotInterval());
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
            }
            else
            {
                spotIndex--;
            }
        }
    }

    private Vector3 DetermineBulletSpawnPos()
    {
        return transform.position + new Vector3(0, -0.327f, 0);
    }

    IEnumerator ShotInterval()
    {
        isFiring = true;
        beeAnimator.SetBool("IsFiring", true);
        yield return new WaitForSeconds(animationDurr/2);
        Instantiate(bulletPrefab, DetermineBulletSpawnPos(), bulletPrefab.transform.rotation);
        isFiring = false;
        beeAnimator.SetBool("IsFiring", false);
        // shot cool down
        isShotCoolDown = true;
        yield return new WaitForSeconds(2);
        isShotCoolDown = false;
    }
}
