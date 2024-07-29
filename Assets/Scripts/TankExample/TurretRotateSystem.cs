using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
public partial struct TurretRotateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
    }


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var spin = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);
        
        foreach(var tank in SystemAPI.Query<RefRW<Tank>>())
        {
            var trans = SystemAPI.GetComponentRW<LocalTransform>(tank.ValueRO.turret);
            trans.ValueRW.Rotation = math.mul(spin, trans.ValueRO.Rotation);
        }
    }
}