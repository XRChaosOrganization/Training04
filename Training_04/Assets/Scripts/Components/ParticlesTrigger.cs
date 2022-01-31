using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesTrigger : MonoBehaviour
{
    ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void Play()
    {
        particle.Play();
    }
}
