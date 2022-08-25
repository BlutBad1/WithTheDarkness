using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingAnim : MonoBehaviour
{
    [SerializeField]
    private  GameObject gun;
    [SerializeField]
    private  GameObject lamp;
    [SerializeField]
    private  float animSpeed=1;
    private  bool animCheck = true;

    public  void ReloadAnim(float difference)
    {
        gun.GetComponent<Animator>().SetBool("Reloading", animCheck);
        lamp.GetComponent<Animator>().SetBool("Reloading", animCheck);
        gun.GetComponent<Animator>().SetFloat("AnimSpeed", 1 / (difference * animSpeed));
        lamp.GetComponent<Animator>().SetFloat("AnimSpeed", 1 / (difference * animSpeed));
        animCheck = !animCheck;
    }
   

    public  void TimeconsububleOn()
    {
        gun.GetComponent<Animator>().SetBool("Timeconsububle", true);
    }
    public  void TimeconsububleOff()
    {
        gun.GetComponent<Animator>().SetBool("Timeconsububle", false);
    }
}
