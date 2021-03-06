﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public bool IsHarvestable;
    [SerializeField]
    private int maxFoodAmount= 4;
    [SerializeField]
    private int foodAmount;
    public int FoodAmount
    {
        get
        {
            return this.foodAmount;
        }
        set
        {
            if(this.foodAmount != value)
            {
                this.foodAmount = value;
            }
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "Prey")
    //    {
    //        if (other.gameObject.GetComponent<PreySprite>().CurState == PreyStates.FoundFood)
    //        {
    //            FoodTaken();
    //        }
    //    }
    //}

    public void FoodTaken()
    {
        this.FoodAmount--;
    }
    // Start is called before the first frame update
    void Start()
    {
        FoodAmount = maxFoodAmount;
        IsHarvestable = true;
    }

    public void FoodRestored()
    {
        this.FoodAmount = maxFoodAmount;
        this.IsHarvestable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(FoodAmount <= 0)
        {
            this.IsHarvestable = false;
        }
    }
}
