using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void GameOver();
    public event GameOver GameOverEvent;
    public static GameManager instance;
    public Transform[] checkpoints;
    public Transform playerTransform;
    Camera mainCamera;
    public TMP_Text text;
    PlayerMovement playerMovement;
    public bool GameWon = false;
    TimerCountdown timer;

    private void Awake()
    {
        instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerMovement=playerTransform.gameObject.GetComponent<PlayerMovement>();
        playerMovement.GameWonEvent += OnGameWonEvent;
        timer = GetComponent<TimerCountdown>();
        timer.GameLostEvent +=  OnGameLostEvent;
    }
    private void OnDestroy()
    {
        playerMovement.GameWonEvent -= OnGameWonEvent;
        timer.GameLostEvent -= OnGameLostEvent;
    }
    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.y>0 && playerTransform.position.y<47)
        {
            mainCamera.transform.position= new Vector3(0, playerTransform.position.y,mainCamera.transform.position.z);
        }
        else if(playerTransform.position.y<0)
        {
            mainCamera.transform.position = new Vector3(0,0, mainCamera.transform.position.z);
        }
        else if(playerTransform.position.y>47)
        {
            mainCamera.transform.position = new Vector3(0,47, mainCamera.transform.position.z);
        }
    }

    private void OnGameWonEvent()
    {
        GameWon = true;
        GameFinished();
    }

    private void OnGameLostEvent()
    {
        GameWon = false;
        GameFinished();
    }

    private void GameFinished()
    {
        string endGameText;
        if (GameWon)
        {
            endGameText = "You Won!";
        }
        else
            endGameText = "You Lost!";

        text.text = endGameText;
        text.gameObject.SetActive(true);
        GameOverEvent?.Invoke();
        StartCoroutine(GoToMM());
    }
    IEnumerator GoToMM()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
