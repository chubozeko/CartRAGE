using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30;

    private Rigidbody2D rb;

    public Sprite explodedAlienImage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (col.tag == "Alien")
        {
            FindObjectOfType<SpaceShip>().AddPoints();
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.alienShot);
            // col.GetComponent<SpriteRenderer>().sprite = explodedAlienImage;
            Destroy(gameObject);
            Destroy(col.gameObject);
        }

        if (col.tag == "Shield")
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.blockHit);
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
