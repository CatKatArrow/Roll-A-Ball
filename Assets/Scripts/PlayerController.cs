using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    private Rigidbody rb;

    private int count;
    private int winCount;
    float timer;
    public bool won;
    GameObject resetPoint;
    bool resetting = false;
    Color originalColour;

    [Header("UI Stuff")]
    public GameObject gameOverScreen;
    public TMP_Text countText;
    public TMP_Text winText;
    public TMP_Text timerText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        gameOverScreen.SetActive(false);
        winCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;

        countText.text = "Count: " + count + " / " + winCount;
        winText.text = "";

        resetPoint = GameObject.Find("Reset Point");
        originalColour = GetComponent<Renderer>().material.color;

        timer = 0;
        won = false;

        Time.timeScale = 1;

    }

    void Update()
    {
        if(won == false)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + timer.ToString("F2");
        }
    }

    void FixedUpdate()
    {
        if (resetting)
            return;

        float moveHorizintal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizintal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);   
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pick Up"))
        {
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
        gameOverScreen.SetActive(true);
        winText.text = "You Win!\n" + "<color=#ff63AE69><size=50>" + "Your Time: " + timer.ToString("F3");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Respawn"))
        {
            StartCoroutine(ResetPlayer());
        }
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
    }
}
