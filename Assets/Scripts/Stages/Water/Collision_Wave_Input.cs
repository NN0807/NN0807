using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Es.WaveformProvider.Sample
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class Collision_Wave_Input : MonoBehaviour
    {

        [SerializeField]
        private Texture2D Wave_Form;

        [SerializeField, Range(0f, 1f)]
        private float Input_Scale_Fitter = 0.01f;

        [SerializeField, Range(0f, 1f)]
        private float Strength = 1f;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            WaveInput(collision);
        }

        public void OnCollisionStay(Collision collision)
        {
            WaveInput(collision);
        }

        private void WaveInput(Collision collision)
        {
            foreach (var p in collision.contacts)
            {
                var canvas = p.otherCollider.GetComponent<Wave_Conductor>();
                if (canvas != null)
                    canvas.Input(Wave_Form, p.point, rigidbody.velocity.magnitude * rigidbody.mass * Input_Scale_Fitter, Strength);
            }
        }
    }

}
