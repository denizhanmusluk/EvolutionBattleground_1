using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    void follow(GameObject targetZombie, GameObject weapon);
    int damage { get; set; }
    bool damageUpActive { get; set; }

}
