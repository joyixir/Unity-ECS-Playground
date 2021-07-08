using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace DefaultNamespace.DataComponents
{
    [GenerateAuthoringComponent]
    [MaterialProperty("_MyColor", MaterialPropertyFormat.Float4)]
    public struct ChangeCubeColorData : IComponentData
    {
        public float4 Value;
    }
}