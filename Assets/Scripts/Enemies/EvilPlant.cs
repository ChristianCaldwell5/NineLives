using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilPlant : MonoBehaviour
{

    public float shotInterval = 4;
    public GameObject bulletPrefab;
    private GameManager gameManager;

    private Animator plantAnimator;
    private SpriteRenderer plantSr;
    private float shotAnimationTime = 0.667f;
    private Vector3 leftFacingSpawnOffset = new Vector3(-0.09299994f, 0.056f, 0);
    private Vector3 rightFacingSpawnOffset = new Vector3(0.085f, 0.056f, 0);
    private bool isLeftFacing = true;

    // Start is called before the first frame update
    void Start()
    {
        plantAnimator = gameObject.GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        StartCoroutine(ShotInterval());
        plantSr = gameObject.GetComponent<SpriteRenderer>();

        if (plantSr.flipX)
        {
            isLeftFacing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 DetermineBulletSpawnPos()
    {
        if (isLeftFacing)
        {
            return transform.position + leftFacingSpawnOffset;
        } else
        {
            return transform.position + rightFacingSpawnOffset;
        }
    }

    private Quaternion DetermineBulletSpawnRotation()
    {
        if (isLeftFacing)
        {
            return bulletPrefab.transform.rotation;
        }
        else
        {
            return Quaternion.Euler(0, 180, 0);
        }
    }

    IEnumerator ShotInterval()
    {
        while(gameManager.isActive)
        {
            yield return new WaitForSeconds(shotInterval);
            plantAnimator.SetBool("isFiring", true);
            yield return new WaitForSeconds(shotAnimationTime/2);
            Instantiate(bulletPrefab, DetermineBulletSpawnPos(), DetermineBulletSpawnRotation());
            yield return new WaitForSeconds(shotAnimationTime/2);
            plantAnimator.SetBool("isFiring", false);
        }
    }
}
