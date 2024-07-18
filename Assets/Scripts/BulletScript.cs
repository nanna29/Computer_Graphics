using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage; // 총알의 데미지

    public bool isRotate; // 회전 여부

    void Update()
    {
        if (isRotate)
        {
            // 회전이 활성화되어 있을 때 총알을 회전시킴
            transform.Rotate(Vector3.forward * 10);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "OuterBullet")
        {
            // 총알 충돌 벽과 충돌하면 총알 파괴
            // 시스템 과부화 막기 위함
            Destroy(gameObject);
        }
    }
}
