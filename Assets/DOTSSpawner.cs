using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class DOTSSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private Entity entityPrefab;
    private World defaultWorld;
    private EntityManager entityManager;
    BlobAssetStore store;

    private void Start() {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;
        store = new BlobAssetStore();

        var settings = GameObjectConversionSettings.FromWorld(defaultWorld, store);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);

        InstantiateEntity(new float3(0f, 0f, 100f));
    }

    void InstantiateEntity(float3 position)
    {
        Entity entity = entityManager.Instantiate(entityPrefab);
        entityManager.SetComponentData(entity, new Translation {
            Value = position
        });
    }

    void Generate()
    {

    }
}
