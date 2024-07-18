using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string type; // 아이템의 종류

    Rigidbody2D rigid; // Rigidbody2D 컴포넌트 저장

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트를 가져와 변수에 할당
        rigid.velocity = Vector2.down * 3; // 아이템을 아래 방향으로 초기 속도를 설정하여 떨어지게 함
    }

}
