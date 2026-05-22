using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

public class ButtonObject : MonoBehaviour
{

    [Tooltip("버튼을 누를 시 실행할 이벤트의 모음입니다.")]
    public UnityEvent Event;

    private void Awake()
    {
        
    }


    private void Init()
    {

    }
    

}
