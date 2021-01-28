using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    private Vector2Int foodGridPosition;
    private int width;
    private int height;
    private Snake snake;
    public GameObject food;
    private GameObject tempFood;
    public GameObject borderLeft;
    public GameObject borderRight;
    public GameObject borderTop;
    public GameObject borderBottom;
    public Transform camera;
    public Transform console;
    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Setup(int size, Snake snake)
    {
        this.width = size;
        this.height = size;
        this.snake = snake;
        tempFood = food;

        SpawnFood();
    }

    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        }
        while (snake.GetFullSnakeGridPosition().IndexOf(foodGridPosition) != -1);
        // while (snake.GetGridPosition() == foodGridPosition);

        tempFood = Instantiate(food, new Vector2(foodGridPosition.x, foodGridPosition.y), Quaternion.identity);
    }

    public bool SnakeMovedAndAte(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(tempFood);
            SpawnFood();
            return true;
        } else
        {
            return false;
        }
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if(gridPosition.x < 0)
        {
            gridPosition.x = width - 1;
        }
        if (gridPosition.x > width - 1)
        {
            gridPosition.x = 0;
        }

        if (gridPosition.y < 0)
        {
            gridPosition.y = height - 1;
        }
        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }

    public void ResizeGrid()
    {
        if(width > 2 && height > 2)
        {
            width--;
            height--;

            if (foodGridPosition.x >= width - 1)
            {
                foodGridPosition = new Vector2Int(foodGridPosition.x - 1, foodGridPosition.y);
            }
            if (foodGridPosition.y >= height - 1)
            {
                foodGridPosition = new Vector2Int(foodGridPosition.x, foodGridPosition.y - 1);
            }
            tempFood.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);

            // Move Camera
            camera.position = new Vector3(camera.position.x - 0.5f, camera.position.y, camera.position.z);
            // Move Console
            console.position = new Vector3(console.position.x - 0.5f, console.position.y, console.position.z);

            // Decrease Borders:
            // Top Border
            borderTop.transform.position = new Vector3(borderTop.transform.position.x, borderTop.transform.position.y - 1, borderTop.transform.position.z);
            int last = borderTop.GetComponentsInChildren<Transform>().Length;
            Destroy(borderTop.GetComponentsInChildren<Transform>()[last - 2].gameObject);
            Transform lastS = borderTop.GetComponentsInChildren<Transform>()[last - 1];
            lastS.position = new Vector3(lastS.position.x - 1, lastS.position.y, lastS.position.z);
            // Bottom Border
            last = borderBottom.GetComponentsInChildren<Transform>().Length;
            Destroy(borderBottom.GetComponentsInChildren<Transform>()[last - 2].gameObject);
            lastS = borderBottom.GetComponentsInChildren<Transform>()[last - 1];
            lastS.position = new Vector3(lastS.position.x - 1, lastS.position.y, lastS.position.z);
            // Left Border
            last = borderLeft.GetComponentsInChildren<Transform>().Length;
            Destroy(borderLeft.GetComponentsInChildren<Transform>()[last - 1].gameObject);
            // Right Border
            borderRight.transform.position = new Vector3(borderRight.transform.position.x - 1, borderRight.transform.position.y, borderRight.transform.position.z);
            last = borderRight.GetComponentsInChildren<Transform>().Length;
            Destroy(borderRight.GetComponentsInChildren<Transform>()[last - 1].gameObject);
        }
        
    }
}
