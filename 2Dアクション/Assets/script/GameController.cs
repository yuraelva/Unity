using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.UI;

public class GameController : MonoBehaviour
{
    Camera cam;
    public RectTransform oc;
    public CameraController cameracontroller;
    public PlayerController playercontroller;
    public MovieFlagAria moviemlagmria;
    bool movieflag = false,movie1 = true,movie2 = false;
    float delta = 0, span = 0.03f;
    /*public void option()
    {
        Debug.Log("オプション画面に遷移");
        oc.position = new Vector3(-oc.position.x, -oc.position.y,0);
    }*/
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        movieflag = moviemlagmria.getFlag();
        if (movieflag)
        {
            Debug.Log("イベント");
            playercontroller.canplay = false;
            //一定時間事にカメラを上に向ける
            if(movie1)delta += Time.deltaTime;
            if (delta > span)
            {
                delta = 0;
                Debug.Log("カメラを上に向ける");
                cameracontroller.MoveFlag = false;
                StartCoroutine(DelayMethod(60, () =>
                {
                    cam.transform.Translate(0, 0.1f, 0);
                }));
                //カメラを上に向ける
                StartCoroutine(DelayMethod(180, () =>
                {
                    movie1 = false;
                    //タイトルの表示
                }));
            }
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
