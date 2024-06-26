using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 cameraOffset = new(0, 0, -10);

    private AudioSource objectAs;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player)
        {
            // follow position with camera offset
            transform.position = player.transform.position + cameraOffset;
        }
    }
}
