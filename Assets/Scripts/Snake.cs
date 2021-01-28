using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private enum State
    {
        Alive,
        Dead
    }

    public bool isGameGlitched;
    private State state;
    private LevelGrid levelGrid;
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    public float moveSpeed = 2f;
    public GameObject snakeBody;
    private int snakeBodySize;
    public List<Vector2Int> snakeMovePositionList;
    public List<GameObject> snakeBodyObjectList;
    public TMPro.TextMeshProUGUI scoreText;
    private int score;
    private bool isGameOver = false;

    public GameObject point;
    private int pointStartX = -10;
    private int pointStartY = 9;
    private int pointEndY = -5;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    private void Awake()
    {
        gridPosition = new Vector2Int(0, 0);
        gridMoveTimerMax = 1f / moveSpeed;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeBodyObjectList = new List<GameObject>();
        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 0;
        state = State.Alive;
        score = 0;
    }

    public bool isGameOverYet()
    {
        if (state == State.Dead)
            return true;
        else
            return false;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPosition()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }

    void Update()
    {
        if(state == State.Alive)
        {
            // Inputs
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (gridMoveDirection.y != -1)
                {
                    transform.GetComponent<SpriteRenderer>().flipY = false;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    gridMoveDirection.x = 0;
                    gridMoveDirection.y = +1;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (gridMoveDirection.y != +1)
                {
                    transform.GetComponent<SpriteRenderer>().flipY = true;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    gridMoveDirection.x = 0;
                    gridMoveDirection.y = -1;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (gridMoveDirection.x != +1)
                {
                    transform.GetComponent<SpriteRenderer>().flipY = true;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    gridMoveDirection.x = -1;
                    gridMoveDirection.y = 0;
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (gridMoveDirection.x != -1)
                {
                    transform.GetComponent<SpriteRenderer>().flipY = false;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                    gridMoveDirection.x = +1;
                    gridMoveDirection.y = 0;
                }
            }

            // Movements
            gridMoveTimer += Time.deltaTime;
            if (gridMoveTimer >= gridMoveTimerMax)
            {
                gridMoveTimer -= gridMoveTimerMax;
                snakeMovePositionList.Insert(0, gridPosition);
                gridPosition += gridMoveDirection;
                gridPosition = levelGrid.ValidateGridPosition(gridPosition);
                AudioManager.Instance.PlayOneShot(AudioManager.Instance.snakeMove);

                bool snakeAteFood = levelGrid.SnakeMovedAndAte(gridPosition);
                if (snakeAteFood)
                {
                    AudioManager.Instance.PlayOneShot(AudioManager.Instance.snakeEat);
                    if (!isGameGlitched)
                    {
                        // Normal Game: Grow Body
                        snakeBodySize++;
                        GameObject temp = Instantiate(snakeBody, new Vector3(gridPosition.x + gridMoveDirection.x, gridPosition.y + gridMoveDirection.y), Quaternion.identity);
                        snakeBodyObjectList.Add(temp);
                        score++;
                        if(pointStartY-score >= pointEndY)
                        {
                            Instantiate(point, new Vector3(pointStartX, pointStartY - score), Quaternion.identity);
                        } else
                        {
                            pointStartX++;
                            //pointStartY = 9;
                            score = 1;
                            Instantiate(point, new Vector3(pointStartX, pointStartY - score), Quaternion.identity);
                        }
                        
                    }
                    else
                    {
                        // Glitched Game: Shrink Body
                        if(snakeBodySize > 0)
                        {
                            snakeBodySize--;
                            try
                            {
                                Destroy(snakeBodyObjectList[snakeBodyObjectList.Count - 1]);
                                snakeBodyObjectList.RemoveAt(snakeBodyObjectList.Count - 1);
                                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
                            }
                            catch (MissingComponentException e) { }
                        }
                        score+=2;
                        if (pointStartY - score >= pointEndY)
                        {
                            Instantiate(point, new Vector3(pointStartX, pointStartY - score), Quaternion.identity);
                            Instantiate(point, new Vector3(pointStartX, pointStartY - score + 1), Quaternion.identity);
                        }
                        else
                        {
                            pointStartX++;
                            //pointStartY = 9;
                            score = 2;
                            Instantiate(point, new Vector3(pointStartX, pointStartY - score), Quaternion.identity);
                            Instantiate(point, new Vector3(pointStartX, pointStartY - score + 1), Quaternion.identity);
                        }
                        
                    }
                }

                if (snakeMovePositionList.Count >= snakeBodySize + 1)
                {
                    snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
                }

                foreach (GameObject sBodyPart in snakeBodyObjectList)
                {
                    Vector2Int snakeBodyPartGridPos = new Vector2Int(
                        Mathf.CeilToInt(sBodyPart.transform.position.x),
                        Mathf.CeilToInt(sBodyPart.transform.position.y));
                    if (gridPosition == snakeBodyPartGridPos)
                    {
                        // Game Over!
                        state = State.Dead;
                        Debug.Log("Game Over");
                    }
                }

                transform.position = new Vector3(gridPosition.x, gridPosition.y);

                for (int i = 0; i < snakeBodyObjectList.Count; i++)
                {
                    Vector3 snakeBodyPos = new Vector3(snakeMovePositionList[i].x, snakeMovePositionList[i].y);
                    snakeBodyObjectList[i].transform.position = snakeBodyPos;
                }
            }
        }
        if(state == State.Dead && !isGameOver)
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.gameOver);
            isGameOver = true;
        }
        scoreText.text = score.ToString();
    }
}
