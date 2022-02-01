using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterScript
{
    public int counter = 0;
    public CharacterScript weapon;
    public CharacterScript arm;
    public CloudScript brainCloud;

    private int[] posMap = {0,1,2,3,4,5,4,3,2,1};
    private int updateCounter = 0;
    private bool flagAttack = false;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        flagAttack = this.position == PlayerScript.i.GetBoardwalkPosition();
        if(flagAttack) arm.UpdateSprites(this.position);
    }

    public override void SlowUpdate()
    {
        base.SlowUpdate();

        // Attack
        if(flagAttack) {
            arm.UpdateSprites(-1);
            weapon.UpdateSprites(this.position);
            StartCoroutine(GameController.i.OnPlayerHurt(this.position));
            flagAttack = false;
        } else {
            arm.UpdateSprites(-1);
            weapon.UpdateSprites(-1);

            // Move
            if(++updateCounter % 2 == 0) {
                UpdateSprites(posMap[counter]);
                counter = ++counter % posMap.Length;
            }
        }
    }

    public void Reset() {
        counter = Random.Range(0, sprites.Length);
        UpdateSprites(-1);
        weapon.UpdateSprites(-1);
        arm.UpdateSprites(-1);
    }
}
