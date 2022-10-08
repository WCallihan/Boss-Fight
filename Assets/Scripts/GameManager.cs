using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField] private Health playerHealth;
    [SerializeField] private Health bossHealth;

    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject winText;

    private void OnEnable() {
        playerHealth.Died += LoseState;
        bossHealth.Died += WinState;
    }

    private void OnDisable() {
        playerHealth.Died -= LoseState;
        bossHealth.Died -= WinState;
    }

    private void Awake()
    {
        loseText.SetActive(false);
        winText.SetActive(false);
    }

    private void Update() {
        //on backspace, reload the scene to reset
        if(Input.GetKeyDown(KeyCode.Backspace)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //on escape, quit the game
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    //when the player dies, trigger the lose state
    private void LoseState() {
        loseText.SetActive(true);
    }

    //when the boss dies, trigger the win state
    private void WinState() {
        winText.SetActive(true);
    }
}