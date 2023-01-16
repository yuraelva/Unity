using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBullet : MonoBehaviour
{
    public PlayerController playercontroller;
    public EnemyController enemycontroller;
    public GameObject enemyPrefab;
    public GameObject attackParticlePrefab;
    Vector2 playerPos,launchDirection,generatePos;
    public float force = 1.2f;
    // Start is called before the first frame update
    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
 
    }
    public void BulletGenerate(float generatePosX, float generatePosY)
    {
        generatePos = new Vector3(generatePosX + transform.position.x, generatePosY + transform.position.y);
        playerPos = playercontroller.transform.position;
        launchDirection = new Vector2(playerPos.x - generatePos.x, playerPos.y - generatePos.y);
        GameObject go = Instantiate(enemyPrefab);
        GameObject atParticle = Instantiate(attackParticlePrefab);
        go.transform.position = generatePos;
        atParticle.transform.position = go.transform.position;
        StartCoroutine(DelayMethod(60, () =>
        {
            go.GetComponent<Rigidbody2D>().velocity = force * launchDirection;
            atParticle.GetComponent<Rigidbody2D>().velocity = force * launchDirection;

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
}
