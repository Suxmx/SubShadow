using UnityEngine;
using UnityEngine.Tilemaps;

public class BackgroundBuilder: MonoBehaviour
{
    private Tile[] tiles;
    private Tile tile;
    private Tilemap tilemap;
    private Transform camTransform;

    // Update����Ҫ�ı�������ǰ����ռ�
    private Vector3Int cell, leftDownCell, rightUpCell, deltaCell;
    private Vector3 prePos, deltaPos, cellSize;

    private void Awake()
    {
        tiles = new Tile[4];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = Resources.Load<Tile>($"BackgroundTile{i + 1}");
        }
        tile = tiles[Random.Range(0, tiles.Length)];
        tilemap = GetComponent<Tilemap>();
        Camera cam = Camera.main;
        camTransform = cam.transform;

        Vector3 rightUpPos = cam.ViewportToWorldPoint(Vector2.one);
        rightUpCell = tilemap.WorldToCell(rightUpPos) + new Vector3Int(1, 1, 0);
        cell = tilemap.WorldToCell(camTransform.position);
        deltaCell = rightUpCell - cell;
        leftDownCell = cell - deltaCell;
        prePos = camTransform.position;
        cellSize = tilemap.cellSize;

        SetAllTiles();
    }

    private void LateUpdate()
    {
        SetTiles();
    }

    private void CalTiles()
    {
        // �������½������Ͻǵ�Cell����
        cell = tilemap.WorldToCell(camTransform.position);
        leftDownCell = cell - deltaCell;
        rightUpCell = cell + deltaCell;
    }

    private void SetTiles()
    {
        CalTiles();

        // �����������deltaλ��
        deltaPos = camTransform.position - prePos;
        prePos = camTransform.position;

        // �������λ���㹻��ʱ������Tile�������ã�����ֻ��������ϵ�һ�м�һ��
        if (deltaPos.x > cellSize.x || deltaPos.y > cellSize.y) SetAllTiles();
        else
        {
            SetTilesVertically(deltaPos.x > 0f);
            SetTilesHorizontally(deltaPos.y > 0f);
        }
    }

    private void SetAllTiles()
    {
        for (cell.x = leftDownCell.x; cell.x <= rightUpCell.x; cell.x++)
        {
            for (cell.y = leftDownCell.y; cell.y <= rightUpCell.y; cell.y++)
            {
                if (!tilemap.HasTile(cell))
                {
                    tilemap.SetTile(cell, tile);
                }
            }
        }
    }

    private void SetTilesVertically(bool right)
    {
        cell.x = right ? rightUpCell.x : leftDownCell.x;
        for (cell.y = leftDownCell.y; cell.y <= rightUpCell.y; cell.y++)
        {
            if (!tilemap.HasTile(cell))
            {
                tilemap.SetTile(cell, tile);
            }
        }
    }

    private void SetTilesHorizontally(bool up)
    {
        cell.y = up ? rightUpCell.y : leftDownCell.y;
        for (cell.x = leftDownCell.x; cell.x <= rightUpCell.x; cell.x++)
        {
            if (!tilemap.HasTile(cell))
            {
                tilemap.SetTile(cell, tile);
            }
        }
    }
}
