using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    private BoxCollider2D skyCollider;
    private float imageHorizontalLength;

    // Start is called before the first frame update
    void Start()
    {
        skyCollider = GetComponent<BoxCollider2D>();
       imageHorizontalLength = skyCollider.size.x*transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -imageHorizontalLength)
        {
            RepositionBackground();
        }
    }

    private void RepositionBackground()
    {
        Vector2 groundOffset = new Vector2(imageHorizontalLength * 2f, 0);
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
