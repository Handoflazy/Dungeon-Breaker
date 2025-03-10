using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstVersion
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField]
        protected string itemName;
        [SerializeField]
        protected string description;
        public abstract void Attack();
    }
}