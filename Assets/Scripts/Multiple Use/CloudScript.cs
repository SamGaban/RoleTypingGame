using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
 private float leftLimit = -485;

    private float leftSpawnZone = -465;
    
    private float rightLimit = 195f;

    private float upLimit = -134f;

    private float downLimit = -150f;

    private int refNumber;

    private Transform _transform;

    private SpriteRenderer _spriteRenderer;

    private float _moveSpeed;

    private void Start()
    {
        int refNumber = Random.Range(1, 5); // 2 - 3 or 4
        
        
        _transform = this.transform;
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        SizeSwitcher();
        _transform.position = new Vector2(Random.Range(leftLimit, leftSpawnZone), Random.Range(downLimit, upLimit));
    }

    private void SizeSwitcher()
    {
        _spriteRenderer.sortingLayerName = "FarBehind";
        
        switch (refNumber)
        {
            case 0:
                _transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                _spriteRenderer.sortingOrder = 5;
                _moveSpeed = 5f;
                break;
            case 1:
                _transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                _spriteRenderer.sortingOrder = 4;
                _moveSpeed = 4f;
                break;
            case 2:
                _transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                _spriteRenderer.sortingOrder = 3;
                _moveSpeed = 3f;
                break;
            case 3:
                _transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                _spriteRenderer.sortingOrder = 2;
                _moveSpeed = 2f;
                break;
            case 4:
                _transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                _spriteRenderer.sortingOrder = 1;
                _moveSpeed = 1f;
                break;
        }
    }

    private void Update()
    {
        if (_transform.position.x >= rightLimit) Destroy(this.gameObject);
        
        _transform.Translate(new Vector3(_moveSpeed * Time.deltaTime, 0f, 0f));
    }
}
