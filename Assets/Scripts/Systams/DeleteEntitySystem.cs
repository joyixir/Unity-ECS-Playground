using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(PickUpSystem))]
public class DeleteEntitySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer buffer = new EntityCommandBuffer(Allocator.TempJob);

        Entities
            .WithAll<DeleteTag>()
            .WithoutBurst()
            .ForEach((Entity entity) =>
            {
                GameManaager.instance.IncreaseScore();
                buffer.DestroyEntity(entity);
            }).Run();
        buffer.Playback(EntityManager);
        buffer.Dispose();

        return default; 
    }
}