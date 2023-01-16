using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartucleDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayMethod(1000, () =>
        {
            Destroy(gameObject);
        }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player")
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "floor")
        {
            Destroy(gameObject);
        }
    }
    IEnumerator DelayMethod(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }
}
