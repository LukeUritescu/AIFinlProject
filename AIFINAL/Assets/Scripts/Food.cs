using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField]
    private int foodAmount = 2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Prey")
        {
            if (other.gameObject.GetComponent<PreySprite>().CurState == PreyStates.FoundFood)
            {
                FoodTaken();
            }
        }
    }

    private void FoodTaken()
    {
        this.foodAmount--;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(this.foodAmount <= 0)
        {
            //Object.Destroy(this.gameObject);
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
