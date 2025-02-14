using System;
using CoreGame;
using UnityEngine;

namespace Game
{
    public class LandController : MonoBehaviour
    {
        [SerializeField] private GameObject _topLeft;
        [SerializeField] private GameObject _topRight;
        [SerializeField] private GameObject _bottomLeft;
        [SerializeField] private GameObject _bottomRight;
        [SerializeField] private Vector3 _topLeftPosition;
        [SerializeField] private Vector3 _topRightPosition;
        [SerializeField] private Vector3 _bottomLeftPosition;
        [SerializeField] private Vector3 _bottomRightPosition;
        public Vector3 TopLeftPosition => _topLeftPosition;
        public Vector3 TopRightPosition => _topRightPosition;
        public Vector3 BottomLeftPosition => _bottomLeftPosition;
        public Vector3 BottomRightPosition => _bottomRightPosition;

        private void Awake()
        {
            Locator<LandController>.Set(this);
            _topLeftPosition = _topLeft.transform.localPosition;
            _topRightPosition = _topRight.transform.localPosition;
            _bottomLeftPosition = _bottomLeft.transform.localPosition;
            _bottomRightPosition = _bottomRight.transform.localPosition;
        }

        private void Start()
        {
        }
    }
}