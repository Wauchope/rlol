using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour
{
    public int id;
    public string name;

    Renderer renderer;

    private void Awake()
    {
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void SetColour(Color colour)
    {
        renderer.material.SetColor("_Color", colour);
    }

    public Vector2 GetTilePos()
    {
        return new Vector2(transform.position.x / LevelGenerator.Instance.GetTileWidth(), transform.position.z / LevelGenerator.Instance.GetTileWidth());
    }

    public int GetDistanceFromOrigin()
    {
        return Mathf.FloorToInt(Vector2.Distance(GetTilePos(), Vector2.zero));
    }
}
