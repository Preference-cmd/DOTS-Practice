using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Rendering;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public partial struct TankSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // System only updates if at least one entity with Config component exists
        state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Disable the system in the first update means that the system will only update once
        state.Enabled = false;

        int callCount = 0;

        // Get the config
        var config = SystemAPI.GetSingleton<Config>();

        // Set the random seed
        var random = new Random(123);

        for (int i = 0; i < config.tankCount; i++)
        {
            // Instantiate the tank prefab
            var tankEntity = state.EntityManager.Instantiate(config.tankPrefab);

            if (i == 0)
            {
                // Add player component to the first tank
                state.EntityManager.AddComponent<Player>(tankEntity);
            }

            // Set the color of the tank
            var color = new URPMaterialPropertyBaseColor{
                Value = RandomColor(ref random)
            };

            // LinkedEntityGroup is a dynamic struct
            var linkedEntities = state.EntityManager.GetBuffer<LinkedEntityGroup>(tankEntity);
            foreach(var entity in linkedEntities)
            {
                // if the entity has the URPMaterialPropertyBaseColor component, 
                // set the color by using EnitiyManager ---- A very ECS-style way
                if(state.EntityManager.HasComponent<URPMaterialPropertyBaseColor>(entity.Value))
                {
                    state.EntityManager.SetComponentData(entity.Value, color);
                }

                callCount++;

                Debug.Log("Call count: " + callCount + " in tank instantiation: " + i );
            }
        }
    }

    static float4 RandomColor(ref Random random)
    {
        // 0.618034005f is inverse of the golden ratio
        var hue = (random.NextFloat() + 0.618034005f) % 1;
        return (Vector4)Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}
