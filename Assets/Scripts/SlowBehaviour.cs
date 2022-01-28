using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlowBehaviour : MonoBehaviour
{
    /** Every how many frames the update should be executed */
    public int throttleFrames = 1;
    public bool isFixed = false;

    private int throttleCounter = 0;

    public void OnEnable() {
        this.throttleCounter = 0;
    }

    public virtual void Update() {
        if(!isFixed) DoUpdate();
    }

    public virtual void FixedUpdate() {
        if(isFixed) DoUpdate();
    }

    private void DoUpdate()
    {
        if(throttleFrames == 0 || (++throttleCounter % throttleFrames) == 0) {
            this.SlowUpdate();
        }
    }

    public abstract void SlowUpdate();
}
