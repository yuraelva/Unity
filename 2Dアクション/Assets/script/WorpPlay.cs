using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class WorpPlay : MonoBehaviour
{
    public VideoPlayer worpplay;
    public GameObject cooltime;
    public bool canworp = true;
    float PX, PY,dec;
    // Start is called before the first frame update
    void Start()
    {
        worpplay.loopPointReached += LoopPointReached;
        dec = 0.016f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canworp)
        {
            cooltime.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 1f);
            DecreaseCoolTime();
        }
        if (cooltime.GetComponent<Image>().fillAmount <= 0)
        {
            cooltime.GetComponent<Image>().fillAmount = 1;
            cooltime.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 0f);
            canworp = true;
        }
    }
    public bool getsetcanworp
    {
        get { return canworp; }
        set { canworp = value; }

    }
    public void PlayMovie(float PlayerX, float PlayerY)
    {
        canworp = false;
        PX = PlayerX; PY = PlayerY;
        worpplay.Play();
        StartCoroutine(DelayMethod(10, () =>
        {
            transform.localScale = new Vector2(1.5f, 1.5f);
            transform.position = new Vector2(PX, PY);
        }));
    }
    IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }
    public void LoopPointReached(VideoPlayer vp)
    {
        transform.position = new Vector2(-20, 0);
        
        worpplay.Stop();
    }
    void DecreaseCoolTime()
    {
        cooltime.GetComponent<Image>().fillAmount -= dec;
    }
}
