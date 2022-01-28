using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : SlowBehaviour
{
    public static GameController i;

    public RectTransform scoreCounter;

    public int score = 0;
    public int misses = 0;
    public int maxMisses = 3;
    public float timeStep = 0.01f;

    public void Awake() {
        GameController.i = this;
    }

    public override void Update() {
        base.Update();
        scoreCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "" + score;
    }

    public override void SlowUpdate() {
        Time.timeScale += timeStep;
    }

    public IEnumerator OnPlayerShot() {
        // Reset game after 3 misses
        if(++misses == maxMisses) {
            GameObject.Find("BrainCloud").GetComponent<CloudScript>().enabled = false;
            GameObject.Find("Player").GetComponent<PlayerScript>().enabled = false;
            Time.timeScale = 1f;
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Increment miss counter
        else {
            yield return null;
        }
    }
}
