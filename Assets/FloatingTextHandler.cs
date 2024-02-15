using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bardent
{
    public class FloatingTextHandler : MonoBehaviour
    {
        void Start()
        {
            Destroy(gameObject, 100f);
            transform.localPosition += new Vector3(0, 0.5f, 0);
        }
    }
}
