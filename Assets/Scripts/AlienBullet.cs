using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    public float speed = 30;

    private Rigidbody2D rb;

    public Sprite explodedShipImage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.down * speed;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (col.tag == "Player")
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.playerShot);
            // col.GetComponent<SpriteRenderer>().sprite = explodedShipImage;
            Destroy(gameObject);
            col.GetComponent<SpaceShip>().GetHit();
            // Destroy(col.gameObject);
        }

        if (col.tag == "Shield")
        {
            // AudioManager.Instance.PlayOneShot(AudioManager.Instance.shieldDestroy);
            Destroy(gameObject);
            Destroy(col.gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
