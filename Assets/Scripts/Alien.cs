using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public float speed = 10;

    public Rigidbody2D rb;
    public Sprite startingImage;
    public Sprite altImage;
    private SpriteRenderer sr;
    public float secBeforeSpriteChange = 0.5f;

    public GameObject alienBullet;
    public float minFireRateTime = 1.0f;
    public float maxFireRateTime = 3.0f;
    public float baseFireWaitTime = 3.0f;
    public float onAwakeTime = 0f;
    public Sprite explodedShipImage;

    private int currentDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.velocity = new Vector2(currentDirection, 0) * speed;

        // StartCoroutine(ChangeAlienSprite());
        onAwakeTime = 0f;
        baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, maxFireRateTime);
    }

    public void GlitchAlienMovement()
    {
        currentDirection *= -1;
        if (rb.velocity.x == 1)
            rb.velocity = new Vector2(-1, 0) * speed;
        else
            rb.velocity = new Vector2(1, 0) * speed;
        // TurnAround(currentDirection);

    }

    public IEnumerator ChangeAlienSprite()
    {
        while (true)
        {
            if(sr.sprite == startingImage)
            {
                sr.sprite = altImage;
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.alienMove);
            }
            else
            {
                sr.sprite = startingImage;
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.alienMove);
            }

            yield return new WaitForSeconds(secBeforeSpriteChange);
        }
    }

    void TurnAround(int direction)
    {
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = speed * direction;
        rb.velocity = newVelocity;
    }

    void MoveDown()
    {
        Vector2 position = rb.position;
        position.y -= 1;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "LeftWall")
        {
            TurnAround(1);
            MoveDown();
        }

        if (col.gameObject.tag == "RightWall")
        {
            TurnAround(-1);
            MoveDown();
        }

        if (col.gameObject.tag == "Bullet")
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.alienShot);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        onAwakeTime += Time.deltaTime;
        if (onAwakeTime >= baseFireWaitTime)
        {
            baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, maxFireRateTime);
            Instantiate(alienBullet, transform.position, Quaternion.identity);
            onAwakeTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.gameOver);
            // col.GetComponent<SpriteRenderer>().sprite = explodedShipImage;
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }
}
