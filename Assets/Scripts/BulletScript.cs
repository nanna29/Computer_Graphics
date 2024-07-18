using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage; // �Ѿ��� ������

    public bool isRotate; // ȸ�� ����

    void Update()
    {
        if (isRotate)
        {
            // ȸ���� Ȱ��ȭ�Ǿ� ���� �� �Ѿ��� ȸ����Ŵ
            transform.Rotate(Vector3.forward * 10);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "OuterBullet")
        {
            // �Ѿ� �浹 ���� �浹�ϸ� �Ѿ� �ı�
            // �ý��� ����ȭ ���� ����
            Destroy(gameObject);
        }
    }
}
