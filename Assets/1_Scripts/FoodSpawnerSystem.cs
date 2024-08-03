using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnerSystem : MonoBehaviour
{
    [SerializeField] private List<CharacterGrowthItem> allFoods;
    [SerializeField] private CharacterManager player;

    //private List<CharacterGrowthItem> foodsNearPlayer = new();
    private CharacterGrowthItem activeFood = null;

    private void Awake()
    {
        DisableAllFood();
        UpdateActiveFood();
    }

    private void Update()
    {
        UpdateActiveFood();
    }

    private void UpdateActiveFood()
    {
        if (HasActiveFood()) return;

        DisableAllFood();
        //foodsNearPlayer.Clear();

        //foreach (CharacterGrowthItem food in allFoods)
        //{
        //    if (Vector3.Distance(food.transform.position, player.transform.position) < 100)
        //    {
        //        foodsNearPlayer.Add(food);
        //    }
        //}

        CharacterGrowthItem toBecomeActive = GetRandomFromList(allFoods);
        toBecomeActive.gameObject.SetActive(true);

        activeFood = toBecomeActive;
    }

    private CharacterGrowthItem GetRandomFromList(List<CharacterGrowthItem> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }


    private void DisableAllFood()
    {
        foreach(CharacterGrowthItem food in allFoods)
        {
            food.gameObject.SetActive(false);
        }
    }

    private bool HasActiveFood()
    {
        foreach(CharacterGrowthItem food in allFoods)
        {
            if (food.gameObject.activeInHierarchy) return true;
        }

        return false;
    }
}
