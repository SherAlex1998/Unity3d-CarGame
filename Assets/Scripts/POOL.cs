using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POOL : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake () 
    {
            PoolManager.init (transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
