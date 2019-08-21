using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{
    public int id;
    public string name;

    public Vector2 position;

    private void Start()
    {
        position = transform.position;
    }
}
