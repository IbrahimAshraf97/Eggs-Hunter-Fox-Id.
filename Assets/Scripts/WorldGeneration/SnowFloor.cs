using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFloor : MonoBehaviour
{

    [SerializeField] private Transform _player;
    [SerializeField] private Material _material;
    private float _offsetSpeed = 0.25f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.forward * _player.transform.position.z;
        _material.SetVector("Snowoffset", new Vector2(0, -transform.position.z * _offsetSpeed));
    }
}
 