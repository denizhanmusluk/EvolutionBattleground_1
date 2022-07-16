using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AttackSpeedGate : MonoBehaviour
{
    [SerializeField] int affectTime;
    [SerializeField] float attackSpeedUp;
    GateBreak gateBreak;
    private void Start()
    {
        gateBreak = GetComponent<GateBreak>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerEvolution>() != null)
        {
            other.GetComponent<PlayerEvolution>().attackSpeedGate(affectTime, attackSpeedUp);
            GetComponent<Collider>().enabled = false;
            gateBreak.glassHit(other.gameObject);
            GateSpawner.Instance.gateAll.Remove(gameObject);
        }
    }
}
