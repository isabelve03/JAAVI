using FishNet.Component.Animating;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAnimate : NetworkBehaviour
{
    private Animator _animator;
    private NetworkAnimator _networkAnimator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _networkAnimator = GetComponent<NetworkAnimator>();
    }

    public void Run(bool run)
    {
        _animator.SetBool("run", run);
    }

    public void Block(bool block)
    {
        if (block)
        {
            _networkAnimator.SetTrigger("block");
        }
        else
        {
            _networkAnimator.SetTrigger("blockDone");
        }
    }

    public void Attack1()
    {
        _networkAnimator.SetTrigger("attack1");
    }
}
