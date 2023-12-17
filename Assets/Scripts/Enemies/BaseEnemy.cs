using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _transform;

}
