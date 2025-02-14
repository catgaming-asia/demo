using System;
using CoreGame;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Game.Player
{
    public class PlayerManager : NetworkBehaviour
    {
        public float speed = 5f; // Movement speed
        public float stopDistance = 100f; // Stop moving when within this range of the mouse
        [SerializeField] private Vector3 _topLeftPosition;
        [SerializeField] private Vector3 _topRightPosition;
        [SerializeField] private Vector3 _bottomLeftPosition;
        [SerializeField] private Vector3 _bottomRightPosition;
        [SerializeField] private Vector3 _cachedPosition;
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private float _cacheAngle;
        [SerializeField] private TypeNetwork _typeNetwork;
        private bool _isOwner;
        private Action _moveAction;
        private Vector3 _direction;
        private string _playerName;


        private NetworkVariable<Vector3> _positionSelfHost = new NetworkVariable<Vector3>(
            writePerm: NetworkVariableWritePermission.Owner);

        private NetworkVariable<float> _angleSelfHost = new NetworkVariable<float>(
            writePerm: NetworkVariableWritePermission.Owner);

        private NetworkVariable<Vector3> _positionServer = new NetworkVariable<Vector3>(
            writePerm: NetworkVariableWritePermission.Owner);

        private NetworkVariable<float> _angleServer = new NetworkVariable<float>(
            writePerm: NetworkVariableWritePermission.Owner);

        private NetworkVariable<Vector3> _position
        {
            get
            {
                if (_typeNetwork == TypeNetwork.SelfHost)
                {
                    return _positionSelfHost;
                }

                return _positionServer;
            }
            set
            {
                if (_typeNetwork == TypeNetwork.SelfHost)
                {
                    _positionSelfHost = value;
                }
                else
                {
                    _positionServer = value;
                }
            }
        }

        private NetworkVariable<float> _angle
        {
            get
            {
                if (_typeNetwork == TypeNetwork.SelfHost)
                {
                    return _angleSelfHost;
                }

                return _angleServer;
            }
            set
            {
                if (_typeNetwork == TypeNetwork.SelfHost)
                {
                    _angleSelfHost = value;
                }
                else
                {
                    _angleServer = value;
                }
            }
        }


        private void Start()
        {
            var landCtrl = Locator<LandController>.Instance;
            _topLeftPosition = landCtrl.TopLeftPosition;
            _topRightPosition = landCtrl.TopRightPosition;
            _bottomLeftPosition = landCtrl.BottomLeftPosition;
            _bottomRightPosition = landCtrl.BottomRightPosition;
            this.transform.parent = Locator<GameManager>.Instance.Parent.transform;
            this.transform.localScale = Vector3.one;
            this.transform.localPosition = new Vector3(0, 0, 0);
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                RpcClient();
            }

            if (IsLocalPlayer)
            {
                _playerName = "local " + NetworkObjectId;
                _playerNameText.text = _playerName;
                gameObject.name = _playerName;
            }
            else
            {
                _playerName = "Player " + NetworkObjectId;
                _playerNameText.text = _playerName;
                gameObject.name = _playerName;
            }
        }

        private void Update()
        {
            if (IsLocalPlayer)
            {
                if (Input.GetMouseButton(0)) // Check if the left mouse button is held
                {
                    Vector3 mousePos = Input.mousePosition;
                    _direction = (mousePos - transform.position).normalized;
                    float distanceToMouse = Vector3.Distance(transform.position, mousePos);
                    if (distanceToMouse <= stopDistance)
                    {
                        return;
                    }

                    if (_direction != Vector3.zero)
                    {
                        _cacheAngle = Mathf.Atan2(-_direction.x, _direction.y) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, _cacheAngle);
                    }
                }

                Vector3 moveDirection = Vector3.zero;

                if (Input.GetKey(KeyCode.W)) // Move forward (toward mouse)
                    moveDirection += _direction;

                if (Input.GetKey(KeyCode.S)) // Move backward (away from mouse)
                    moveDirection -= _direction;

                if (Input.GetKey(KeyCode.A)) // Move left (perpendicular to mouse direction)
                    moveDirection += new Vector3(-_direction.y, _direction.x, 0);

                if (Input.GetKey(KeyCode.D)) // Move right (perpendicular to mouse direction)
                    moveDirection += new Vector3(_direction.y, -_direction.x, 0);

                if (moveDirection != Vector3.zero)
                {
                    _cachedPosition = ClampPosition(moveDirection.normalized * speed);
                    RpcClient();
                }
            }
            else
            {
                transform.localPosition = _position.Value;
                transform.rotation = Quaternion.Euler(0, 0, _angle.Value);
            }
        }

        private void RpcClient(RpcParams rpcParams = default)
        {
            var currentPosition = GetcurrentPosition();
            var currentAngle = GetcurrentAngle();
            transform.localPosition += currentPosition;
            Vector3 position = ClampPosition(transform.localPosition);
            transform.localPosition = position;
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            _position.Value = transform.localPosition;
            _angle.Value = currentAngle;
        }

        private Vector3 GetcurrentPosition()
        {
            return _cachedPosition;
        }

        private float GetcurrentAngle()
        {
            return _cacheAngle;
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, _bottomLeftPosition.x, _bottomRightPosition.x);
            position.y = Mathf.Clamp(position.y, _bottomLeftPosition.y, _topLeftPosition.y);
            return position;
        }

        enum TypeNetwork
        {
            Server,
            SelfHost
        }
    }
}