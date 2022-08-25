using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private Light muzzleLight;
    [SerializeField]
    private float timer;
    private float MuzzleTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void MuzzleFlashEnable()
    {
        MuzzleTimer = 0;

        muzzleFlash.Play();
        muzzleLight.enabled = true;
        muzzleLight.enabled = false;


    }
    private void Update()
    {


        MuzzleTimer += Time.deltaTime;
        muzzleLight.enabled = MuzzleTimer >= timer ? false : true;
       

    }
}
