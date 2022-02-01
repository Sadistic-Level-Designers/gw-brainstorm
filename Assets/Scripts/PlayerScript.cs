using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterScript
{
    public static PlayerScript i;

    public CharacterScript bridgeLeft;
    public CharacterScript pillLeft;
    public CharacterScript bridgeRight;
    public CharacterScript pillRight;
    public CloudScript brainCloud;
    
    public CharacterScript hair;

    public override void Awake() {
        base.Awake();

        // Set singleton instance
        i = this;
    }

    public int walkPressed = 0;
    public int stepsOnTrapdoor = 0;
    public int trapdoorFallThreshold = 30;
    public override void SlowUpdate()
    {
        // Left-right movement
        int x = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        int newPos = (int) Mathf.Clamp(position + x * this.walkPressed, 0, sprites.Length - 1);
        this.walkPressed = (int) Mathf.Cos(Mathf.Abs(x) * Mathf.PI/2);

        UpdateSprites(newPos);

        // Grab pill
        if((position == 0 && pillLeft.position == 0) || (position == sprites.Length - 1 && pillRight.position == 0)) {
            brainCloud.value = 1f;
            pillLeft.UpdateSprites(-1);
            pillRight.UpdateSprites(-1);
            GameController.i.audio.PlayOneShot(GameController.i.pillGet);
        }

        // Update trapdoor counter
        if(position == 0 || position == 7) {
            ++stepsOnTrapdoor;
        } else {
            stepsOnTrapdoor = 0;
        }

        // Check trapdoor fall condition
        if(stepsOnTrapdoor >= trapdoorFallThreshold) {
            if(position == 0)
                bridgeLeft.UpdateSprites(2);
            else
                bridgeRight.UpdateSprites(2);

            this.UpdateSprites(-1);
            this.position = 4;

            StartCoroutine(GameController.i.OnPlayerHurt(-1));
        }
    }

    public readonly int[] x = {-1, 0, 1, 2, 3, 4, 5, 6};
    public int GetBoardwalkPosition() {
        return x[position];
    }
}


