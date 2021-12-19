using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 1f;
    private Rigidbody rb;
    
    private int count;
    private int winCount;
    float timer;
    public bool won;
    GameObject resetPoint;
    public bool resetting = false;
    Color originalColour;
    bool grounded = true;

    CameraController cameraController;

    //Number for Lifes
    public int lifes = 3;

    [Header("UI Stuff")]
    public GameObject gameOverScreen;
    public GameObject nextLevelScreen;
    public TMP_Text countText;
    public TMP_Text winText;
    public TMP_Text timerText;
    public List<GameObject> healthIcons;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        gameOverScreen.SetActive(false);
        nextLevelScreen.SetActive(false);
        winCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;

        countText.text = "Count: " + count + " / " + winCount;
        winText.text = "";

        resetPoint = GameObject.Find("Reset Point");
        originalColour = GetComponent<Renderer>().material.color;

        cameraController = FindObjectOfType<CameraController>();

        timer = 0;
        won = false;

        Time.timeScale = 1;

        UpdateHealthIcons();
    }

    void Update()
    {
        if(won == false)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + timer.ToString("F2");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Jump");
            rb.AddForce(Vector3.up * jumpSpeed);
        }

    }

    void FixedUpdate()
    {
        if (resetting)
            return;

        if (grounded)
        {
            float moveHorizintal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizintal, 0.0f, moveVertical);

            if (cameraController.cameraStyle == CameraStyle.Free)
            {
                //rotates the player to the dirextion of the camera.
                transform.eulerAngles = Camera.main.transform.eulerAngles;
                //translates the input vectors into coordinates.
                movement = transform.TransformDirection(movement);
            }

            rb.AddForce(movement * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pick Up"))
        {
            if (resetting)
                return;

            SetCountText();
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    void SetCountText()
    {
        count++;
        countText.text = "Count: " + count + " / " + winCount;
        if (count == winCount)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        nextLevelScreen.SetActive(true);
        winText.text = "You Win!\n" + "<color=#ff63AE69><size=50>" + "Your Time: " + timer.ToString("F3");
        string currLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        char nextLevel = currLevel[currLevel.Length - 1];
        int nextInt = (int)char.GetNumericValue(nextLevel) + 1;
        PlayerPrefs.SetInt("Level" + nextInt, 1);
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Respawn"))
        {
            StartCoroutine(ResetPlayer());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            grounded = false;
    }

    public IEnumerator ResetPlayer()
    {
        resetting = true;
        GetComponent<Renderer>().material.color = Color.black;
        rb.velocity = Vector3.zero;
        Vector3 startPos = transform.position;
        float resetSpeed = 2f;
        var i = 0.0f;
        var rate = 1.0f / resetSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, resetPoint.transform.position, i);
            yield return null;
        }
        GetComponent<Renderer>().material.color = originalColour;
        resetting = false;
        ChangeLifes();
    }

    public void ChangeLifes()
    {
        lifes -= 1;
        UpdateHealthIcons();
        if (lifes == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverScreen.SetActive(true);
        resetting = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void UpdateHealthIcons()
    {
        for(int i =0; i < healthIcons.Count; i++)
        {
            healthIcons[i].SetActive(false);
        }

        for (int i = 0; i < lifes; i++)
        {
            healthIcons[i].SetActive(true);
        }
    }
}
