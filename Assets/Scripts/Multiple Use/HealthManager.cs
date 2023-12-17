using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Transform _entityTransform;

    [SerializeField] Transform _ownTransform;

    [SerializeField] Slider healthSlider;

    public int _healthPoints;

    private Vector3 defaultScale;
    private Vector3 reversedScale;

    private void Start()
    {
        defaultScale = _ownTransform.localScale;
        reversedScale = new Vector3(-_ownTransform.localScale.x, _ownTransform.localScale.y, _ownTransform.localScale.z);
    }

    private void Update()
    {
        healthSlider.value = _healthPoints * 0.01f;
        KeepOrientation();
    }

    private void KeepOrientation()
    {
        if (Mathf.Sign(_entityTransform.localScale.x) > 0)
        {
            _ownTransform.localScale = defaultScale;
        }
        else
        {
            _ownTransform.localScale = reversedScale;
        }
    }

}
