using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSnack : MonoBehaviour
{
    private Animator fruitAnimator;

    // Start is called before the first frame update
    void Start()
    {
        fruitAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
    }
}
