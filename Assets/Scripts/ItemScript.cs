using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string type; // �������� ����

    Rigidbody2D rigid; // Rigidbody2D ������Ʈ ����

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ�� ������ ������ �Ҵ�
        rigid.velocity = Vector2.down * 3; // �������� �Ʒ� �������� �ʱ� �ӵ��� �����Ͽ� �������� ��
    }

}
