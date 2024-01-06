using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpellEffectsCanvas : MonoBehaviour
{
    [TabGroup("references", "References")]
    [SerializeField]
    private Transform parentPanel;

    [TabGroup("references", "References")]
    [SerializeField]
    private GameObject itemPrefab;

    [TabGroup("references", "Data")]
    [ShowInInspector]
    private Caster _caster;

    [TabGroup("references", "Data")]
    [ShowInInspector]
    private List<GameObject> _items;

    [TabGroup("references", "Data")]
    [ShowInInspector]
    private List<float> _times;



    private void Start()
    {
        _caster = FindObjectOfType<Caster>();
        _items = new List<GameObject>();
        _times = new List<float>();
    }

    [ButtonGroup]
    public void CreateTest()
    {
        CreateNewItem(15f, _caster.ReturnLogoList()[0]);
    }

    /// <summary>
    /// Creates a new item and adds it on screen
    /// </summary>
    /// <param name="time">Time duration of the spell</param>
    /// <param name="sprite">Logo of the spell</param>
    public void CreateNewItem(float time, Sprite sprite)
    {
        GameObject item = Instantiate(itemPrefab, parentPanel);
        item.transform.localPosition = new Vector3(-61, 319 - (100 * _items.Count), 0);

        SpellEffectsItem script = item.GetComponent<SpellEffectsItem>();
        script.Initialize(time, sprite);

        _items.Add(item);
        _times.Add(Time.time + time);
    }

    /// <summary>
    /// Method checking the time of each currently existing timer
    /// <para>If one reaches the end, the item is destroyed, and all the positions are shifted accordingly</para>
    /// </summary>
    private void TimeCheck()
    {
        int counter = 0;

        bool done = false;

        foreach (float time in _times)
        {
            if ((time - Time.time) <= 0)
            {
                done = true;
            }

            counter += done ? 0 : 1;
        }

        if (done)
        {
            Destroy(_items[counter]);
            _items.RemoveAt(counter);
            _times.RemoveAt(counter);

            if (_items.Count > counter)
            {
                for (int i = counter; i < _items.Count; i++)
                {
                    _items[i].transform.localPosition = new Vector3(_items[i].transform.localPosition.x, _items[i].transform.localPosition.y + 100, _items[i].transform.localPosition.z);
                }
            }
        }

    }

    private void Update()
    {
        TimeCheck();
    }
}
