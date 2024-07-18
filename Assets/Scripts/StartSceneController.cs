using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        // GameScene으로 씬 전환
        SceneManager.LoadScene("GameScene");
    }
    public void OnDesButtonClick()
    {
        // HowToPlayScene 씬 전환
        SceneManager.LoadScene("HowToPlayScene");
    }
    public void OnFirstButtonClick()
    {
        // 처음화면 씬 전환
        SceneManager.LoadScene("StartScene");
    }
    public void NextStage1()
    {
        // 스테이지 1로 넘어가기
        SceneManager.LoadScene("GameScene1");
    }
    public void NextStage2()
    {
        // 스테이지 2로 넘어가기
        SceneManager.LoadScene("GameScene2");
    }
    public void NextStage3()
    {
        // 스테이지 2로 넘어가기
        SceneManager.LoadScene("GameScene3");
    }
    public void OnOverScene()
    {
        // 게임 종료 화면
        SceneManager.LoadScene("OverScene");
    }
    public void GameRetry()
    {
        // 스테이지 0으로 넘어가기
        SceneManager.LoadScene("GameScene");
    }
}
