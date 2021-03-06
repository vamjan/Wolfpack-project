﻿using UnityEngine;

using System.Collections;
using Wolfpack.Characters;

namespace Wolfpack
{
    public class CameraFollowScript : MonoBehaviour
    {
        public PlayerCharacterScript target;
        public float damping = 0.2f;
        public float lookAheadFactor = 5;

        private float offsetZ;
        private Vector3 currentVelocity;

        // Use this for initialization
        void Start()
        {
            offsetZ = transform.position.z - target.transform.position.z;
            transform.parent = null;
        }

        // Update is called once per frame
        void Update()
        {
            //direction facing by main character
            Vector3 direction = new Vector3(-lookAheadFactor * target.direction.x, -lookAheadFactor * target.direction.y, 1);
            //desired camera position
            Vector3 aheadTargetPos = target.transform.position + direction * offsetZ;
            //actual camera position
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);
            //move camera
            transform.position = newPos;

            Debug.DrawLine(transform.position, new Vector3(aheadTargetPos.x, aheadTargetPos.y, 10),
            Color.green, 0.0f, false);
        }
    }
}