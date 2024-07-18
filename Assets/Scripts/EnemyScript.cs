using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int enemyScroe; // �� óġ �� ��� ����
    public string enemyName; // �� ĳ������ �̸�
    public float speed; // �� ĳ������ �̵� �ӵ�
    public int hp; // �� ĳ������ ü��

    // �Ѿ� �߻� ���� ����
    public float maxFireDelay; // �ִ� �߻� ������
    public float curFireDelay; // ���� �߻� ������
    public GameObject bulletObjectA; // �Ѿ� ������Ʈ A
    public GameObject bulletObjectB; // �Ѿ� ������Ʈ B

    // ������ ��� ���� ����
    public GameObject item; // �Ϲ� ������ ������Ʈ
    public GameObject itemUlti; // �ñر� ������ ������Ʈ

    public GameObject player; // �÷��̾� ������Ʈ
    public GameObject explosion; // ���� ȿ�� ������Ʈ

    // Update is called once per frame
    void Update()
    {
        // �Ѿ� �߻� ������ üũ �� �߻� �޼��� ȣ��
        if (curFireDelay > maxFireDelay)
        {
            fireControl();
        }

        Reload(); // �߻� ������ ����
    }

    void fireControl()
    {
        // �߻� ������ �ʱ�ȭ
        curFireDelay = 0;

        // �� ĳ���� �̸��� ���� �ٸ� �߻� ���� ����
        if (enemyName == "1")
        {
            // 1 enemy�� �Ѿ� 1�� �߻�
            GameObject bullet = Instantiate(bulletObjectA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 pyVec = player.transform.position - transform.position;
            rigid.AddForce(pyVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "3" || enemyName == "4")
        {
            // 3, 4�� �翷���� 2���� �߻�
            GameObject bullet1 = Instantiate(bulletObjectB, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bullet2 = Instantiate(bulletObjectB, transform.position + Vector3.left * 0.3f, transform.rotation);
            Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
            Vector3 pyVec1 = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 pyVec2 = player.transform.position - (transform.position + Vector3.left * 0.3f);
            rigid1.AddForce(pyVec1.normalized * 4, ForceMode2D.Impulse);
            rigid2.AddForce(pyVec2.normalized * 4, ForceMode2D.Impulse);
        }
    }

    void Reload()
    {
        // �߻� ������ ����
        curFireDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        // ü�� ����
        if (hp <= 0)
        {
            return; // ü���� 0 ���ϸ� �ƹ� �۾��� �������� ����
        }

        hp -= damage; // ü���� �Էµ� ��������ŭ ����

        if (hp <= 0)
        {
            // ���� ����� ���
            PlayerScript playerLogic = player.GetComponent<PlayerScript>();
            playerLogic.score += enemyScroe; // �÷��̾� ���� ����
            Instantiate(explosion, transform.position, Quaternion.identity); // ���� ȿ�� ����

            // ������ ��� Ȯ�� ����
            int ran = Random.Range(0, 100); // 0���� 99������ ��
            if (ran < 20)
            {
                // 20% Ȯ���� �Ϲ� ������ ���
                Instantiate(item, transform.position, item.transform.rotation);
            }
            else if (ran < 30)
            {
                // 10% Ȯ���� �ñر� ������ ���
                Instantiate(itemUlti, transform.position, item.transform.rotation);
            }
            // ������ Ȯ���δ� �ƹ��͵� ������� ����

            Destroy(gameObject); // �� ĳ���� ������Ʈ �ı�
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �Ѿ� ���� �浹�� ��� �� ĳ���� �ı�
        // ȭ�� ������ ���� ������ �ڿ� �Ҹ�
        if (other.gameObject.tag == "OuterBullet")
        {
            Destroy(gameObject);
        }
        // �÷��̾� �Ѿ˰� �浹�� ���
        else if (other.gameObject.tag == "PyBullet")
        {
            BulletScript bullet = other.gameObject.GetComponent<BulletScript>();
            OnHit(bullet.damage); // �ǰ� ó�� �޼��� ȣ��

            Destroy(other.gameObject); // �Ѿ� ������Ʈ �ı�

        }
    }
}
