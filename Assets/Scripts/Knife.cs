using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlipKnife
{
    public class Knife : MonoBehaviour
    {
        public Rigidbody rigid;

        public float force = 5f;
        public float torque = 20f;

        private Vector2 startSwipe;
        private Vector2 endSwipe;

        private float timeWhenWeStartedFlying;

        // Update is called once per frame
        void Update ()
        {
            if (!rigid.isKinematic)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                startSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                endSwipe = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                Swipe();
            }
        }

        void Swipe ()
        {
            rigid.isKinematic = false;

            timeWhenWeStartedFlying = Time.time;

            Vector2 swipe = endSwipe - startSwipe;

            rigid.AddForce(swipe * force, ForceMode.Impulse);

            rigid.AddTorque(0f, 0f, -torque, ForceMode.Impulse);
        }

        void OnTriggerEnter (Collider col)
        {
            if (col.tag == "WoodenBlock")
            {
                rigid.isKinematic = true;
            }
            else
            {
                Restart();
            }
        }

        void OnCollisionEnter ()
        {
            float timeInAir = Time.time - timeWhenWeStartedFlying;

            if (!rigid.isKinematic && timeInAir >= .05f)
            {
                Restart();
            }
        }

        void Restart ()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}