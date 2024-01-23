using System;
using System.Collections;
using System.Collections.Generic;
using RandomOrg.CoreApi;
using UnityEngine;

public class RandomGeneratorInstance : MonoBehaviour
{
    public static RandomGeneratorInstance Instance;
    private RandomOrgClient Client;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        Client = RandomOrgClient.GetRandomOrgClient("4a9c574d-9f5e-4549-99c3-0d719adc6e89");
    }

    public int GetRandomNumber(int min, int max)
    {
        try
        {
            int[] response = Client.GenerateIntegers(1, min, max);
            return response[0];
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return 1;
        };
    }

    public int[] GetMultipleRandomNumbers(int count, int min, int max)
    {
        try
        {
            int[] response = Client.GenerateIntegers(count, min, max);
            return response;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return new int[] { 1 };
        };
    }
}
