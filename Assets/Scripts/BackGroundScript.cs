using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScript : MonoBehaviour
{
    // 배경이 움직일 대상(Transform)
    public Transform target;

    // 배경 스크롤 범위
    public float scrollRange = 9.9f;

    // 배경 이동 속도
    public float moveSpeed = 3.0f;

    // 배경 이동 방향
    public Vector3 moveDirection = Vector3.down;

    void Update()
    {
        // 매 프레임마다 배경을 이동 방향과 이동 속도에 따라 이동시킴
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 배경이 스크롤 범위 이하로 내려가면
        if (transform.position.y <= -scrollRange)
        {
            // 배경을 대상의 위치에서 스크롤 범위만큼 위로 옮김
            transform.position = target.position + Vector3.up * scrollRange;
        }
    }
}
