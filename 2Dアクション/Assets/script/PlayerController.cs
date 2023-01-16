using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    static class Cons //モード．定数
    {
        public const int stay = 0;
        public const int walkright = 1;
        public const int walkleft = 2;
        public const int jump = 3;
        public const int dash = 4;
        public const int jumpwalkright = 5;
        public const int jumpwalkleft = 6;
    }
    Rigidbody2D rigid2D;
    Animator animator;

    public AttackPlay attackplay;
    public WorpPlay worpplay;
    public WorpPlayReverse worpplayR;
    public GameObject HPGauge;
    public GameObject MagicAttackPre;
    public GameObject cooltime;
    public GameObject choice1;
    public GameObject choice3;
    public GameObject worpmark;
    public GameObject go_dashstar;
    int jumpCount = 1;

    public float jumpForce = 500.0f;
    public float walkForce = 60.0f;
    public float maxWalkSpeed = 2.0f;
    public int HPDecreaseCooltime = 60; //フレーム数]
    public int MPDecreaseCooltime = 30; //フレーム数
    public int mode = Cons.stay;
    public int beforemode = Cons.stay;
    public int UseSkill = 1;//1 or 2 or 3

    bool OnGround = true;
    bool canDecreaseHp = true;
    bool canDecreaseMp;
    bool motionflag;
    int magicCooltime = 1;
    bool canMagicAttack = true;

    public float nowHP;
    public float defaultHP = 100;
    public float attack = 100;
    public float defense = 100;
    public float MP = 100;
    public bool canplay = true;

    Vector2 follSpeed;
    Vector2 worppoint;
    Vector2 size = new Vector2(0.25f,0.25f);

    int key=1;


    void ini()
    {
        jumpForce = 600f;
        walkForce = 45.0f;
        maxWalkSpeed = 2.0f;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(size.x, size.y, 0);
        this.rigid2D = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 60;
        this.animator = GetComponent<Animator>();
        nowHP = defaultHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (canplay)
        {
            
            motionflag = attackplay.getsetcanstart;
            follSpeed = new Vector2(0, this.rigid2D.velocity.y);//落下速度を求めている     
            if (Input.GetKey(KeyCode.D) && mode != Cons.dash)
            {
                if (mode == Cons.jump)
                {
                    mode = Cons.jumpwalkright;
                }
                else mode = Cons.walkright;
            }//モードをwalkrightに
            if (Input.GetKey(KeyCode.A) && mode != Cons.dash)
            {
                if (mode == Cons.jump)
                {
                    mode = Cons.jumpwalkleft;
                }
                else mode = Cons.walkleft;
            }//モードをwalkleftに
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UseSkill = 1;
                choice1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                choice3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                UseSkill = 2;
                choice1.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                choice3.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) { UseSkill = 3; }
            if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) && mode != Cons.dash)
            {
                this.rigid2D.velocity = follSpeed;
                if (OnGround) mode = Cons.stay;
                else mode = Cons.jump;
            }//キーを離したときに停止する
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount == 1)
            {
                jumpCount--;
                this.rigid2D.AddForce(transform.up * this.jumpForce);
                mode = Cons.jump;
            }//モードをjumpに
            if (Input.GetKeyDown(KeyCode.LeftShift) && worpplay.getsetcanworp == true)
            {
                mode = Cons.dash;
                rigid2D.gravityScale = 0;
                transform.localScale = new Vector3(0, 0, 0);
                GetComponent<Collider2D>().isTrigger = true;
                if (OnGround) worpplay.PlayMovie(transform.position.x, transform.position.y);
                rigid2D.velocity = Vector2.zero;
                Time.timeScale = 0.01f;
            }//モードをdashに
            if (Input.GetMouseButtonDown(0) && mode != Cons.dash && attackplay.getsetcanstart)
            {//セットしてある攻撃によって攻撃のスクリプトを変更したい
                Debug.Log("攻撃");
                switch (UseSkill)
                {
                    case 1:

                        attack = 100;
                        animator.SetTrigger("attack");
                        //mode = Cons.attack1;//一段階目の攻撃(剣なら)
                        attackplay.PlayMovie(transform.position.x, transform.position.y, key);

                        break;
                    case 2:

                        attack = 40;
                        if (canMagicAttack)
                        {
                            canMagicAttack = false;
                            GameObject go = Instantiate(MagicAttackPre);
                            go.transform.position = new Vector2(transform.position.x + key * 2, transform.position.y);
                            go.GetComponent<Rigidbody2D>().velocity = key * new Vector2(5, 0);
                            StartCoroutine(DelayMethod(magicCooltime, () =>
                            {
                                canMagicAttack = true;
                                cooltime.GetComponent<Image>().fillAmount = 1;
                                cooltime.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 0f);
                            }));
                        }
                        break;
                    case 3:
                        break;
                }
            }
            if (!canMagicAttack)
            {
                cooltime.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 1f);
                cooltime.GetComponent<Image>().fillAmount -= 0.03f; // 1 / magicCooltime;
            }
            Debug.Log("now mode: " + mode);
            switch (mode)
            { //それぞれのモードで何をするか
                case Cons.stay:
                    animator.SetTrigger("stay");
                    ini();
                    break;

                case Cons.walkleft:
                    key = -1;
                    walkSys();
                    break;

                case Cons.walkright:
                    key = 1;
                    walkSys();
                    break;

                case Cons.jump:
                    OnGround = false;
                    break;

                case Cons.dash: //　dash
                                //地面でのダッシュの際に，ワープ開始のエフェクトが追いつかず，終了のタイミングで出る


                    int R = 1;
                    Vector2 dashVec = new Vector2((float)(R * key), (float)0);

                    if (!OnGround)
                    {


                        worppoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        worppoint.x = worppoint.x - transform.position.x;
                        worppoint.y = worppoint.y - transform.position.y;
                        R = 3;
                        double theta = Math.Atan2(worppoint.y, worppoint.x);
                        double worpVectX = R * Math.Cos(theta);
                        double worpVectY = R * Math.Sin(theta);
                        dashVec = new Vector2((float)worpVectX, (float)worpVectY);
                        go_dashstar.transform.position = new Vector2(dashVec.x + transform.position.x,dashVec.y + transform.position.y);
                        if (Input.GetMouseButtonUp(0))
                        {
                            Time.timeScale = 1f;
                            
                            this.rigid2D.AddForce(dashVec * 200);
                            StartCoroutine(DelayMethod(10, () =>
                            {
                                worpplayR.PlayMovie(transform.position.x, transform.position.y);
                                this.rigid2D.velocity = Vector2.zero;
                                rigid2D.gravityScale = 4;
                                transform.localScale = new Vector3(key * size.x, size.y, 0);
                                GetComponent<Collider2D>().isTrigger = false;
                                if (!OnGround) mode = Cons.jump;
                                else mode = Cons.stay;
                                go_dashstar.transform.position = new Vector2(-2000, -2000);
                            }));
                        }
                        else break;
                    }
                    else
                    {
                        Time.timeScale = 1f;
                        this.rigid2D.AddForce(dashVec * 200);
                        StartCoroutine(DelayMethod(10, () =>
                        {
                            worpplayR.PlayMovie(transform.position.x, transform.position.y);
                            this.rigid2D.velocity = Vector2.zero;
                            rigid2D.gravityScale = 4;
                            transform.localScale = new Vector3(key * size.x, size.y, 0);
                            GetComponent<Collider2D>().isTrigger = false;
                            if (!OnGround) mode = Cons.jump;
                            else mode = Cons.stay;
                            go_dashstar.transform.position = new Vector2(-2000, -2000);
                        }));
                    }
                    
                    break;
                case Cons.jumpwalkleft:
                    //this.key = Input.GetAxisRaw("Horizontal"); ;
                    key = -1;
                    walkSys();
                    break;
                case Cons.jumpwalkright:
                    //this.key = Input.GetAxisRaw("Horizontal"); ;
                    key = 1;
                    walkSys();
                    break;
            }
        }
    }
    public void DecreaseHP(float decHp)
    {
        if (canDecreaseHp)
        {
            nowHP -= decHp;
            HPGauge.GetComponent<Image>().fillAmount -= decHp / defaultHP;
        }
        StartCoroutine(DelayMethod(HPDecreaseCooltime, () =>
        {
            canDecreaseHp = true;
        }));
        canDecreaseHp = false;
    }
    public void DecreaseMP(float decMp)
    {
        canDecreaseMp = false;
        if (canDecreaseMp) MP -= decMp;
        StartCoroutine(DelayMethod(MPDecreaseCooltime, () =>
        {
            canDecreaseMp = true;
        }));
    }
    IEnumerator DelayMethod(int delayFrameCount,Action action)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            yield return null;
        }
        action();
    }
    IEnumerator DelayMethod2(int delayFrameCount, Action action)
    {
        for (var i = 0; i < delayFrameCount; i++)
        {
            rigid2D.velocity = Vector2.zero;
            yield return null;
        }
        action();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "floor")
        {
            mode = Cons.stay;
            OnGround = true;
            jumpCount = 1;
        }
    }
    void walkSys()
    {
        transform.localScale = new Vector3(key * size.x, size.y, 1);
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);
        if (OnGround)
        {
            walkForce = 45.0f;
            maxWalkSpeed = 2.0f;
            animator.SetTrigger("walk");
        }
        else
        {
            walkForce = 30f;
            maxWalkSpeed = 1f;
            mode = Cons.jump;
        }
        if (speedx < this.maxWalkSpeed && motionflag) this.rigid2D.AddForce(transform.right * key * this.walkForce);
    }
}
/*背景素材七三ゆきのアトリエ様
https://nanamiyuki.com/
*/