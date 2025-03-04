using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    float ShootingForce { get; set; }
    abstract void Shoot();
}
