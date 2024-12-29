using UnityEngine;

public class Stone : MonoBehaviour
{
    public GemControler gemControler; // Tham chiếu đến GemControler

    private void OnMouseDown()
    {
        Vector3 stonePosition = transform.position;

        // Snap vị trí theo grid trước khi thực hiện các thao tác
        stonePosition = gemControler.SnapToGrid(stonePosition);

        

        // Tạo gem ngẫu nhiên với xác suất 30%
        if (Random.value < 0.3f) // 30% xác suất
        {
            gemControler.SpawnRandomGem(stonePosition);
        }
        // Phá hủy stone
        Destroy(gameObject);
        // Đánh dấu vị trí stone đã bị phá hủy và loại bỏ khỏi danh sách
        gemControler.RemoveStonePosition(stonePosition);  // Xóa khỏi allStonePositions
    }
}
