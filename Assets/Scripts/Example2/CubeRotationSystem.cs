using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

// Each system will be loaded as we enter play mode
public partial struct CubeRotationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;

        // In order to rotate the cubes, we need to do following steps:
        // 1. Query all the entities that have LocalTransform component and RotationSpeed component
        // 2. Loop through the entities
        foreach (var(localTransform, rotationSpeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
        {
            // 3. Rotate the cube by the rotation speed
            var radians = rotationSpeed.ValueRO.radiansPerSecond * deltaTime;
            localTransform.ValueRW = localTransform.ValueRW.RotateY(radians); 
        }
    }
}
