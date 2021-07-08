
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

[AlwaysSynchronizeSystem]
public class MovemenetSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float2 curInput = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Entities.ForEach((ref PhysicsVelocity velocity, ref SpeedData speedData) =>
        {
            float2 newVelocity = velocity.Linear.xz;
            newVelocity += curInput * speedData.speed * deltaTime;
            velocity.Linear.xz = newVelocity;
            
        }).Run();

        return default;
    }
}
