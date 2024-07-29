using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;

public partial struct ShootingSystem : ISystem
{
    private float timer;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // cool down when timer is not expired
        timer -= SystemAPI.Time.DeltaTime;
        if(timer > 0f)
        {
            return;
        }
        timer = 0.3f;

        var config = SystemAPI.GetSingleton<Config>();
        var ballTransform = state.EntityManager.GetComponentData<LocalTransform>(config.cannonBallPrefab);

        foreach (var (tank, color) in
                 SystemAPI.Query<RefRO<Tank>, RefRO<URPMaterialPropertyBaseColor>>())
        {
            Entity cannonBallEntity = state.EntityManager.Instantiate(config.cannonBallPrefab);
            
            // Set color of the cannonball to match the tank that shot it.
            state.EntityManager.SetComponentData(cannonBallEntity, color.ValueRO);
            
            // We need the transform of the cannon in world space, so we get its LocalToWorld instead of LocalTransform.
            var cannonTransform = state.EntityManager.GetComponentData<LocalToWorld>(tank.ValueRO.cannon);
            ballTransform.Position =  cannonTransform.Position;
            
            // Set position of the new cannonball to match the spawn point
            state.EntityManager.SetComponentData(cannonBallEntity, ballTransform);

            // Set velocity of the cannonball to shoot out of the cannon.
            state.EntityManager.SetComponentData(cannonBallEntity, new CannonBall
            {
                velocity = math.normalize(cannonTransform.Up) * 12.0f
            });
        }
    }
}