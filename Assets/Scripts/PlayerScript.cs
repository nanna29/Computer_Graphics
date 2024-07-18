using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // 플레이어 이동 속도
    public float moveSpeed = 5.0f;

    // 벽 충돌 상태
    public bool isTop;
    public bool isBottom;
    public bool isLeft;
    public bool isRight;

    // 플레이어의 생명과 점수
    public int life;
    public int score;

    // 플레이어의 파워와 최대 파워
    public int power;
    public int MaxPower;

    // 총알 발사 관련 변수
    public float maxFireDelay;
    public float curFireDelay;
    public GameObject bulletObjectA;
    public GameObject bulletObjectB;
    public GameObject ultimate;

    // 게임 매니저 연결
    public GameManager manager;

    // 플레이어 피격 상태 확인
    public bool isHit;

    // 오디오 소스
    private AudioSource audioSource;

    // 폭발 효과
    public GameObject explosion;

    void Start()
    {
        // 시작 시 오디오 소스 설정
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // 플레이어 이동 제어
        moveControl();

        // 마우스 클릭 시 총알 발사
        if (Input.GetMouseButton(0) && curFireDelay > maxFireDelay)
        {
            fireControl();
            audioSource.Play();
        }

        // 총알 발사 딜레이 갱신
        Reload();
    }

    // 총알 발사 로직
    void fireControl()
    {
        if (power == 1)
        {
            // 한 발 총알 발사
            GameObject bullet = Instantiate(bulletObjectA, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
        else if (power == 2)
        {
            // 두 발 총알 발사
            GameObject bullet1 = Instantiate(bulletObjectA, transform.position + Vector3.right * 0.2f, transform.rotation);
            GameObject bullet2 = Instantiate(bulletObjectA, transform.position + Vector3.left * 0.2f, transform.rotation);
            Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
            rigid1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigid2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
        else if (power == 3)
        {
            // 세 발 총알 발사
            GameObject bullet1 = Instantiate(bulletObjectA, transform.position + Vector3.right * 0.25f, transform.rotation);
            GameObject bulletB = Instantiate(bulletObjectB, transform.position, transform.rotation);
            GameObject bullet2 = Instantiate(bulletObjectA, transform.position + Vector3.left * 0.25f, transform.rotation);
            Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidB = bulletB.GetComponent<Rigidbody2D>();
            Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
            rigid1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigidB.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            rigid2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }

        // 총알 발사 딜레이 초기화
        curFireDelay = 0;
    }

    // 플레이어 이동 제어
    void moveControl()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // 벽과 충돌 시 게임 화면을 벗어나지 못하도록 제어
        if ((isRight && x == 1) || (isLeft && x == -1))
        {
            x = 0;
            Debug.Log("충돌");
        }
        if ((y == 1 && isTop) || (y == -1 && isBottom))
        {
            y = 0;
            Debug.Log("충돌");
        }

        // 플레이어 이동 계산
        float moveX = x * Time.deltaTime * moveSpeed;
        float moveY = y * Time.deltaTime * moveSpeed;
        transform.Translate(moveX, moveY, 0);
    }

    // 총알 발사 딜레이 갱신
    void Reload()
    {
        curFireDelay += Time.deltaTime;
    }

    // 플레이어와 충돌한 객체 처리
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Outer")
        {
            // 외곽 충돌 처리
            switch (other.gameObject.name)
            {
                case "top":
                    isTop = true;
                    break;
                case "bottom":
                    isBottom = true;
                    break;
                case "left":
                    isLeft = true;
                    break;
                case "right":
                    isRight = true;
                    break;
            }
        }
        else if (other.gameObject.tag == "enemy" || other.gameObject.tag == "enemyBullet")
        {
            if (isHit)
            {
                return;
            }
            // 플레이어 피격 처리
            isHit = true;
            life--;
            manager.UpdateLifeIcon(life);
            Instantiate(explosion, transform.position, Quaternion.identity);
            if (life == 0)
            {
                // 생명이 0이면 게임 오버 처리
                manager.GameOver();
            }
            else
            {
                // 생명이 남아있으면 플레이어 재생성
                manager.RespawnPlayer();
            }

            // 플레이어 비활성화
            gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "item")
        {
            // 아이템 획득 처리
            ItemScript item = other.gameObject.GetComponent<ItemScript>();
            if (item.type == "power")
            {
                if (power == MaxPower)
                {
                    // 최대 파워일 때 추가 점수
                    score += 500;
                }
                else
                {
                    // 파워 증가
                    power++;
                }
            }
            else if (item.type == "ultimate")
            {
                // 궁극기 아이템 사용
                ultimate.SetActive(true);
                Invoke("OffUltimate", 2f);

                // 모든 적 제거
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
                foreach (GameObject enemy in enemies)
                {
                    EnemyScript enemyLogic = enemy.GetComponent<EnemyScript>();
                    enemyLogic.OnHit(1000);
                }

                // 모든 적 총알 제거
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("enemyBullet");
                foreach (GameObject bullet in bullets)
                {
                    Destroy(bullet);
                }
            }
            Destroy(other.gameObject);
        }
    }

    // 벽과 닿아있지 않으면 벽과 충돌 해제 처리
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Outer")
        {
            switch (other.gameObject.name)
            {
                case "top":
                    isTop = false;
                    break;
                case "bottom":
                    isBottom = false;
                    break;
                case "left":
                    isLeft = false;
                    break;
                case "right":
                    isRight = false;
                    break;
            }
        }
    }

    // 궁극기 상태 해제
    void OffUltimate()
    {
        ultimate.SetActive(false);
    }
}
