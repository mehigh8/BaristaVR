using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pour : Holdable
{
    public float inclinationThreshold;
    public float basePouringSpeed;
    public GameObject pouringPoint;
    public bool isCoffee;
    [SerializeField] private GameObject particleSystem;
    private ParticleSystem currentParticleSystem;

    [SerializeField] private Material milkColor;
    [SerializeField] private Material coffeeColor;

    private void Start()
    {
        currentParticleSystem = Instantiate(particleSystem).GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = currentParticleSystem.emission;
        emission.rateOverTime = 0f;
        currentParticleSystem.GetComponent<Renderer>().material = isCoffee ? coffeeColor : milkColor;
    }

    void Update()
    {
        currentParticleSystem.transform.position = pouringPoint.transform.position;
        if (isHeld)
        {
            float inclination = Vector3.Angle(Vector3.up, transform.up);
            if (inclination > inclinationThreshold)
            {
                ParticleSystem.EmissionModule emission = currentParticleSystem.emission;
                emission.rateOverTime = 20f;
                if (Physics.Raycast(pouringPoint.transform.position, Vector3.down, out RaycastHit hit, float.MaxValue, PlayerData.GetInstance().cupLayer))
                {
                    Cup cup = hit.collider.gameObject.GetComponent<Cup>();
                    if (cup != null)
                    {
                        if (isCoffee)
                            cup.IncreaseCoffee(inclination * basePouringSpeed * Time.deltaTime);
                        else
                            cup.IncreaseMilk(inclination * basePouringSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                ParticleSystem.EmissionModule emission = currentParticleSystem.emission;
                emission.rateOverTime = 0f;
            }
        }
    }

    public override void PickUp()
    {
        base.PickUp();
        PlayerData.GetInstance().pour = this;
    }

    public override void PutDown()
    {
        base.PutDown();
        PlayerData.GetInstance().pour = null;
    }
}
