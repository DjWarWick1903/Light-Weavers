using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColliderLimit : MonoBehaviour
{
    private TilemapRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<TilemapRenderer>();
    }

    private void Start()
    {
        renderer.enabled = false;
    }
}
