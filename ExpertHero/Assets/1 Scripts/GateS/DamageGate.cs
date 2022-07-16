using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageGate : MonoBehaviour
{
    [SerializeField] int affectTime;
    [SerializeField] int damageUp;
    GateBreak gateBreak;
    private void Start()
    {
        gateBreak = GetComponent<GateBreak>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerEvolution>() != null)
        {
            other.GetComponent<PlayerEvolution>().damageGate(affectTime, damageUp);
            GetComponent<Collider>().enabled = false;
            gateBreak.glassHit(other.gameObject);
            GateSpawner.Instance.gateAll.Remove(gameObject);
        }
    }
}
