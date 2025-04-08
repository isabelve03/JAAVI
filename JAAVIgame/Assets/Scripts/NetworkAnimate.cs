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
            Debug.Log("starting block");
            _networkAnimator.SetTrigger("block");
        }
        else
        {
            Debug.Log("done block");
            _networkAnimator.SetTrigger("blockDone");
        }
    }
}
