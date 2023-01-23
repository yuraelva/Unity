using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    public PlayerController playercontroller;
    public EnemyAttackBullet enemyattackbullet;

    public GameObject enemyHPGauge;
    public GameObject DieParticle;

    public float damageReceived;
    Vector2 playerPos;
    float span_jump = 5f;float delta_jump = 0; //飛びつき攻撃のクールタイム
    float span_bullet = 5f;float delta_bullet = 0; //弾発射攻撃のクールタイム
    float span_shot = 0.3f; float delta_shot = 0; //弾と弾の間隔

    bool At_flag = false; //クールタイムのフラグ．trueは攻撃中のため，クールタイムは減らないようにする

    int Done_bullet_Num = 1, bullet_Num = 5; //発射済みの弾の数と発射する弾の数
    int Pos_direction; //プレイヤーのいる方向 右にいるなら1,左にいるなら-1

    //ステータス
    public float defaultHP = 2000,nowHP;
    public float attack = 10;
    // Start is called before the first frame update
    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        nowHP = defaultHP;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = playercontroller.transform.position;

        if (playerPos.x - transform.position.x < 0) Pos_direction = -1;
        else Pos_direction = 1;
        float dist = Vector3.Distance(transform.position, playerPos); //プレイヤーと敵の距離
        if (dist < 6 && !At_flag) { this.delta_jump += Time.deltaTime; Debug.Log("ジャンプクール"); } //近くにいる時，ジャンプ攻撃のクールタイムを減らす
        if (5 < dist && dist < 10 && !At_flag) { this.delta_bullet += Time.deltaTime; Debug.Log("弾クール"); } // 遠くにいるとき，弾攻撃のクールタイムを減らす．
        //Debug.Log("接敵");

        if (this.delta_jump > this.span_jump)
        {
            this.delta_jump = 0;
            At_flag = true;
            
            StartCoroutine(DelayMethod(25, () =>
            {
                Vector2 jumpAttack = new Vector2(Pos_direction * 4, 6);
                rigid2D.velocity = jumpAttack;
                StartCoroutine(DelayMethod(10, () =>
                {
                    At_flag = false;
                }));
            }));
            
        }

        if (this.delta_bullet > this.span_bullet) 
        {
            if (!At_flag)
            {
                bullet_Num = UnityEngine.Random.Range(1, 5);
                //bullet_Num = 6; //弾を何個だすか
                Debug.Log("ランダムで弾の数を決める ->" + bullet_Num);
            }
            At_flag = true;//攻撃のクールタイムが回復しない状態
            delta_shot += Time.deltaTime;
            if (delta_shot > span_shot)//弾のしゅつげん感覚
            {
                delta_shot = 0;
                double genex, geney;
                genex = 3 * Math.Cos((Math.PI) * Done_bullet_Num / (bullet_Num + 1));
                geney = 3 * Math.Sin((Math.PI) * Done_bullet_Num / (bullet_Num + 1));
                enemyattackbullet.BulletGenerate((float)genex,(float)geney);
                Done_bullet_Num++;
            }
            if(Done_bullet_Num > bullet_Num)
            {
                this.delta_bullet = 0;
                Done_bullet_Num = 1;
                Debug.Log("発射終わり");
                At_flag = false;
            }
        }
        //hpゲージを減少させる
        nowHP -= damageReceived;
        enemyHPGauge.GetComponent<Image>().fillAmount -= damageReceived / defaultHP;
        damageReceived = 0;
        Debug.Log(nowHP / defaultHP);
        enemyHPGauge.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y+1));
        if (nowHP <= 0)
        {
            GameObject particle = Instantiate(DieParticle);
            particle.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Attack")
        {
            Debug.Log("hit");
            damageReceived = playercontroller.attack;
        }
    }//ダメージを受けたとき，攻撃力分のダメージを受ける
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name == "player")
        {
            playercontroller.DecreaseHP(attack);
            playercontroller.GetComponent<Rigidbody2D>().AddForce(new Vector2(1* Pos_direction, 1)  * 400);
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
