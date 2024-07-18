using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        // GameScene���� �� ��ȯ
        SceneManager.LoadScene("GameScene");
    }
    public void OnDesButtonClick()
    {
        // HowToPlayScene �� ��ȯ
        SceneManager.LoadScene("HowToPlayScene");
    }
    public void OnFirstButtonClick()
    {
        // ó��ȭ�� �� ��ȯ
        SceneManager.LoadScene("StartScene");
    }
    public void NextStage1()
    {
        // �������� 1�� �Ѿ��
        SceneManager.LoadScene("GameScene1");
    }
    public void NextStage2()
    {
        // �������� 2�� �Ѿ��
        SceneManager.LoadScene("GameScene2");
    }
    public void NextStage3()
    {
        // �������� 2�� �Ѿ��
        SceneManager.LoadScene("GameScene3");
    }
    public void OnOverScene()
    {
        // ���� ���� ȭ��
        SceneManager.LoadScene("OverScene");
    }
    public void GameRetry()
    {
        // �������� 0���� �Ѿ��
        SceneManager.LoadScene("GameScene");
    }
}
