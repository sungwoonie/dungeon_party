using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_Particle_Controller : SingleTon<Reward_Particle_Controller>
{
    private ParticleSystem[] particles;

    #region "Unity"

    protected override void Awake()
    {
        base.Awake();

        Initialize_Component();
    }

    #endregion

    #region "Initialize"

    private void Initialize_Component()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    #endregion

    #region "Play"

    public void Play_Particle(int particle_number, double reward_amount)
    {
        if(reward_amount <= 0)
        {
            return;
        }

        particles[particle_number].Play();
    }

    #endregion
}