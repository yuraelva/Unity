using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Camera cam;
    public GameObject titleText;
    public GameObject talk;
    public RectTransform oc;
    public CameraController cameracontroller;
    public PlayerController playercontroller;
    public EnemyController enemycontroller;
    bool iventflag = false;
    bool event1 = true, event2 = true, event3 = true,canTalkClick = true;
    //float delta = 0, span = 0.03f;
    int event2count = 0;
    Vector2 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(event1);
        if (iventflag)
        {
            Time.timeScale = 0f;
            playercontroller.canplay = false;
            
            if(-7.2f < playerPos.x && playerPos.x < 2.4f && event1)
            {
              //�������
                oc.position = new Vector3(-oc.position.x, -oc.position.y, 0);
                iventflag = false;
                event1 = false;
            }
            playercontroller.canplay = true;
            Time.timeScale = 1f;
            if (4.5f < playerPos.x && playerPos.x < 14f && event2)
            {
                Time.timeScale = 0f;
                playercontroller.canplay = false;
                Text talkText = talk.GetComponent<Text>();
                
                if (event2count == 0)
                {
                    //EditText("��,��,��,...";
                    EditText("��,��,��,...,",5);
                    event2count++;
                }
                if (Input.GetMouseButtonDown(0) && canTalkClick)
                {
                    canTalkClick = false;
                    if (event2count == 1) EditText("��,��,�A,��,��,",5);
                    else if (event2count == 2) EditText("��,��,��,��,��,��,��,��,��,��,�S,...,��,��,��,��,��,",5);
                    else if (event2count == 3) EditText("��,��,����,��,��,�l,�q,��,��,��,�C,��,��,��,...,",5);
                    else if (event2count == 4) { talkText.text = ""; event2 = false; }
                    event2count++;
                }
            }
            if(29 < playerPos.x && playerPos.x < 35 && event3)
            {

            }
        }
        //�G�l�~�[�����񂾂Ƃ��Ɍo���l�𑝂₷�D
        //�o���l�̓C���X�y�N�^�[�Őݒ�ł���悤��EnemyController�Őݒ�
        /*
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
                    //titleText.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
                    //�^�C�g���̕\��
                }));
            }
        }*/
    }
    int i = 0;
    void EditText(string str,int span)
    {
        string[] strArray = str.Split(',');
        i = 0;
        Text talkText = talk.GetComponent<Text>();
        talkText.text = "";
        StartCoroutine(RepeatMethod(span, strArray.Length, () => {
            talkText.text = talkText.text + strArray[i];
            if (i < strArray.Length - 1) i++;
            else
            {
                canTalkClick = true;
                Array.Clear(strArray,0,strArray.Length);
            }
        }));
        
        //talkText.text = "";
    }
    IEnumerator RepeatMethod(int delayFrameCount, int RepeatCount, Action action)
    {
        action();
        int count;
        for (count = 0; RepeatCount > count; count++)
        {
            for (var i = 0; i < delayFrameCount; i++)
            {
                yield return null;
            }
            action();
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "player") 
        {
            
            playerPos = playercontroller.transform.position;
            Debug.Log(playerPos);
            iventflag = true;
        }
    }
}
