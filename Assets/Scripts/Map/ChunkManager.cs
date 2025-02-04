using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class ChunkManager : MonoBehaviour
{
    public int chunkSize = 16;
    public int loadDistance = 3;  // Y�klenmesi gereken chunk mesafesi
    public Tilemap tilemap;  // Tilemap referans�
    public TileBase[] tiles;  // Tile prefab'lar� dizisi (farkl� t�rdeki tile'lar)

    private HashSet<Vector2> loadedChunks = new HashSet<Vector2>();
    private Dictionary<Vector2, List<Vector3Int>> chunkTilePositions = new Dictionary<Vector2, List<Vector3Int>>();

    void Start()
    {
        LoadNearbyChunks();
    }

    void Update()
    {
        // Yeni chunk'lar� y�kle
        LoadNearbyChunks();

        // Uzakla�an chunk'lar� indir
        UnloadChunks();
    }

    // Y�klenmesi gereken chunk'lar� kontrol et
    void LoadNearbyChunks()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector2 chunkPosition = new Vector2(Mathf.Floor(cameraPosition.x / chunkSize), Mathf.Floor(cameraPosition.y / chunkSize));

        // Y�klenmesi gereken chunk'lar� kontrol et
        for (int i = -loadDistance; i <= loadDistance; i++)
        {
            for (int j = -loadDistance; j <= loadDistance; j++)
            {
                Vector2 chunkCoord = chunkPosition + new Vector2(i, j);

                // E�er chunk daha �nce y�klenmediyse, y�kle
                if (!loadedChunks.Contains(chunkCoord))
                {
                    LoadChunk(chunkCoord);
                }
            }
        }
    }

    // Chunk'� y�kle
    void LoadChunk(Vector2 chunkCoord)
    {
        // Chunk'� olu�tur (rastgele seed kullanarak)
        int chunkSeed = (int)(chunkCoord.x * 1000 + chunkCoord.y);
        System.Random rand = new System.Random(chunkSeed);

        // E�er bu chunk daha �nce olu�turulmu�sa, �nceden olu�turulan tile pozisyonlar�n� y�kle
        if (!chunkTilePositions.ContainsKey(chunkCoord))
        {
            // E�er daha �nce olu�turulmam��sa, yeni tile pozisyonlar�n� rastgele olu�tur
            GenerateTiles(chunkCoord, rand);
        }
        else
        {
            // Daha �nce olu�turulmu� chunk'� y�kle
            LoadExistingChunk(chunkCoord);
        }

        // Y�klenen chunk'� ekle
        loadedChunks.Add(chunkCoord);
    }

    // Chunk'� y�klerken mevcut tile'lar� tekrar y�kle
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

    // Tilemap �zerinde chunk'� olu�tur
    void GenerateTiles(Vector2 chunkCoord, System.Random rand)
    {
        List<Vector3Int> tilePositions = new List<Vector3Int>();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int tilePosition = new Vector3Int((int)(chunkCoord.x * chunkSize + x), (int)(chunkCoord.y * chunkSize + y), 0);

                // Rastgele tile se�
                TileBase randomTile = tiles[rand.Next(tiles.Length)];

                // Tilemap �zerinde tile yerle�tir
                tilemap.SetTile(tilePosition, randomTile);

                // Tile pozisyonlar�n� kaydet
                tilePositions.Add(tilePosition);
            }
        }

        // Tile pozisyonlar�n� kaydet
        chunkTilePositions[chunkCoord] = tilePositions;
    }

    // Uzakla�an chunk'lar� indir
    void UnloadChunks()
    {
        // Bu fonksiyonu gerekti�inde eski chunk'lar� silmek i�in kullanabilirsiniz
        // �rne�in, oyuncudan �ok uzak olan chunk'lar silinebilir.
    }
}
