using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool shootLeft = true;
    private float speed = 5.0f;
    // ANIMATION is disabled for now
    //private Animator bulletAnimator;
    //private float destroyAnimationTime = 0.167f;

    // Start is called before the first frame update
    void Start()
    {
        //bulletAnimator = gameObject.GetComponent<Animator>();
        //bulletAnimator.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle")
            || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            //StartCoroutine(AnimateAndDestroy());
        }

    }

    //IEnumerator AnimateAndDestroy()
    //{
    //    bulletAnimator.enabled = true;
    //    yield return new WaitForSeconds(destroyAnimationTime);
    //    Destroy(gameObject);
    //}
}
