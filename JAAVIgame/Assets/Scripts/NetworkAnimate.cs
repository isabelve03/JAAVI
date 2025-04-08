using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAnimate : NetworkBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Run(bool run)
    {
        //_animator.SetBool("run", run);
    }

    public void Block(bool block)
    {
        if (block)
        {
            Debug.Log("starting block");
            _animator.SetTrigger("block");
        }
        else
        {
            Debug.Log("done block");
            _animator.SetTrigger("blockDone");
        }
    }
}
