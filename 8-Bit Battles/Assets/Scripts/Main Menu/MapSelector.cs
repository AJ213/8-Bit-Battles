using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelector : MonoBehaviour
{
    public GameObject Map1;
    public GameObject Map2;
    public GameObject Map3;

    int map = 1;

    public void ChangeMap(int change)
    {
        map = map + change;

        if (map < 1)
        {
            map = 3;
        }
        if (map > 3)
        {
            map = 1;
        }
        DisableMaps();
        ActivateMap(map);
    }

    void DisableMaps()
    {
        Map1.SetActive(false);
        Map2.SetActive(false);
        Map3.SetActive(false);
    }
    void ActivateMap(int map)
    {
        if (map == 1)
        {
            Map1.SetActive(true);
        }
        if (map == 2)
        {
            Map2.SetActive(true);
        }
        if (map == 3)
        {
            Map3.SetActive(true);
        }
    }
}
