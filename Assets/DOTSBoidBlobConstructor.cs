using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Collections;

public class DOTSBoidBlobConstructor : GameObjectConversionSystem
{
    protected override void OnUpdate()
    {
        using (BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp)) {
            ref DOTSBoidBlobAsset blobAsset = ref blobBuilder.ConstructRoot<DOTSBoidBlobAsset>();

            BlobAssetReference<DOTSBoidBlobAsset> blobAssetRef = blobBuilder.CreateBlobAssetReference<DOTSBoidBlobAsset>(Allocator.Persistent);
        }
    }
}
