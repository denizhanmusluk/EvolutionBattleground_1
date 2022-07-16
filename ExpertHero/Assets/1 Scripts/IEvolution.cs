using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvolution
{
    void duplicate(int cloneCount);
    void playerDead(int damage);
    void levelGate(int amountYear);
    void magneticGate(int affectTime);
    void speedGate(int affectTime, int speedUp);
    void attackSpeedGate(int affectTime, float speedUp);
    void damageGate(int affectTime, int damageUp);
}
