using Services;
using UnityEngine;

public class BloodSpatter : MonoBehaviour
{
    private ParticleSystem ps;
    private MyObject myObject;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        myObject = GetComponent<MyObject>();
        myObject.OnActivate += () => ps.Play();
    }

    private void OnParticleSystemStopped()
    {
        myObject.Recycle();
    }
}
