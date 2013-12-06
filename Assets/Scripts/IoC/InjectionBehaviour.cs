using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class InjectionBehaviour : MonoBehaviour
{
    void Start()
    {
        var dependencyInjector = (DependencyInjector)FindObjectOfType(typeof(DependencyInjector));
        dependencyInjector.Inject(this);

        StartOverride();
    }

    protected abstract void StartOverride();
}