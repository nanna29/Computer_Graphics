using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int enemyScroe; // 적 처치 시 얻는 점수
    public string enemyName; // 적 캐릭터의 이름
    public float speed; // 적 캐릭터의 이동 속도
    public int hp; // 적 캐릭터의 체력

    // 총알 발사 관련 변수
    public float maxFireDelay; // 최대 발사 딜레이
    public float curFireDelay; // 현재 발사 딜레이
    public GameObject bulletObjectA; // 총알 오브젝트 A
    public GameObject bulletObjectB; // 총알 오브젝트 B

    // 아이템 드랍 관련 변수
    public GameObject item; // 일반 아이템 오브젝트
    public GameObject itemUlti; // 궁극기 아이템 오브젝트

    public GameObject player; // 플레이어 오브젝트
    public GameObject explosion; // 폭발 효과 오브젝트

    // Update is called once per frame
    void Update()
    {
        // 총알 발사 딜레이 체크 및 발사 메서드 호출
        if (curFireDelay > maxFireDelay)
        {
            fireControl();
        }

        Reload(); // 발사 딜레이 갱신
    }

    void fireControl()
    {
        // 발사 딜레이 초기화
        curFireDelay = 0;

        // 적 캐릭터 이름에 따라 다른 발사 패턴 설정
        if (enemyName == "1")
        {
            // 1 enemy는 총알 1개 발사
            GameObject bullet = Instantiate(bulletObjectA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 pyVec = player.transform.position - transform.position;
            rigid.AddForce(pyVec.normalized * 3, ForceMode2D.Impulse);
        }
        else if (enemyName == "3" || enemyName == "4")
        {
            // 3, 4는 양옆으로 2개씩 발사
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
        // 발사 딜레이 증가
        curFireDelay += Time.deltaTime;
    }

    public void OnHit(int damage)
    {
        // 체력 감소
        if (hp <= 0)
        {
            return; // 체력이 0 이하면 아무 작업도 수행하지 않음
        }

        hp -= damage; // 체력을 입력된 데미지만큼 감소

        if (hp <= 0)
        {
            // 적이 사망한 경우
            PlayerScript playerLogic = player.GetComponent<PlayerScript>();
            playerLogic.score += enemyScroe; // 플레이어 점수 증가
            Instantiate(explosion, transform.position, Quaternion.identity); // 폭발 효과 생성

            // 아이템 드랍 확률 설정
            int ran = Random.Range(0, 100); // 0에서 99까지의 값
            if (ran < 20)
            {
                // 20% 확률로 일반 아이템 드랍
                Instantiate(item, transform.position, item.transform.rotation);
            }
            else if (ran < 30)
            {
                // 10% 확률로 궁극기 아이템 드랍
                Instantiate(itemUlti, transform.position, item.transform.rotation);
            }
            // 나머지 확률로는 아무것도 드랍하지 않음

            Destroy(gameObject); // 적 캐릭터 오브젝트 파괴
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 총알 벽과 충돌한 경우 적 캐릭터 파괴
        // 화면 밖으로 나간 적들은 자연 소멸
        if (other.gameObject.tag == "OuterBullet")
        {
            Destroy(gameObject);
        }
        // 플레이어 총알과 충돌한 경우
        else if (other.gameObject.tag == "PyBullet")
        {
            BulletScript bullet = other.gameObject.GetComponent<BulletScript>();
            OnHit(bullet.damage); // 피격 처리 메서드 호출

            Destroy(other.gameObject); // 총알 오브젝트 파괴

        }
    }
}
