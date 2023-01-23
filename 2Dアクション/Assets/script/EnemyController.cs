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
    float span_jump = 5f;float delta_jump = 0; //��т��U���̃N�[���^�C��
    float span_bullet = 5f;float delta_bullet = 0; //�e���ˍU���̃N�[���^�C��
    float span_shot = 0.3f; float delta_shot = 0; //�e�ƒe�̊Ԋu

    bool At_flag = false; //�N�[���^�C���̃t���O�Dtrue�͍U�����̂��߁C�N�[���^�C���͌���Ȃ��悤�ɂ���

    int Done_bullet_Num = 1, bullet_Num = 5; //���ˍς݂̒e�̐��Ɣ��˂���e�̐�
    int Pos_direction; //�v���C���[�̂������ �E�ɂ���Ȃ�1,���ɂ���Ȃ�-1

    //�X�e�[�^�X
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
        float dist = Vector3.Distance(transform.position, playerPos); //�v���C���[�ƓG�̋���
        if (dist < 6 && !At_flag) { this.delta_jump += Time.deltaTime; Debug.Log("�W�����v�N�[��"); } //�߂��ɂ��鎞�C�W�����v�U���̃N�[���^�C�������炷
        if (5 < dist && dist < 10 && !At_flag) { this.delta_bullet += Time.deltaTime; Debug.Log("�e�N�[��"); } // �����ɂ���Ƃ��C�e�U���̃N�[���^�C�������炷�D
        //Debug.Log("�ړG");

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
                //bullet_Num = 6; //�e����������
                Debug.Log("�����_���Œe�̐������߂� ->" + bullet_Num);
            }
            At_flag = true;//�U���̃N�[���^�C�����񕜂��Ȃ����
            delta_shot += Time.deltaTime;
            if (delta_shot > span_shot)//�e�̂�����񊴊o
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
                Debug.Log("���ˏI���");
                At_flag = false;
            }
        }
        //hp�Q�[�W������������
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
    }//�_���[�W���󂯂��Ƃ��C�U���͕��̃_���[�W���󂯂�
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
