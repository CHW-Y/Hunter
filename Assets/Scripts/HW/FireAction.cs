using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAction : MonoBehaviour
{
    PlayerMove pm;
    BossFSM bfsm;
    ParticleSystem ps;
    Transform firePos;
    Transform player;
    float curTime;
    float destroyTime = 2.5f;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        firePos = GameObject.Find("FirePosition").transform;
        player = GameObject.Find("Player").transform;
        pm = GameObject.Find("Player").GetComponent<PlayerMove>();
        bfsm = GameObject.Find("Boss").GetComponent<BossFSM>();

        curTime = 0;
    }

    void Update()
    {
        if (ps.collision.planeCount == 0)
        {
            ps.collision.AddPlane(player);
        }

        transform.position = firePos.position;
        curTime += Time.deltaTime;

        if (curTime >= destroyTime)
        {
            Destroy(gameObject);
        }


    }

    private void OnParticleCollision(GameObject other)
    {
        pm.AddHP(-bfsm.attackDamage);
        pm.SetAbsoluteStateTime(0.2f);
    }
}
