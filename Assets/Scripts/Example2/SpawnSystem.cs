using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CubeSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        // we can make sure that only one entity has the spawner component
        var prefab = SystemAPI.GetSingleton<CubeSpawner>().cubePrefab;

        // Call the EntityManager.Instantiate method to create 10 instances of the prefab
        // and return the NativeArray of Entity IDs.
        var instance = state.EntityManager.Instantiate(prefab, 10, Allocator.Temp);

        // Randomly position the instances within a 10x10x10 cube centered at the origin
        var random = new Random(123); // seed the random number generator
        foreach(var entities in instance)
        {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entities);
            transform.ValueRW.Position = random.NextFloat3(new float3(10, 10, 10));
        }
    }
}
