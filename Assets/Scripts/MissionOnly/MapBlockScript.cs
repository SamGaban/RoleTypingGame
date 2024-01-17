using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>
/// New map system instance script
/// </summary>
public class MapBlockScript : MonoBehaviour
{
    [TabGroup("references", "References")][SerializeField] private TeleporterScript _teleportScript;

    [TabGroup("references", "References")][SerializeField] private Omen _omen;

    [TabGroup("references", "References")] [SerializeField]
    private List<CoinScript> coinList;


    public bool isDone = false;


    /// Teleport method.
    /// This method is used to trigger the teleportation process.
    /// It calls the GoTo method from the _teleportScript object.
    /// /
    [ButtonGroup]
    public void Teleport()
    {
        _teleportScript.GoTo();
    }

    private void Start()
    {
        ChoseCoins();
    }

    /// <summary>
    /// Updates the state of the object.
    /// </summary>
    private void Update()
    {
        if (isDone) return;

        if (_omen != null)
        {
            if (_omen.isDestroying)
            {
                isDone = true;
            }
        }
    }

    public void ClearAllCoins()
    {
        foreach (CoinScript coin in coinList)
        {
            if (coin._toKeep) coin.Throw();
        }
        coinList.RemoveAll(item => item == null);
    }

    private void ChoseCoins()
    {
        int maxCoins = Mathf.Min(4, coinList.Count);
        int numCoins = Random.Range(1, maxCoins + 1);

        if (coinList == null || coinList.Count <= 1) return;

        List<int> coinsToKeep = GenerateNonRepeatingNumbers(numCoins, 0, coinList.Count - 1);
        
        foreach (int index in coinsToKeep)
        {
            coinList[index]._toKeep = true;
        }

        foreach (CoinScript coin in coinList)
        {
            if (!coin._toKeep)
            {
                coin.Throw();
            }
        }

        coinList.RemoveAll(item => item == null);
    }
    
    public static List<int> GenerateNonRepeatingNumbers(int n, int min, int max)
    {
        List<int> numbers = new List<int>();
        for (int i = min; i <= max; i++)
            numbers.Add(i);

        List<int> result = new List<int>();
        while (result.Count < n)
        {
            int index = Random.Range(0, numbers.Count);
            result.Add(numbers[index]);
            numbers.RemoveAt(index);
        }

        return result;
    }
    
}
