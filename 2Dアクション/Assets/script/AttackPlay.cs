using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class AttackPlay : MonoBehaviour
{
    public VideoPlayer attackplay;
    public PlayerController playercontroller;
    public GameObject cooltime;
    float PX, PY,dec;
    int key;
    public bool canstart = true;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        attackplay.loopPointReached += LoopPointReached;
        this.animator = GetComponent<Animator>();
        dec = 0.016f; //HP�Q�[�W�̌����ʁC�����I�ȃN�[���^�C��

    }

    // Update is called once per frame
    void Update()
    {
        if (!canstart)//�t���O��true�łȂ������ꍇ�C�N�[���^�C��������������D�t���O�͊�{true�ŁC�X�^�[�g�����Ƃ���false�ɂȂ�
        {
            cooltime.GetComponent<Image>().color = new Color(0.9f,0.9f,0.9f,1f);//���i�͓��������Ă���D
            DecreaseCoolTime();
        }
        if (cooltime.GetComponent<Image>().fillAmount <= 0) {
            cooltime.GetComponent<Image>().fillAmount = 1;//�N�[���^�C������
            cooltime.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 0f);//�����ɖ߂�
            canstart = true;//�X�^�[�g�t���O��true�ɖ߂�
        }
    }
    public bool getsetcanstart
    {
        get{ return canstart;}
        set{ canstart = value; }
        
    }
    public void PlayMovie(float PlayerX,float PlayerY,int tmp)
    {
        if (canstart)
        {
             canstart = false;//���[�V�����Đ����͍U���ł��Ȃ�����
            //attack.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            PX = PlayerX; PY = PlayerY; key = tmp;
            
            attackplay.Play();
            
            StartCoroutine(DelayMethod(10));//�ŏ��̐��t���[���͉��n���o�Ă��܂��̂ŁC�����x�点�Ă���Đ�����D
        }
    }


    IEnumerator DelayMethod(int delayFrameCount)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        animator.SetTrigger("Collid");//�����蔻��̃A�j���[�V�������Đ�
        transform.localScale = new Vector2(4 * key, 4);
        transform.position = new Vector2(PX + key, PY);
    }
    public void LoopPointReached(VideoPlayer vp)
    {
        transform.position = new Vector2(-20, 0);
        attackplay.Stop();
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "enemy")
        {
            
        }
    }
    void DecreaseCoolTime()
    {
        cooltime.GetComponent<Image>().fillAmount -= dec;
    }
}
