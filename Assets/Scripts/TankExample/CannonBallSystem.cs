using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct CannonBallSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // ECB is a special system that allows us to create a command buffer that stores commands which need
        // to be executed in the main thread such as destroying entities.
        // By using ECB, we could execute these commands in other threads, that's said, execute them in Job system.
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        var cannonBallJob = new CannonBallJob
        {
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        cannonBallJob.Schedule();
    }
}


[BurstCompile]
public partial struct CannonBallJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public float DeltaTime;

    void Execute(Entity entity, ref CannonBall cannonBall, ref LocalTransform transform)
    {
        var gravity = new float3(0, -9.8f, 0);

        transform.Position += cannonBall.velocity * DeltaTime;

        // if CannonBall hits the ground
        if (transform.Position.y <= 0.0f)
        {
            ECB.DestroyEntity(entity);
        }

        // add gravity to CannonBall velocity
        cannonBall.velocity += gravity * DeltaTime;
    }
}
