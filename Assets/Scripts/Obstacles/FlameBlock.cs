using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBlock : MonoBehaviour
{

    public Sprite activationWarningSprite;
    public Sprite disabledSprite;
    public float startDelay = 0;
    public float onAndOffInterval = 3;
    public GameObject flameTrigger;

    private Animator flameAnimator;
    private SpriteRenderer flameSr;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        flameAnimator = gameObject.GetComponent<Animator>();
        flameAnimator.enabled = false;
        flameSr = gameObject.GetComponent<SpriteRenderer>();
        flameTrigger.SetActive(false);
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        StartCoroutine(delayBeforeIntervalBegins());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator delayBeforeIntervalBegins()
    {
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(flameBlockInterval());
    }

    IEnumerator flameBlockInterval()
    {
        while(gameManager.isActive)
        {
            if (onAndOffInterval < 1)
            {
                onAndOffInterval = 2;
            }
            // interval minus one second warning wait
            float pauseTime = onAndOffInterval - 1;
            yield return new WaitForSeconds(pauseTime);
            // display warning sprite for the final half second
            flameSr.sprite = activationWarningSprite;
            yield return new WaitForSeconds(1);
            // begin flame animation and enable flam trigger for interval
            flameAnimator.enabled = true;
            flameTrigger.SetActive(true);
            yield return new WaitForSeconds(onAndOffInterval);
            // disable flame trigger and animator
            flameTrigger.SetActive(false);
            flameAnimator.enabled = false;
            flameSr.sprite = disabledSprite;
        }
    }
}
