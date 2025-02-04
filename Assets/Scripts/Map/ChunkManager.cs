using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public int chunkSize = 16;
    public int loadDistance = 3;  // Yüklenmesi gereken chunk mesafesi
    public Tilemap tilemap;  // Tilemap referansý
    public TileBase[] tiles;  // Tile prefab'larý dizisi (farklý türdeki tile'lar)

    private HashSet<Vector2> loadedChunks = new HashSet<Vector2>();
    private Dictionary<Vector2, List<Vector3Int>> chunkTilePositions = new Dictionary<Vector2, List<Vector3Int>>();

    void Start()
    {
        LoadNearbyChunks();
    }

    void Update()
    {
        // Yeni chunk'larý yükle
        LoadNearbyChunks();

        // Uzaklaþan chunk'larý indir
        UnloadChunks();
    }

    // Yüklenmesi gereken chunk'larý kontrol et
    void LoadNearbyChunks()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector2 chunkPosition = new Vector2(Mathf.Floor(cameraPosition.x / chunkSize), Mathf.Floor(cameraPosition.y / chunkSize));

        // Yüklenmesi gereken chunk'larý kontrol et
        for (int i = -loadDistance; i <= loadDistance; i++)
        {
            for (int j = -loadDistance; j <= loadDistance; j++)
            {
                Vector2 chunkCoord = chunkPosition + new Vector2(i, j);

                // Eðer chunk daha önce yüklenmediyse, yükle
                if (!loadedChunks.Contains(chunkCoord))
                {
                    LoadChunk(chunkCoord);
                }
            }
        }
    }

    // Chunk'ý yükle
    void LoadChunk(Vector2 chunkCoord)
    {
        // Chunk'ý oluþtur (rastgele seed kullanarak)
        int chunkSeed = (int)(chunkCoord.x * 1000 + chunkCoord.y);
        System.Random rand = new System.Random(chunkSeed);

        // Eðer bu chunk daha önce oluþturulmuþsa, önceden oluþturulan tile pozisyonlarýný yükle
        if (!chunkTilePositions.ContainsKey(chunkCoord))
        {
            // Eðer daha önce oluþturulmamýþsa, yeni tile pozisyonlarýný rastgele oluþtur
            GenerateTiles(chunkCoord, rand);
        }
        else
        {
            // Daha önce oluþturulmuþ chunk'ý yükle
            LoadExistingChunk(chunkCoord);
        }

        // Yüklenen chunk'ý ekle
        loadedChunks.Add(chunkCoord);
    }

    // Chunk'ý yüklerken mevcut tile'larý tekrar yükle
    void LoadExistingChunk(Vector2 chunkCoord)
    {
        if (chunkTilePositions.ContainsKey(chunkCoord))
        {
            foreach (var position in chunkTilePositions[chunkCoord])
            {
                tilemap.SetTile(position, tilemap.GetTile(position));
            }
        }
    }

    // Tilemap üzerinde chunk'ý oluþtur
    void GenerateTiles(Vector2 chunkCoord, System.Random rand)
    {
        List<Vector3Int> tilePositions = new List<Vector3Int>();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int tilePosition = new Vector3Int((int)(chunkCoord.x * chunkSize + x), (int)(chunkCoord.y * chunkSize + y), 0);

                // Rastgele tile seç
                TileBase randomTile = tiles[rand.Next(tiles.Length)];

                // Tilemap üzerinde tile yerleþtir
                tilemap.SetTile(tilePosition, randomTile);

                // Tile pozisyonlarýný kaydet
                tilePositions.Add(tilePosition);
            }
        }

        // Tile pozisyonlarýný kaydet
        chunkTilePositions[chunkCoord] = tilePositions;
    }

    // Uzaklaþan chunk'larý indir
    void UnloadChunks()
    {
        // Bu fonksiyonu gerektiðinde eski chunk'larý silmek için kullanabilirsiniz
        // Örneðin, oyuncudan çok uzak olan chunk'lar silinebilir.
    }
}
