using System.Collections;
using System.Collections.Generic;
using GenericHelper;
using UnityEngine;


namespace ProjectileHandler
{
    public class ProjectilePool : Pool<Projectile>
    {
        private void Awake()
        {
            Instance = this;
        }
    }
}