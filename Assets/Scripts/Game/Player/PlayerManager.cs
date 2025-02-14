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
        [SerializeField] Vector3 CachedPosition;

        private bool _isOwner;
        private Action _moveAction;

        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>(
            writePerm: NetworkVariableWritePermission.Owner);


        string _playerName;
        [SerializeField] TMP_Text _playerNameText;

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

        // private void Update()
        // {
        //     if (_isOwner)
        //     {
        //         if (Input.GetMouseButton(0))
        //         {
        //             Vector3 mousePos = Input.mousePosition;
        //
        //             _direction = (mousePos - transform.position).normalized;
        //             float distanceToMouse = Vector3.Distance(transform.position, mousePos);
        //
        //
        //             if (distanceToMouse - stopDistance < 0)
        //             {
        //                 return; // Stop movement
        //             }
        //
        //             if (_direction != Vector3.zero)
        //             {
        //                 float angle = Mathf.Atan2(-_direction.x, _direction.y) * Mathf.Rad2Deg;
        //                 transform.rotation = Quaternion.Euler(0, 0, angle);
        //             }
        //         }
        //
        //         Vector3 moveDirection = Vector3.zero;
        //         if (Input.GetKey(KeyCode.W))
        //         {
        //             moveDirection += _direction;
        //         }
        //
        //         if (Input.GetKey(KeyCode.S))
        //         {
        //             moveDirection -= _direction;
        //         }
        //
        //         if (Input.GetKey(KeyCode.A))
        //         {
        //             moveDirection += new Vector3(-_direction.y, _direction.x, 0);
        //         }
        //
        //         if (Input.GetKey(KeyCode.D))
        //         {
        //             moveDirection += new Vector3(_direction.y, -_direction.x, 0);
        //         }
        //
        //         Vector3 newPosition = transform.position + moveDirection.normalized * speed;
        //         newPosition.x =
        //             Mathf.Clamp(newPosition.x, _bottomLeftPosition.x + stopDistance,
        //                 _bottomRightPosition.x - stopDistance);
        //         newPosition.y =
        //             Mathf.Clamp(newPosition.y, _bottomLeftPosition.y + stopDistance,
        //                 _topLeftPosition.y - stopDistance);
        //         transform.position = newPosition;
        //     }
        //     else
        //     {
        //         transform.position = Position.Value;
        //     }
        // }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
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


        void Update()
        {
      

            if (IsLocalPlayer)
            {
                if (Input.GetMouseButton(0)) // Check if the left mouse button is held
                {
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 direction = (mousePos - transform.position).normalized;
                    float distanceToMouse = Vector3.Distance(transform.position, mousePos);
                    if (distanceToMouse <= stopDistance)
                    {
                        return; 
                    }
                    if (direction != Vector3.zero)
                    {
                        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, angle);
                    }
                }
                if (Input.GetKey(KeyCode.A))
                {
                    CachedPosition = Movement(TypeMove.left);
                    Move();
                }

                if (Input.GetKey(KeyCode.D))
                {
                    CachedPosition = Movement(TypeMove.right);
                    Move();
                }

                if (Input.GetKey(KeyCode.W))
                {
                    CachedPosition = Movement(TypeMove.forward);
                    Move();
                }

                if (Input.GetKey(KeyCode.S))
                {
                    CachedPosition = Movement(TypeMove.backward);
                    Move();
                }
            }

            transform.localPosition = Position.Value;
        }


        public void Move()
        {
            SubmitPositionRequestRpc();
            Sendclient();
        }

        [Rpc(SendTo.Server)]
        void SubmitPositionRequestRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetcurrentPosition();
            transform.localPosition += randomPosition;
            Position.Value = transform.localPosition;
        }

        void Sendclient(RpcParams rpcParams = default)
        {
            var randomPosition = GetcurrentPosition();
            transform.localPosition += randomPosition;
            Position.Value = transform.localPosition;
        }

        public Vector3 GetcurrentPosition()
        {
            return CachedPosition;
        }

        Vector3 Movement(TypeMove typeMove)
        {
            Vector3 moveDirection = Vector3.zero;
            switch (typeMove)
            {
                case TypeMove.left:
                    moveDirection = Vector3.left; // (-1, 0, 0)
                    break;
                case TypeMove.right:
                    moveDirection = Vector3.right; // (1, 0, 0)
                    break;
                case TypeMove.forward:
                    moveDirection = Vector3.up; // (0, 1, 0)
                    break;
                case TypeMove.backward:
                    moveDirection = Vector3.down; // (0, -1,0)
                    break;
            }

            var postion = moveDirection * speed * Time.deltaTime;
            return postion;
        }

        enum TypeMove
        {
            left,
            right,
            forward,
            backward
        }
    }
}