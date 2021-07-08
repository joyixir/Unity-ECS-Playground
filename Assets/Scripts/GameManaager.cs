using DefaultNamespace;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;
using Material = UnityEngine.Material;

public class GameManaager : MonoBehaviour
{
    public static GameManaager instance;
    public GameObject ballPrefab;
    public Text score;


    public int maxScore;
    public int cubePerFrame;
    public GameObject cubePrefab;

    private Entity cubeEntityPrefab;

    public Material Material;
    public Mesh Mesh;
    
    public int curScore { get; set; }

    private bool insaneMode = false;
    private Entity _ballEntity;
    private EntityManager _entityManager;
    private BlobAssetStore _blobAsset;
    [SerializeField] private float cubeSpeed;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;

        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _blobAsset = new BlobAssetStore();
        GameObjectConversionSettings conversionSettings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAsset);
        _ballEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, conversionSettings);
        cubeEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubePrefab, conversionSettings);
    }

    private void OnDestroy()
    {
        _blobAsset.Dispose();
    }

    private void Start()
    {
        curScore = 0;
        DisplayScore();
        SpawnBall();
        insaneMode = false;

    }

    private void Update()
    {
        if (!insaneMode && curScore >= maxScore)
        {
            // insaneMode = true;
            for (int i = 0; i < cubePerFrame; i++)
            {
                SpawnNewCube();
            }
        }
    }

    private void SpawnNewCube()
    {
        Entity newCubeEntity = _entityManager.Instantiate(cubeEntityPrefab);
        Vector3 direction = Vector3.up;
        Vector3 speed = direction * cubeSpeed;

        PhysicsVelocity velocity = new PhysicsVelocity
        {
            Linear = speed,
            Angular = float3.zero
        };

        _entityManager.AddComponentData(newCubeEntity, velocity);
        _entityManager.SetSharedComponentData(newCubeEntity, new RenderMesh
        {
            material = new Material(Material),
            mesh = Mesh
        });
    }

    public void IncreaseScore()
    {
        curScore++;
        DisplayScore();
    }

    private void DisplayScore()
    {
        score.text = "Score : " + curScore;
    }

    private void SpawnBall()
    {
        Entity newBallEntity = _entityManager.Instantiate(_ballEntity);
        Translation ballTranslation = new Translation
        {
            Value = new float3(0, 0, 0)
        };
        
        _entityManager.AddComponentData(newBallEntity, ballTranslation);
        CameraFollow.instance.BallEntity = newBallEntity;
    }

}
