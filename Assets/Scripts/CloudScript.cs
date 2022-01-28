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
    public CharacterScript[] pills = new CharacterScript[2];

    public int clapTarget;

    public override void SlowUpdate() {
        GameController.i.audio.Stop();

        // Update clouds
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
                GameController.i.player.enabled = true;
                GameController.i.enemy.enabled = true;
                // Speed up time a little
                Time.timeScale += (GameController.i.timeStep * 2f) / Time.timeScale;
                break;

            case 3:
                GameController.i.audio.PlayOneShot(GameController.i.warning);
                break;

            // Store player position
            case 2:
                GameController.i.audio.PlayOneShot(GameController.i.warning);
                clapTarget = GameController.i.player.GetBoardwalkPosition();
                break;

            // BOOM!
            case 1:
                lightning.UpdateSprites(Mathf.Clamp(clapTarget, 0, lightning.sprites.Length - 1));

                // Check player death
                if(lightning.position == GameController.i.player.GetBoardwalkPosition()) {
                    StartCoroutine(GameController.i.OnPlayerHurt(lightning.position));
                }
                // Check enemy death
                else if(lightning.position == GameController.i.enemy.position) {
                    StartCoroutine(GameController.i.OnEnemyHurt());
                } else {
                    Debug.Log("Empty shot");
                    GameController.i.player.enabled = false;
                    GameController.i.enemy.enabled = false;
                }
                break;
        }

        // Tick
        value = Mathf.Clamp01( value - step );
    }
}
