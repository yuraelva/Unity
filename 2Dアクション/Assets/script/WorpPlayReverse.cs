using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class WorpPlayReverse : MonoBehaviour
{
    public VideoPlayer worpplay;

    float PX, PY;
    // Start is called before the first frame update
    void Start()
    {
        worpplay.loopPointReached += LoopPointReached;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMovie(float PlayerX, float PlayerY)
    {
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
}
