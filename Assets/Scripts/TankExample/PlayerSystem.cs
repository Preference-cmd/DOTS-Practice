using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct PlayerSystem : ISystem
{
    // we cannot use [BurstCompile] here because we are accessing managed objects(like camera)
    public void OnUpdate(ref SystemState state)
    {
        var movement = new float3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );
        movement *= movement*SystemAPI.Time.DeltaTime;                  

        foreach(var playerTransform in 
            SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Player>()
        )
        {
            // move the player
            playerTransform.ValueRW.Position += movement;

            // move the camera to follow the player
            var camTransform = Camera.main.transform;
            camTransform.position = playerTransform.ValueRO.Position; // Get the player's position
            camTransform.position -= -10f*(Vector3)playerTransform.ValueRO.Forward(); // move the camera backwards
            camTransform.position += new Vector3(0, 5f, 0); // move the camera up
            camTransform.LookAt(playerTransform.ValueRO.Position); // look at the player
        }
    }
}
