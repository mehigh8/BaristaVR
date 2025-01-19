using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public GameObject pouringPoint;
    public float basePouringSpeed;

    private bool isPressed = false;
    
    [SerializeField] private GameObject particleSystem;
    [SerializeField] private Material coffeeColor;
    private ParticleSystem currentParticleSystem;

    private void Start()
    {
        currentParticleSystem = Instantiate(particleSystem).GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule emission = currentParticleSystem.emission;
        emission.rateOverTime = 0f;
        currentParticleSystem.GetComponent<Renderer>().material = coffeeColor;
    }
    
    private void Update()
    {
        currentParticleSystem.transform.position = pouringPoint.transform.position;
        if (isPressed)
        {
            ParticleSystem.EmissionModule emission = currentParticleSystem.emission;
            emission.rateOverTime = 20f;
            if (Physics.Raycast(pouringPoint.transform.position, Vector3.down, out RaycastHit hit, float.MaxValue, PlayerData.GetInstance().cupLayer))
            {
                Cup cup = hit.collider.gameObject.GetComponent<Cup>();
                if (cup != null)
                    cup.IncreaseCoffee(basePouringSpeed * Time.deltaTime);
            }
        }
        else
        {
            ParticleSystem.EmissionModule emission = currentParticleSystem.emission;
            emission.rateOverTime = 0f;
        }
    }

    public void Press()
    {
        print("am apasat");
        isPressed = true;
    }

    public void Unpress()
    {
        print("nu am mai apasat");
        isPressed = false;
    }
}
