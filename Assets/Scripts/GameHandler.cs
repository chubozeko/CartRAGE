using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField] public Snake snake;
    [SerializeField] public SpaceShip ship;
    public LevelGrid levelGrid;
    public int gridSize = 10;
    private GameState gameState;
    private float glitchTimer;
    private float normalTimer;
    public float glitchTimerMin = 3f;
    public float glitchTimerMax = 10f;
    private float glitchTimerValue;
    private float normalTimerValue = 5f;
    public List<GameObject> boundsAndConnections;
    private string gameMode;
    [Header("2_Space_Invaders Properties")]
    public List<GameObject> aliens;
    public List<GameObject> currentAliens;
    public GameObject alienGroup;
    public float alienSpeed = 3f;
    public List<GameObject> shields;
    public List<GameObject> currentShields;
    public GameObject shieldGroup;

    private enum GameState
    {
        Normal,
        Glitched
    }

    void Start()
    {
        glitchTimer = 0;
        normalTimer = 0;

        gameState = GameState.Normal;

        if (SceneManager.GetActiveScene().name.Equals("1_Snake"))
        {
            gameMode = "1_Snake";
            snake.Setup(levelGrid);
            levelGrid.Setup(gridSize, snake);
        }
        else if (SceneManager.GetActiveScene().name.Equals("2_Space_In"))
        {
            gameMode = "2_Space_In";
            // alienGroupCopy = alienGroup;
            SetupAliensAndShield();
        }

        glitchTimerValue = Random.Range(glitchTimerMin, glitchTimerMax);
    }

    private void SetupAliensAndShield()
    {
        // ALIENS
        foreach (GameObject a in aliens)
        {
            GameObject newAlien = Instantiate(a, new Vector2(a.transform.position.x, a.transform.position.y - 6), Quaternion.identity, alienGroup.transform);
            newAlien.SetActive(true);
            newAlien.GetComponent<Alien>().speed = alienSpeed;
            currentAliens.Add(newAlien);
        }

        // SHIELDS
        foreach (GameObject s in shields)
        {
            GameObject newShield = Instantiate(s, new Vector2(s.transform.position.x, s.transform.position.y - 16), Quaternion.identity, shieldGroup.transform);
            newShield.SetActive(true);
            currentShields.Add(newShield);
        }
    }

    private void Update()
    {
        if((snake != null && !snake.isGameOverYet()) || (ship != null && !ship.isGameOverYet()))
        {
            glitchTimer += Time.deltaTime;
            normalTimer += Time.deltaTime;

            if(gameMode == "2_Space_In")
            {
                if(alienGroup.transform.childCount == 0)
                {
                    currentAliens.Clear();
                    currentShields.Clear();
                    alienSpeed++;
                    SetupAliensAndShield();
                    // Remove a random shield
                    int i = Random.Range(0, currentShields.Count);
                    Destroy(currentShields[i]);
                    currentShields.RemoveAt(i);
                }
            }

            if (glitchTimer >= glitchTimerValue)
            {
                glitchTimer -= glitchTimerValue;
                normalTimer = 0;

                if (gameState == GameState.Normal)
                    gameState = GameState.Glitched;
                else if (gameState == GameState.Glitched)
                    gameState = GameState.Normal;

                SetGlitchedGameState();
                if (gameMode == "1_Snake")
                    ResizeGrid();
                else if (gameMode == "2_Space_In")
                    GlitchAliens();
                Debug.Log(gameState.ToString());
            }
            if (normalTimer >= normalTimerValue)
            {
                normalTimer -= normalTimerValue;
                
                if (gameState == GameState.Normal)
                    gameState = GameState.Glitched;
                else if (gameState == GameState.Glitched)
                    gameState = GameState.Normal;

                SetGlitchedGameState();
                if (gameMode == "1_Snake")
                    ResizeGrid();
                else if (gameMode == "2_Space_In")
                    GlitchAliens();

                Debug.Log(gameState.ToString());
                // Randomize Glitch timer
                glitchTimerValue = Random.Range(glitchTimerMin, glitchTimerMax);
            }
        }
        if (ship.isGameOverYet())
        {
            foreach(GameObject a in currentAliens)
            {
                if (a != null)
                    a.GetComponent<Rigidbody2D>().Sleep();
            }
        }
    }

    public void SetGlitchedGameState()
    {
        if (gameState == GameState.Glitched)
        {
            if (gameMode == "1_Snake")
                snake.isGameGlitched = true;
            else if (gameMode == "2_Space_In")
                ship.isGameGlitched = true;
        }
        else
        {
            if (gameMode == "1_Snake")
                snake.isGameGlitched = false;
            else if (gameMode == "2_Space_In")
                ship.isGameGlitched = false;
        }
    }

    public void ResizeGrid()
    {
        if (gameState == GameState.Glitched)
        {
            StartCoroutine(PlayGlitch());
            levelGrid.ResizeGrid();
        } else
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.normal);
            foreach (GameObject g in boundsAndConnections)
            {
                foreach (SpriteRenderer sr in g.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.color = new Color(0.00f, 0.78f, 0.93f);
                }
            }
        }
    }

    public void GlitchAliens()
    {
        if (gameState == GameState.Glitched)
        {
            StartCoroutine(PlayGlitch());
            foreach (GameObject a in currentAliens)
            {
                if(a != null)
                    a.GetComponent<Alien>().GlitchAlienMovement();
            }
        }
        else
        {
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.normal);
            foreach (GameObject g in boundsAndConnections)
            {
                foreach (SpriteRenderer sr in g.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.color = new Color(0.00f, 0.78f, 0.93f);
                }
            }

            foreach (GameObject a in currentAliens)
            {
                if (a != null)
                    a.GetComponent<Alien>().GlitchAlienMovement();
            }
        }
    }

    public IEnumerator PlayGlitch()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.glitch);

        foreach (GameObject g in boundsAndConnections)
        {
            foreach(SpriteRenderer sr in g.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(0.81f, 0.04f, 0.00f);
            }
        }
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject g in boundsAndConnections)
        {
            foreach (SpriteRenderer sr in g.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(0.00f, 0.78f, 0.93f);
            }
        }
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject g in boundsAndConnections)
        {
            foreach (SpriteRenderer sr in g.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(0.81f, 0.04f, 0.00f);
            }
        }
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject g in boundsAndConnections)
        {
            foreach (SpriteRenderer sr in g.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(0.00f, 0.78f, 0.93f);
            }
        }
        yield return new WaitForSeconds(0.2f);
        foreach (GameObject g in boundsAndConnections)
        {
            foreach (SpriteRenderer sr in g.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(0.81f, 0.04f, 0.00f);
            }
        }
    }

    

}
