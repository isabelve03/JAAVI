using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAnimator : NetworkBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Run(Boolean run)
    {

        _animator.SetBool("run", run);
    }

    // No jump animation implemented yet
    public void Jump(Boolean jump)
    {
//        _animator.SetBool("Jump", jump);
    }

    // not sure if this needs to be over network or not yet
    public void FlipSprite(Boolean flip)
    {

    }

    /* maybe implement if there is an airdash animation 
    public void AirDash()
    {
        
    }
    */
}

