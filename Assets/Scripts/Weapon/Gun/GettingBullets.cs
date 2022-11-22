using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingBullets : MonoBehaviour
{

    public GunData revolver;
    
   public void AddBullets(int amount)
    {
        revolver.reserveAmmo += amount;

     
    }
   
}
