using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public GameObject[] enemyObjects; // 적 오브젝트들을 담을 배열   
    public Transform[] spawnLocations; // 스폰 위치들을 담을 배열
    public int maxSpawnLocationIndex; // 최대 스폰 위치 인덱스   
    public int enemyRange; // 적의 종류 수  
    public float maxSpawnDelay; // 최대 스폰 딜레이   
    public float curSpawnDelay; // 현재 스폰 딜레이  
    public int stageScore;  // 스테이지 점수   
    public GameObject player; // 플레이어 오브젝트   
    public TextMeshProUGUI scoreText; // 점수를 표시할 UI 텍스트   
    public SpriteRenderer[] lifeImage; // 생명 아이콘들을 담을 배열    
    public GameObject gameOverSet; // 게임 오버 UI 오브젝트
    public GameObject gameClearSet; // 게임 클리어 UI 오브젝트

    // Update is called once per frame
    void Update()
    {
        // 현재 스폰 딜레이 갱신
        curSpawnDelay += Time.deltaTime;

        // 최대 스폰 딜레이보다 현재 스폰 딜레이가 크면
        if (curSpawnDelay > maxSpawnDelay)
        {
            // 적 생성 메서드 호출
            SpawnEnemy();
            // 스폰 딜레이 초기화
            curSpawnDelay = 0;
        }

        // 플레이어 스크립트 컴포넌트 가져오기
        PlayerScript playerLogic = player.GetComponent<PlayerScript>();

        // 점수 텍스트 업데이트
        scoreText.text = string.Format("{0:n0}", playerLogic.score);

        // 스테이지 점수 이상으로 점수가 올라가면
        if (playerLogic.score >= stageScore)
        {
            // 게임 클리어 메서드 호출
            GameClear();
        }
    }

    // 적 생성 메서드
    void SpawnEnemy()
    {
        // 현재 씬 이름 가져오기
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject enemy = null;

        // 씬에 따라 다른 enemy 생성
        // 씬 3일경우 enemy4 등장 (보스 개념)
        if (sceneName == "GameScene3")
        {
            // enemyRange 범위 내에서 랜덤으로 적 선택
            int randonEnemy = Random.Range(0, enemyRange);

            // 스폰 위치 인덱스 범위 내에서 랜덤으로 위치 선택
            int ranLocation = Random.Range(0, maxSpawnLocationIndex);

            // 선택된 적을 스폰 위치에 생성
            enemy = Instantiate(enemyObjects[randonEnemy],
                                spawnLocations[ranLocation].position,
                                spawnLocations[ranLocation].rotation);
        }
        else
        {
            // enemyRange 범위 내에서 랜덤으로 적 선택
            int randonEnemy = Random.Range(0, enemyRange);

            // 스폰 위치 인덱스 범위 내에서 랜덤으로 위치 선택
            int ranLocation = Random.Range(0, maxSpawnLocationIndex);

            // 선택된 적을 스폰 위치에 생성
            enemy = Instantiate(enemyObjects[randonEnemy],
                                spawnLocations[ranLocation].position,
                                spawnLocations[ranLocation].rotation);
        }

        // 적의 Rigidbody2D 컴포넌트 가져오기
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        // EnemyScript 가져오기
        EnemyScript ememyLogic = enemy.GetComponent<EnemyScript>();

        // 생성 후 플레이어 오브젝트를 적의 player 변수에 넘겨주기
        ememyLogic.player = player;

        // enemy 속도 설정
        rigid.velocity = new Vector2(0, ememyLogic.speed * (-1));
    }

    // 플레이어 리스폰 메서드
    public void RespawnPlayer()
    {
        // RespawnPlayerExe 메서드를 1.5초 후에 호출
        Invoke("RespawnPlayerExe", 1.5f);
    }

    // 플레이어 리스폰 실행 메서드
    void RespawnPlayerExe()
    {
        // 플레이어 위치를 설정된 위치로 이동
        player.transform.position = Vector3.down * 4.4f;

        // 플레이어 오브젝트 활성화
        player.SetActive(true);

        // 플레이어 스크립트 컴포넌트 가져오기
        PlayerScript playerLogic = player.GetComponent<PlayerScript>();

        // 플레이어가 피격되지 않은 상태로 설정
        playerLogic.isHit = false;
    }

    // 생명 아이콘 업데이트 메서드
    public void UpdateLifeIcon(int life)
    {
        // 현재 씬 이름 가져오기
        string sceneName = SceneManager.GetActiveScene().name;

        // 씬에 따라 다른 life 설정
        // 씬 3은 보스전인 만큼 생명 5개
        if (sceneName == "GameScene3")
        {
            // 씬3에서는 5개의 생명 아이콘을 사용
            for (int i = 0; i < 5; i++)
            {
                lifeImage[i].enabled = false; // 생명 아이콘 비활성화
            }
            for (int i = 0; i < life; i++)
            {
                lifeImage[i].enabled = true; // 생명 아이콘 활성화
            }
        }
        else
        {
            // 그 외의 씬에서는 3개의 생명 아이콘을 사용
            for (int i = 0; i < 3; i++)
            {
                lifeImage[i].enabled = false; // 생명 아이콘 비활성화
            }
            for (int i = 0; i < life; i++)
            {
                lifeImage[i].enabled = true; // 생명 아이콘 활성화
            }
        }
    }

    // 게임 오버 메서드
    public void GameOver()
    {
        // 게임 오버 UI 활성화
        gameOverSet.SetActive(true);

        // 플레이어 오브젝트 비활성화
        player.SetActive(false);
    }

    // 게임 클리어 메서드
    public void GameClear()
    {
        // 게임 클리어 UI 활성화
        gameClearSet.SetActive(true);

        // 플레이어 오브젝트 비활성화
        player.SetActive(false);
    }
}
