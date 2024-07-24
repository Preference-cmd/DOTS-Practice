using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;


public partial struct TankMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        // For each entity with localtransform and tank component, 
        // we access the localtransform and entity id
        // then update the LocalTransform to move the tank along a random curve
        foreach(var (localtransform, entity) in 
            SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Tank>().WithEntityAccess()
        )
        {
            var pos = localtransform.ValueRO.Position;
            pos.y = (float)entity.Index;

            // compute the angle and the direction 
            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);

            localtransform.ValueRW.Position += dir * dt * 5f;
            localtransform.ValueRW.Rotation = quaternion.RotateY(angle);
        }
    }
}
