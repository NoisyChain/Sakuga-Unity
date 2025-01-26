using UnityEngine;
using System;

namespace SakugaEngine
{
    public partial class FighterCamera : MonoBehaviour
    {
        //private Listener audioListener;

        public bool isCinematic;
        public Vector2 minBounds = new Vector2(-5.5f, 1.25f), maxBounds = new Vector2(5.50f, 10f);
        //public int limitPlayersDistance = 600;
        public Vector2 minOffset = new Vector2(-4f, 1.2f), maxOffset = new Vector2(-5f, 1.55f);
        public float minSmoothDistance = 4;
        public float minDistance = 4f, maxDistance = 5.5f;
        public float boundsAdditionalNear = 2.3f, boundsAdditionalFar = 2.95f;

        private Camera thisCam;
        [SerializeField] private Camera charCam;

        const float DELTA = 1f / Global.TicksPerSecond;

        public void Start()
        {
            thisCam = GetComponent<Camera>();
            //charCam = GetNode<Camera3D>("../CanvasLayer/ViewportContainer/Viewport_Foreground/CharacterCamera");
            //audioListener = GetNode<Listener>("Listener");
        }

        public void UpdateCamera(Transform player1, Transform player2)
        {
            if (player1 == null || player2 == null) return;

            Vector3 _p1Position = player1.position;
            Vector3 _p2Position = player2.position;

            bool canSmooth = Mathf.Abs(_p2Position.x - _p1Position.x) > minSmoothDistance;

            float playerDistance = Mathf.Clamp(Mathf.Abs(_p2Position.x - _p1Position.x), minDistance, maxDistance);
            float pl = (playerDistance - minDistance) / (maxDistance - minDistance);
            float FinalYOffset = Mathf.Lerp(minOffset.y, maxOffset.y, pl);
            float FinalZOffset = Mathf.Lerp(minOffset.x, maxOffset.x, pl);

            float finalCamY = 0;
            if (Mathf.Max(_p1Position.y, _p2Position.y) >= FinalYOffset)
                finalCamY = Mathf.Max(_p1Position.y, _p2Position.y);
            else
                finalCamY = FinalYOffset;

            float actualCenter = (_p1Position.x + _p2Position.x) / 2;
            Vector3 newCamPosition = new Vector3(actualCenter, finalCamY, 0);

            float BoundsAdd = Mathf.Lerp(boundsAdditionalNear, boundsAdditionalFar, pl);
            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, newCamPosition.x, 10f * DELTA),
                Mathf.Lerp(transform.position.y, newCamPosition.y, 10f * DELTA),
                0);
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minBounds.x + BoundsAdd, maxBounds.x - BoundsAdd),
                Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y), 
                FinalZOffset);
            
            if (charCam != null)
            {
                charCam.transform.position = transform.position;
                charCam.transform.rotation = transform.rotation;
                charCam.fieldOfView = thisCam.fieldOfView;
            }

            //audioListener.GlobalTranslation = new Vector3(Position.X, Position.Y, 0);
        }
    }
}
