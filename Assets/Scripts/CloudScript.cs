using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : CharacterScript
{
    [Range(0f, 1f)]
    public float value = 1f;
    public float step = 0.05f;

    [Header("Actors")]

    public CharacterScript lightning;
    public EnemyScript enemy;
    public PlayerScript player;
    public CharacterScript hair;
    public CharacterScript[] pills = new CharacterScript[2];

    [Header("Sounds")]
    private new AudioSource audio;
    public AudioClip warning;
    public AudioClip playerDeath;
    public AudioClip enemyDeath;

    public int clapTarget;

    new void OnEnable() {
        audio = GetComponent<AudioSource>();
    }

    public override void SlowUpdate() {
        audio.Stop();

        // Update clouds
        value = Mathf.Clamp01( value - step );

        base.sprites[0].enabled = value > (0.2f * 1);
        base.sprites[1].enabled = value > (0.2f * 4);
        base.sprites[2].enabled = value > (0.2f * 3);
        base.sprites[3].enabled = value > (0.2f * 0);
        base.sprites[4].enabled = value > (0.2f * 2);

        // Update pills
        if(pills[0].position + pills[1].position == -2) {
            CharacterScript pill = pills[ Mathf.RoundToInt( Random.Range(0, 1f) ) ];
            pill.UpdateSprites( Mathf.RoundToInt(value) * -1 );
        }
        
        // Update lightning
        int clap = Mathf.RoundToInt(value / step) % 4; // one lightning every 4 steps
        Debug.Log("Clap " + clap);

        switch(clap) {
            // Reset lightning
            case 0:
                lightning.UpdateSprites(-1);
                hair.UpdateSprites(-1);
                // Reenable animation/control
                player.enabled = true;
                enemy.enabled = true;
                break;

            case 3:
                audio.PlayOneShot(warning);
                break;

            // Store player position
            case 2:
                audio.PlayOneShot(warning);
                clapTarget = player.GetBoardwalkPosition();
                break;

            // BOOM!
            case 1:
                lightning.UpdateSprites(Mathf.Clamp(clapTarget, 0, lightning.sprites.Length - 1));

                // Disable animation/control
                player.enabled = false;
                enemy.enabled = false;

                // Check player death
                bool flagResetEnemy = false;
                if(lightning.position == player.GetBoardwalkPosition()) {
                    audio.PlayOneShot(playerDeath);
                    Debug.Log("Player shot");
                    hair.UpdateSprites(lightning.position);

                    flagResetEnemy = true;
                }
                // Check enemy death
                else if(lightning.position == enemy.position) {
                    audio.PlayOneShot(enemyDeath);
                    Debug.Log("Enemy shot");
                    // GameController.i.score++;

                    flagResetEnemy = true;
                }

                if(flagResetEnemy) {
                    enemy.Reset();
                    enemy.UpdateSprites(-1);
                }
                break;
        }
    }
}
