using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFX : MonoBehaviour
{
    public Transform SpawnPosition;
    public GameObject ParticleFX;
    public Vector3 _pRotation;
    void Start()
    {
        InvokeRepeating("FireParticle", 2f, 3f);
    }

    void FireParticle(){
    	GameObject _particle = Instantiate(ParticleFX);
    	_particle.transform.SetParent(SpawnPosition);
    	_particle.transform.localPosition = new Vector3(0,0,0);
    	_particle.transform.localRotation = Quaternion.Euler(_pRotation);
    	Destroy(_particle, 6f);
	}
}
