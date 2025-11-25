using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class CameraController : MonoBehaviour
    {
        private static CameraController instance;

        public float damping = 1.5f; 
        public Vector2 offset = new Vector2(0f, 0f);
        public bool faceLeft;

        private Transform player;
        private int lastX;


        // ---------- MAKE CAMERA PERSISTENT ----------
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }


        void Start()
        {
            offset = new Vector2(Mathf.Abs(offset.x), offset.y);
            ReconnectPlayer();
        }

        // Called after scene changes
        void OnLevelWasLoaded(int level)
        {
            ReconnectPlayer();
        }



        // ---------- FIND PLAYER AGAIN AFTER SCENE LOAD ----------
        void ReconnectPlayer()
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
            {
                player = p.transform;
                lastX = Mathf.RoundToInt(player.position.x);
                SetInitialPosition();
            }
        }



        void SetInitialPosition()
        {
            if (player == null) return;

            if (faceLeft)
                transform.position = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            else
                transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }



        void Update()
        {
            if (!player)
                return;

            int currentX = Mathf.RoundToInt(player.position.x);

            if (currentX > lastX) faceLeft = false;
            else if (currentX < lastX) faceLeft = true;

            lastX = Mathf.RoundToInt(player.position.x);

            Vector3 target;
            if (faceLeft)
                target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            else
                target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            transform.position = currentPosition;
        }
    }
}
