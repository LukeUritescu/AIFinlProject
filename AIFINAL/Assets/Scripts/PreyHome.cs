using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreyHome : MonoBehaviour
{

    public GameObject PreyPrefab;
    private int foodStorage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Prey")
        {
            if(other.gameObject.GetComponent<PreySprite>().CurState == PreyStates.ReturnHome)
            {
                this.foodStorage++;
            }
        }
    }

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        this.foodStorage = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.foodStorage == 2)
        {
           GameObject newPrey = Instantiate(PreyPrefab, this.transform.position, this.transform.rotation);
            this.foodStorage = 0;
        }
    }
}
