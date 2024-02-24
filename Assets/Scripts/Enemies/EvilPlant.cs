using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilPlant : MonoBehaviour
{

    public float shotInterval = 4;
    public GameObject bulletPrefab;

    private Animator plantAnimator;
    private float shotAnimationTime = 0.667f;
    private Vector3 leftFacingSpawnOffset = new Vector3(-0.09299994f, 0.05499999f, 0);

    // Start is called before the first frame update
    void Start()
    {
        plantAnimator = gameObject.GetComponent<Animator>();
        StartCoroutine(ShotInterval());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShotInterval()
    {
        // todo: switch to while not game over logic
        while(true)
        {
            yield return new WaitForSeconds(shotInterval);
            plantAnimator.SetBool("isFiring", true);
            yield return new WaitForSeconds(shotAnimationTime/2);
            Instantiate(bulletPrefab, transform.position + leftFacingSpawnOffset, bulletPrefab.transform.rotation);
            yield return new WaitForSeconds(shotAnimationTime/2);
            plantAnimator.SetBool("isFiring", false);
        }
    }
}
