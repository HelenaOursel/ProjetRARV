using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool isStarted = false;
    public float timer;
    private TextMeshProUGUI count;
    private GameObject block;

    private void Start()
    {
        count = GetComponent<TextMeshProUGUI>();
        block = GameObject.Find("StartDelimitation");
        timer = 0;
    }
    void Update()
    {
        if(isStarted == false)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeRemaining / 60F);
                int seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);

                string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
                count.text = niceTime;
            }
            else
            {
                count.text = "Go !";
                block.SetActive(false);
                StartCoroutine(Timer());
            }
        }
        else
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);

            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            count.text = niceTime;
        }
       
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3);
        isStarted = true;
    }
}
