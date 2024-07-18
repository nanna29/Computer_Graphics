using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    // ����� ������ ���(Transform)
    public Transform target;

    // ��� ��ũ�� ����
    public float scrollRange = 9.9f;

    // ��� �̵� �ӵ�
    public float moveSpeed = 3.0f;

    // ��� �̵� ����
    public Vector3 moveDirection = Vector3.down;

    void Update()
    {
        // �� �����Ӹ��� ����� �̵� ����� �̵� �ӵ��� ���� �̵���Ŵ
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ����� ��ũ�� ���� ���Ϸ� ��������
        if (transform.position.y <= -scrollRange)
        {
            // ����� ����� ��ġ���� ��ũ�� ������ŭ ���� �ű�
            transform.position = target.position + Vector3.up * scrollRange;
        }
    }
}
