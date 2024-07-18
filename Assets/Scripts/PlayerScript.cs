using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // �÷��̾� �̵� �ӵ�
    public float moveSpeed = 5.0f;

    // �� �浹 ����
    public bool isTop;
    public bool isBottom;
    public bool isLeft;
    public bool isRight;

    // �÷��̾��� ����� ����
    public int life;
    public int score;

    // �÷��̾��� �Ŀ��� �ִ� �Ŀ�
    public int power;
    public int MaxPower;

    // �Ѿ� �߻� ���� ����
    public float maxFireDelay;
    public float curFireDelay;
    public GameObject bulletObjectA;
    public GameObject bulletObjectB;
    public GameObject ultimate;

    // ���� �Ŵ��� ����
    public GameManager manager;

    // �÷��̾� �ǰ� ���� Ȯ��
    public bool isHit;

    // ����� �ҽ�
    private AudioSource audioSource;

    // ���� ȿ��
    public GameObject explosion;

    void Start()
    {
        // ���� �� ����� �ҽ� ����
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // �÷��̾� �̵� ����
        moveControl();

        // ���콺 Ŭ�� �� �Ѿ� �߻�
        if (Input.GetMouseButton(0) && curFireDelay > maxFireDelay)
        {
            fireControl();
            audioSource.Play();
        }

        // �Ѿ� �߻� ������ ����
        Reload();
    }

    // �Ѿ� �߻� ����
    void fireControl()
    {
        if (power == 1)
        {
            // �� �� �Ѿ� �߻�
            GameObject bullet = Instantiate(bulletObjectA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
        else if (power == 2)
        {
            // �� �� �Ѿ� �߻�
            GameObject bullet1 = Instantiate(bulletObjectA, transform.position + Vector3.right * 0.2f, transform.rotation);
            GameObject bullet2 = Instantiate(bulletObjectA, transform.position + Vector3.left * 0.2f, transform.rotation);
            Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
            rigid1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigid2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
        else if (power == 3)
        {
            // �� �� �Ѿ� �߻�
            GameObject bullet1 = Instantiate(bulletObjectA, transform.position + Vector3.right * 0.25f, transform.rotation);
            GameObject bulletB = Instantiate(bulletObjectB, transform.position, transform.rotation);
            GameObject bullet2 = Instantiate(bulletObjectA, transform.position + Vector3.left * 0.25f, transform.rotation);
            Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidB = bulletB.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
            rigid1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigidB.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigid2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }

        // �Ѿ� �߻� ������ �ʱ�ȭ
        curFireDelay = 0;
    }

    // �÷��̾� �̵� ����
    void moveControl()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // ���� �浹 �� ���� ȭ���� ����� ���ϵ��� ����
        if ((isRight && x == 1) || (isLeft && x == -1))
        {
            x = 0;
            Debug.Log("�浹");
        }
        if ((y == 1 && isTop) || (y == -1 && isBottom))
        {
            y = 0;
            Debug.Log("�浹");
        }

        // �÷��̾� �̵� ���
        float moveX = x * Time.deltaTime * moveSpeed;
        float moveY = y * Time.deltaTime * moveSpeed;
        transform.Translate(moveX, moveY, 0);
    }

    // �Ѿ� �߻� ������ ����
    void Reload()
    {
        curFireDelay += Time.deltaTime;
    }

    // �÷��̾�� �浹�� ��ü ó��
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Outer")
        {
            // �ܰ� �浹 ó��
            switch (other.gameObject.name)
            {
                case "top":
                    isTop = true;
                    break;
                case "bottom":
                    isBottom = true;
                    break;
                case "left":
                    isLeft = true;
                    break;
                case "right":
                    isRight = true;
                    break;
            }
        }
        else if (other.gameObject.tag == "enemy" || other.gameObject.tag == "enemyBullet")
        {
            if (isHit)
            {
                return;
            }
            // �÷��̾� �ǰ� ó��
            isHit = true;
            life--;
            manager.UpdateLifeIcon(life);
            Instantiate(explosion, transform.position, Quaternion.identity);
            if (life == 0)
            {
                // ������ 0�̸� ���� ���� ó��
                manager.GameOver();
            }
            else
            {
                // ������ ���������� �÷��̾� �����
                manager.RespawnPlayer();
            }

            // �÷��̾� ��Ȱ��ȭ
            gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "item")
        {
            // ������ ȹ�� ó��
            ItemScript item = other.gameObject.GetComponent<ItemScript>();
            if (item.type == "power")
            {
                if (power == MaxPower)
                {
                    // �ִ� �Ŀ��� �� �߰� ����
                    score += 500;
                }
                else
                {
                    // �Ŀ� ����
                    power++;
                }
            }
            else if (item.type == "ultimate")
            {
                // �ñر� ������ ���
                ultimate.SetActive(true);
                Invoke("OffUltimate", 2f);

                // ��� �� ����
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
                foreach (GameObject enemy in enemies)
                {
                    EnemyScript enemyLogic = enemy.GetComponent<EnemyScript>();
                    enemyLogic.OnHit(1000);
                }

                // ��� �� �Ѿ� ����
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("enemyBullet");
                foreach (GameObject bullet in bullets)
                {
                    Destroy(bullet);
                }
            }
            Destroy(other.gameObject);
        }
    }

    // ���� ������� ������ ���� �浹 ���� ó��
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Outer")
        {
            switch (other.gameObject.name)
            {
                case "top":
                    isTop = false;
                    break;
                case "bottom":
                    isBottom = false;
                    break;
                case "left":
                    isLeft = false;
                    break;
                case "right":
                    isRight = false;
                    break;
            }
        }
    }

    // �ñر� ���� ����
    void OffUltimate()
    {
        ultimate.SetActive(false);
    }
}
