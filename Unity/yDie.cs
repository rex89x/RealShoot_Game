using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class yDie: MonoBehaviour
{

    public GameObject Particles;

    public void Reacttohit()
    {
        
        StartCoroutine(Die());

    }
    private IEnumerator Die()
    {
        GameObject particles = Instantiate(Particles, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(Particles, 1);
        yield return new WaitForSeconds(1.5f);

    }
    //-------------------
    

}
