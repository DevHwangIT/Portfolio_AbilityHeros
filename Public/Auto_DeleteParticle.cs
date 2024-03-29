﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_DeleteParticle : MonoBehaviour {

    private ParticleSystem ps;

    private void Start()
    {
        ps = this.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                Destroy(this.gameObject);
            }
        }
    }

}
