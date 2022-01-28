using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : SlowBehaviour
{
    public static GameController i;

    [Header("Objects")]
    public EnemyScript enemy;
    public PlayerScript player;
    public CloudScript brainCloud;


    [Header("Score")]
    public RectTransform scoreCounter;
    public int score = 0;
    public int misses = 0;
    public int maxMisses = 3;
    public float timeStep = 0.01f;

    [Header("Sounds")]
    public new AudioSource audio;
    public AudioClip warning;
    public AudioClip playerDeath;
    public AudioClip enemyDeath;

    public void Awake() {
        audio = GetComponent<AudioSource>();
        GameController.i = this;
    }

    public override void Update() {
        base.Update();
        scoreCounter.GetComponent<TMPro.TextMeshProUGUI>().text = "" + score;
    }

    public override void SlowUpdate() {
        Time.timeScale += timeStep;
    }

    public IEnumerator OnPlayerHurt(int pos) {
        Debug.Log("Player shot");
        player.hair.UpdateSprites(pos);
        audio.PlayOneShot(playerDeath);

        // Reset game after 3 misses
        if(++misses == maxMisses) {
            GameObject.Find("BrainCloud").GetComponent<CloudScript>().enabled = false;
            GameObject.Find("Player").GetComponent<PlayerScript>().enabled = false;
            Time.timeScale = 1f;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        // Increment miss counter
        else {
            yield return OnAttackEvent();
        }
    }

    public IEnumerator OnEnemyHurt() {
        Debug.Log("Enemy shot");
        audio.PlayOneShot(enemyDeath);
        GameController.i.score++;

        yield return OnAttackEvent();
    }

    protected IEnumerator OnAttackEvent() {
        player.enabled = false;
        enemy.enabled = false;
        brainCloud.enabled = false;

        yield return new WaitForSeconds(1.0f);

        // Reset sprites
        enemy.Reset();
        player.hair.UpdateSprites(-1);
        brainCloud.value = 1f;
        brainCloud.lightning.UpdateSprites(-1);
        
        // Reenable animation/control
        player.enabled = true;
        enemy.enabled = true;
        brainCloud.enabled = true;

        yield return null;
    }
}
