using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Animator _anim;

    private void Start() {
        _anim = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            PickupFish();
        }
    }
    private void PickupFish() {
        _anim?.SetTrigger("Pickup");
        GameStats.Instance.CollectFish();
        // fish count
        //score
        //play sfx
        //trigger animation
    }

    public void OnShowChunk() {
        _anim?.SetTrigger("Idle");
    }
}
