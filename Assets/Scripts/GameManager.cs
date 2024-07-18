using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public GameObject[] enemyObjects; // �� ������Ʈ���� ���� �迭   
    public Transform[] spawnLocations; // ���� ��ġ���� ���� �迭
    public int maxSpawnLocationIndex; // �ִ� ���� ��ġ �ε���   
    public int enemyRange; // ���� ���� ��  
    public float maxSpawnDelay; // �ִ� ���� ������   
    public float curSpawnDelay; // ���� ���� ������  
    public int stageScore;  // �������� ����   
    public GameObject player; // �÷��̾� ������Ʈ   
    public TextMeshProUGUI scoreText; // ������ ǥ���� UI �ؽ�Ʈ   
    public SpriteRenderer[] lifeImage; // ���� �����ܵ��� ���� �迭    
    public GameObject gameOverSet; // ���� ���� UI ������Ʈ
    public GameObject gameClearSet; // ���� Ŭ���� UI ������Ʈ

    // Update is called once per frame
    void Update()
    {
        // ���� ���� ������ ����
        curSpawnDelay += Time.deltaTime;

        // �ִ� ���� �����̺��� ���� ���� �����̰� ũ��
        if (curSpawnDelay > maxSpawnDelay)
        {
            // �� ���� �޼��� ȣ��
            SpawnEnemy();
            // ���� ������ �ʱ�ȭ
            curSpawnDelay = 0;
        }

        // �÷��̾� ��ũ��Ʈ ������Ʈ ��������
        PlayerScript playerLogic = player.GetComponent<PlayerScript>();

        // ���� �ؽ�Ʈ ������Ʈ
        scoreText.text = string.Format("{0:n0}", playerLogic.score);

        // �������� ���� �̻����� ������ �ö󰡸�
        if (playerLogic.score >= stageScore)
        {
            // ���� Ŭ���� �޼��� ȣ��
            GameClear();
        }
    }

    // �� ���� �޼���
    void SpawnEnemy()
    {
        // ���� �� �̸� ��������
        string sceneName = SceneManager.GetActiveScene().name;
        GameObject enemy = null;

        // ���� ���� �ٸ� enemy ����
        // �� 3�ϰ�� enemy4 ���� (���� ����)
        if (sceneName == "GameScene3")
        {
            // enemyRange ���� ������ �������� �� ����
            int randonEnemy = Random.Range(0, enemyRange);

            // ���� ��ġ �ε��� ���� ������ �������� ��ġ ����
            int ranLocation = Random.Range(0, maxSpawnLocationIndex);

            // ���õ� ���� ���� ��ġ�� ����
            enemy = Instantiate(enemyObjects[randonEnemy],
                                spawnLocations[ranLocation].position,
                                spawnLocations[ranLocation].rotation);
        }
        else
        {
            // enemyRange ���� ������ �������� �� ����
            int randonEnemy = Random.Range(0, enemyRange);

            // ���� ��ġ �ε��� ���� ������ �������� ��ġ ����
            int ranLocation = Random.Range(0, maxSpawnLocationIndex);

            // ���õ� ���� ���� ��ġ�� ����
            enemy = Instantiate(enemyObjects[randonEnemy],
                                spawnLocations[ranLocation].position,
                                spawnLocations[ranLocation].rotation);
        }

        // ���� Rigidbody2D ������Ʈ ��������
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        // EnemyScript ��������
        EnemyScript ememyLogic = enemy.GetComponent<EnemyScript>();

        // ���� �� �÷��̾� ������Ʈ�� ���� player ������ �Ѱ��ֱ�
        ememyLogic.player = player;

        // enemy �ӵ� ����
        rigid.velocity = new Vector2(0, ememyLogic.speed * (-1));
    }

    // �÷��̾� ������ �޼���
    public void RespawnPlayer()
    {
        // RespawnPlayerExe �޼��带 1.5�� �Ŀ� ȣ��
        Invoke("RespawnPlayerExe", 1.5f);
    }

    // �÷��̾� ������ ���� �޼���
    void RespawnPlayerExe()
    {
        // �÷��̾� ��ġ�� ������ ��ġ�� �̵�
        player.transform.position = Vector3.down * 4.4f;

        // �÷��̾� ������Ʈ Ȱ��ȭ
        player.SetActive(true);

        // �÷��̾� ��ũ��Ʈ ������Ʈ ��������
        PlayerScript playerLogic = player.GetComponent<PlayerScript>();

        // �÷��̾ �ǰݵ��� ���� ���·� ����
        playerLogic.isHit = false;
    }

    // ���� ������ ������Ʈ �޼���
    public void UpdateLifeIcon(int life)
    {
        // ���� �� �̸� ��������
        string sceneName = SceneManager.GetActiveScene().name;

        // ���� ���� �ٸ� life ����
        // �� 3�� �������� ��ŭ ���� 5��
        if (sceneName == "GameScene3")
        {
            // ��3������ 5���� ���� �������� ���
            for (int i = 0; i < 5; i++)
            {
                lifeImage[i].enabled = false; // ���� ������ ��Ȱ��ȭ
            }
            for (int i = 0; i < life; i++)
            {
                lifeImage[i].enabled = true; // ���� ������ Ȱ��ȭ
            }
        }
        else
        {
            // �� ���� �������� 3���� ���� �������� ���
            for (int i = 0; i < 3; i++)
            {
                lifeImage[i].enabled = false; // ���� ������ ��Ȱ��ȭ
            }
            for (int i = 0; i < life; i++)
            {
                lifeImage[i].enabled = true; // ���� ������ Ȱ��ȭ
            }
        }
    }

    // ���� ���� �޼���
    public void GameOver()
    {
        // ���� ���� UI Ȱ��ȭ
        gameOverSet.SetActive(true);

        // �÷��̾� ������Ʈ ��Ȱ��ȭ
        player.SetActive(false);
    }

    // ���� Ŭ���� �޼���
    public void GameClear()
    {
        // ���� Ŭ���� UI Ȱ��ȭ
        gameClearSet.SetActive(true);

        // �÷��̾� ������Ʈ ��Ȱ��ȭ
        player.SetActive(false);
    }
}
