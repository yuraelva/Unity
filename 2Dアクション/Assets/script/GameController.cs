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
        Debug.Log("�I�v�V������ʂɑJ��");
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
            Debug.Log("�C�x���g");
            playercontroller.canplay = false;
            //��莞�Ԏ��ɃJ��������Ɍ�����
            if(movie1)delta += Time.deltaTime;
            if (delta > span)
            {
                delta = 0;
                Debug.Log("�J��������Ɍ�����");
                cameracontroller.MoveFlag = false;
                StartCoroutine(DelayMethod(60, () =>
                {
                    cam.transform.Translate(0, 0.1f, 0);
                }));
                //�J��������Ɍ�����
                StartCoroutine(DelayMethod(180, () =>
                {
                    movie1 = false;
                    //�^�C�g���̕\��
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
