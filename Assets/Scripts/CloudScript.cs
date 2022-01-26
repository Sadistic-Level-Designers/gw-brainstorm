using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : CharacterScript
{
    [Range(0f, 1f)]
    public float value = 1f;
    public float step = 0.05f;

    public CharacterScript lightning;
    public EnemyScript enemy;
    public PlayerScript player;
    public CharacterScript hair;

    public int clapTarget;

    public override void SlowUpdate() {
        // Update clouds
        value -= step;

        base.sprites[0].enabled = value > (0.2f * 1);
        base.sprites[1].enabled = value > (0.2f * 4);
        base.sprites[2].enabled = value > (0.2f * 3);
        base.sprites[3].enabled = value > (0.2f * 0);
        base.sprites[4].enabled = value > (0.2f * 2);

        // Update lightning
        int clap = Mathf.RoundToInt(Mathf.Clamp01(value) / step) % 4;
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
                break;

            // Store player position
            case 2:
                clapTarget = player.GetBoardwalkPosition();
                break;

            // BOOM!
            case 1:
                lightning.UpdateSprites(Mathf.Clamp(clapTarget, 0, lightning.sprites.Length - 1));

                // Disable animation/control
                player.enabled = false;
                enemy.enabled = false;

                // Check player death
                if(lightning.position == player.GetBoardwalkPosition()) {
                    hair.UpdateSprites(lightning.position);

                    enemy.Reset();
                    enemy.UpdateSprites(-1);
                }
                // Check enemy death
                else if(lightning.position == enemy.position) {
                    Debug.Log("Enemy shot");
                    // GameController.i.score++;
                    
                }
                break;
        }
    }
}
