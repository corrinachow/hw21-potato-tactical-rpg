using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Collidable
{

    protected override void Update() {
        base.Update();
        Action();
    }

    abstract protected void Action();
}
