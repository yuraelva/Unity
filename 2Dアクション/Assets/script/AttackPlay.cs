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
        dec = 0.016f; //HPゲージの減少量，実質的なクールタイム

    }

    // Update is called once per frame
    void Update()
    {
        if (!canstart)//フラグがtrueでなかった場合，クールタイムを減少させる．フラグは基本trueで，スタートしたときにfalseになる
        {
            cooltime.GetComponent<Image>().color = new Color(0.9f,0.9f,0.9f,1f);//普段は透明化している．
            DecreaseCoolTime();
        }
        if (cooltime.GetComponent<Image>().fillAmount <= 0) {
            cooltime.GetComponent<Image>().fillAmount = 1;//クールタイム復活
            cooltime.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 0f);//透明に戻す
            canstart = true;//スタートフラグをtrueに戻す
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
             canstart = false;//モーション再生中は攻撃できなくする
            //attack.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            PX = PlayerX; PY = PlayerY; key = tmp;
            
            attackplay.Play();
            
            StartCoroutine(DelayMethod(10));//最初の数フレームは下地が出てしまうので，少し遅らせてから再生する．
        }
    }


    IEnumerator DelayMethod(int delayFrameCount)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        animator.SetTrigger("Collid");//当たり判定のアニメーションを再生
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
