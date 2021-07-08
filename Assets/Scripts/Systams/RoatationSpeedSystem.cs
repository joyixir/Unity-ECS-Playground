using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class RoatationSpeedSystem : JobComponentSystem
{

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        var random = Random.CreateFromIndex(1);
        
        Entities.ForEach((ref Rotation rotation, ref RotationSpeedData speedData) =>
        {
            rotation.Value = math.mul(rotation.Value, quaternion.RotateX(math.radians(random.NextFloat(-1*speedData.speed, speedData.speed)  * deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateY(math.radians(random.NextFloat(-1*speedData.speed, speedData.speed)  * deltaTime)));
            rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(math.radians(random.NextFloat(-1*speedData.speed, speedData.speed) * deltaTime)));
        }).Run();
        return default;
    }
}
