using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 25f;
    public float speed=250;

    public bool doesHaveKnockout { get; set ; }

    private void Awake()
    {
        doesHaveKnockout = false;
    }
    public Transform GetTransform()
    {
     return transform;
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GetComponent<AudioSource>().Play();
            transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z), speed).SetUpdate(UpdateType.Normal, true);

        }

    }

    public void TakeDamage(int damage, RaycastHit hit)
    {
     
    }
}
