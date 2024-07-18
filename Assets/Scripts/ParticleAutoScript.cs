using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoScript : MonoBehaviour
{
    private ParticleSystem particle; // 파티클 시스템 컴포넌트를 저장할 변수

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>(); // 자신의 파티클 시스템 컴포넌트를 가져와서 변수에 할당
    }

    // Update is called once per frame
    void Update()
    {
        // 파티클 시스템이 재생 중이 아니면
        if (particle.isPlaying == false)
        {
            Destroy(gameObject); // 파괴
        }
    }
}
