using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAttackGenerate : MonoBehaviour
{
    GameObject player;
    float attack = 10;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayMethod(1000, () =>
        {
            Destroy(gameObject);
        }));
        player = GameObject.Find("player");
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
            player.GetComponent<PlayerController>().DecreaseHP(attack);
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
