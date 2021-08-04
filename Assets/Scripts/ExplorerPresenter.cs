using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explorer
{
    public class ExplorerPresenter : MonoBehaviour
    {
        [SerializeField]
        Transform target;
        [SerializeField]
        float forwardFactor = 10;
        [SerializeField]
        float rotateFactor = 100;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                target.position += target.forward * Time.deltaTime * forwardFactor;
            }
            if (Input.GetKey(KeyCode.S))
            {
                target.position -= target.forward * Time.deltaTime * forwardFactor;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                target.Rotate(-Vector3.up, Time.deltaTime * rotateFactor);
            }
            if (Input.GetKey(KeyCode.E))
            {
                target.Rotate(Vector3.up, Time.deltaTime * rotateFactor);
            }
        }
    }
}
