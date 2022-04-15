using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int hp = 100;

    public bool IsDead()
    {
        return hp <= 0;
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
    }
}
