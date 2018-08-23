using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashZone : MonoBehaviour
{
    public Transform upperBound;
    public Transform lowerBound;
    public float lowerOffset = 0.25f;
    public float width;
    public BoxCollider2D _collider;

    // Use this for initialization
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.size = new Vector2(width, 0);
        _collider.offset = new Vector2(upperBound.position.x, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _collider.size = new Vector2(_collider.size.x, upperBound.position.y - (lowerBound.position.y - lowerOffset));
        _collider.offset = new Vector2(_collider.offset.x, (upperBound.position.y + lowerBound.position.y - lowerOffset) * 0.5f);
    }
}
