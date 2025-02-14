using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            SubmitPositionRequestRpc();
        }

        [Rpc(SendTo.Server)]
        void SubmitPositionRequestRpc(RpcParams rpcParams = default)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
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