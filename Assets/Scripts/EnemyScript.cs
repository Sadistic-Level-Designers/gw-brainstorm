using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterScript
{
    public int counter = 0;

    private int[] posMap = {0,1,2,3,4,5,4,3,2,1};
    public override void SlowUpdate()
    {
        base.SlowUpdate();

        UpdateSprites(posMap[counter]);
        counter = ++counter % posMap.Length;
    }

    public void Reset() {
        counter = Random.Range(0, sprites.Length);
        UpdateSprites(0);
    }
}
