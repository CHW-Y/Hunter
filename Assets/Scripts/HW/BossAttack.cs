using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public PlayerMove pm;
    public BossFSM bfsm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            pm.AddHP(-bfsm.attackDamage);
            pm.SetAbsoluteStateTime(1.2f);
        }
    }
}
