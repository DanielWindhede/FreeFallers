using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] public Vector2Int size;
    Color color;

    private void Start()
    {
        color = Random.ColorHSV();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, (Vector2)size);
    }
}
