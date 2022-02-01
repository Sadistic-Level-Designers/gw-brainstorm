using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : SlowBehaviour
{
    public SpriteRenderer[] sprites;
    public int position = 0;

    public virtual void Awake() {
        // Setup sprites
        this.sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        foreach(SpriteRenderer s in sprites) {
            s.enabled = false;
        }

        if(position >= 0) sprites[position].enabled = true;

        yield return null;
    }

    public void Move(int steps = 0) {

    }

    public void Hide() {

    }

    public void UpdateSprites(int newPos, bool force = false) {
        if(newPos != position || force) {
            if(position >= 0) sprites[position].enabled = false;
            position = newPos;
            if(position >= 0) sprites[position].enabled = true;
        }
    } 

    public override void SlowUpdate() {}
}
