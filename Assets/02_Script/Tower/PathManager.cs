using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance { get; private set; }

    [SerializeField] private Transform[] nodes;
    public Transform[] Nodes => nodes;

    private void Awake()
    {
        if (PathManager.Instance == null)
            Instance = this;
    }

}
