using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
   private enum State
    {
        Alive,
        Dead
    }

    public float speed = 5;
    public GameObject theBullet;
    public bool isGameGlitched;
    private State state;
    private bool isGameOver = false;
    private int score = 0;
    public GameObject point;
    private int pointStartX = -12;
    private int pointStartY = 10;
    private int pointEndY = -6;

    public bool isGameOverYet()
    {
        if (state == State.Dead)
            return true;
        else
            return false;
    }

    public void GetHit()
    {
        state = State.Dead;
    }

    public void AddPoints()
    {
        if (pointStartY - score >= pointEndY)
        {
            Instantiate(point, new Vector3(pointStartX, pointStartY - score), Quaternion.identity);
        }
        else
        {
            pointStartX++;
            //pointStartY = 9;
            score = 1;
            Instantiate(point, new Vector3(pointStartX, pointStartY - score), Quaternion.identity);
        }
        score++;
    }

    private void Update()
    {
        if (state == State.Dead && !isGameOver)
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.gameOver);
            isGameOver = true;
        }
        if (state == State.Alive)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Instantiate(theBullet, transform.position, Quaternion.identity);
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.playerShot);
            }
            float horzMove = Input.GetAxisRaw("Horizontal");
            GetComponent<Rigidbody2D>().velocity = new Vector2(horzMove, 0) * speed;
        }
        
    }

}
