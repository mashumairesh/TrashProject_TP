using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Char_TestCode : MonoBehaviour
{
    //Var
    [SerializeField] private Sprite sprIdle;
    [SerializeField] private Sprite sprMeleeAttack;
    [SerializeField] private Sprite sprRangeAttack;

    //Func

    private void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
    }

    private void Initialize()
    {

    }

    private void InputManager()
    {

    }

}
