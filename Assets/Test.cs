using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public UnityAction testAction;

    private UnityAction test;
    private UnityAction testB;

    private void Start()
    {
        test = () => Debug.Log("Test");
        testB = () => Debug.Log("TestB");

        testAction += test;
        testAction += testB;

        testAction?.Invoke();

        testAction -= test;

        testAction?.Invoke();
    }

    public void A()
    {
        Debug.Log("A");
    }
}
