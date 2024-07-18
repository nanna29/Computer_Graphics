using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoScript : MonoBehaviour
{
    private ParticleSystem particle; // ��ƼŬ �ý��� ������Ʈ�� ������ ����

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>(); // �ڽ��� ��ƼŬ �ý��� ������Ʈ�� �����ͼ� ������ �Ҵ�
    }

    // Update is called once per frame
    void Update()
    {
        // ��ƼŬ �ý����� ��� ���� �ƴϸ�
        if (particle.isPlaying == false)
        {
            Destroy(gameObject); // �ı�
        }
    }
}
