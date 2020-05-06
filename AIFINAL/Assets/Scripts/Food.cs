using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public enum HarvestState { HarvestFood, HarvestRemains, HarvestSeeds}

    public HarvestState harvestState;
    [SerializeField]
    private int foodAmount = 2;
    public int FoodAmount { get; set; }
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
        this.foodAmount--;
    }
    // Start is called before the first frame update
    void Start()
    {
        harvestState = HarvestState.HarvestFood;
    }

    // Update is called once per frame
    void Update()
    {
        switch (harvestState)
        {
            case HarvestState.HarvestFood:
            if(this.foodAmount <= 0)
            {
                harvestState = HarvestState.HarvestRemains;
                    this.foodAmount = 1;
            }
                break;
            case HarvestState.HarvestRemains:
                if (this.foodAmount <= 0)
                {
                    harvestState = HarvestState.HarvestSeeds;
                    this.foodAmount = 1;
                }
                break;
            case HarvestState.HarvestSeeds:
                break;
        }
    }
}
