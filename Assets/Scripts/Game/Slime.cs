using UnityEngine;

namespace Game
{
    public class Slime : MonoBehaviour
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private float speed = 5;
        [SerializeField] private SoundManager soundManager;
        private bool jumpKeyWasPressed;
        private float horizontalInput;
        private Rigidbody slimeRigidbody;
        private Vector3 eulerAnglesVelocity;
        private bool facingWest;
        private bool isPlaying = false;

        public void Play()
        {
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
        }

        void Start()
        {
            slimeRigidbody = GetComponent<Rigidbody>();
            eulerAnglesVelocity = new Vector3(0, 180, 0);
            facingWest = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPlaying)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpKeyWasPressed = true;
            }

            horizontalInput = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            if (!isPlaying)
            {
                return;
            }

            if (horizontalInput == 0)
            {
                soundManager.MoveStop();
            }
            else
            {
                if (Physics.OverlapSphere(transform.position, 0.1f, playerMask).Length != 0)
                {
                    soundManager.MoveSound();
                }
            }

            slimeRigidbody.velocity = new Vector3(horizontalInput * speed, slimeRigidbody.velocity.y, 0);
            if (horizontalInput < 0 && !facingWest)
            {
                Quaternion deltaRotation = Quaternion.Euler(eulerAnglesVelocity);
                slimeRigidbody.MoveRotation(slimeRigidbody.rotation * deltaRotation);
                facingWest = true;
            }
            else if (horizontalInput > 0 && facingWest)
            {
                Quaternion deltaRotation = Quaternion.Euler(eulerAnglesVelocity);
                slimeRigidbody.MoveRotation(slimeRigidbody.rotation * deltaRotation);
                facingWest = false;
            }

            if (Physics.OverlapSphere(transform.position, 0.1f, playerMask).Length == 0)
            {
                return;
            }

            if (jumpKeyWasPressed)
            {
                slimeRigidbody.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
                jumpKeyWasPressed = false;
            }
        }
    }
}