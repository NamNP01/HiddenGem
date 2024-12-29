using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemControler : MonoBehaviour
{
    public float cellSize = 1.0f; // Kích thước mỗi ô trên grid
    public int gridWidth = 10;   // Số ô theo chiều ngang
    public int gridHeight = 10;  // Số ô theo chiều dọc

    public Dictionary<Vector3, bool> stonePositions = new Dictionary<Vector3, bool>();

    [System.Serializable]
    public class GemConfig
    {
        public GameObject gemPrefab; // Prefab của gem
        public Vector2Int size;      // Kích thước gem trên grid
        public int count;            // Số lượng gem còn lại
    }
    void Start()
    {
        // Gọi FindAllStones để lưu tất cả các vị trí stone khi game bắt đầu
        FindAllStones();
    }
    public List<GemConfig> gemConfigs; // Danh sách các gem có thể tạo

    // Danh sách các vị trí stone được public để có thể truy cập từ bên ngoài
    public List<Vector3> allStonePositions = new List<Vector3>();

    // Hàm để tìm tất cả các GameObject có tag "Stone" và lưu vị trí của chúng
    public void FindAllStones()
    {
        // Lấy tất cả các GameObject có tag "Stone"
        GameObject[] stoneObjects = GameObject.FindGameObjectsWithTag("Stone");

        // Lưu vị trí của từng stone vào danh sách allStonePositions
        allStonePositions.Clear();  // Xóa danh sách cũ trước khi thêm mới
        foreach (GameObject stone in stoneObjects)
        {
            if (stone != null)
            {
                allStonePositions.Add(stone.transform.position);
                Debug.Log("Stone found at: " + stone.transform.position);
            }
        }
    }

    // Xóa vị trí stone khỏi grid
    public void RemoveStonePosition(Vector3 position)
    {
        // Làm tròn vị trí nếu cần
        position = SnapToGrid(position);

        // Loại bỏ vị trí khỏi allStonePositions
        if (allStonePositions.Contains(position))
        {
            allStonePositions.Remove(position);  // Xóa vị trí stone khỏi danh sách
            Debug.Log("Stone removed from allStonePositions: " + position);
        }
        else
        {
            Debug.Log("Position not found in allStonePositions: " + position);
        }
    }


    //// Kiểm tra nếu có stone tại vị trí
    //public bool IsStoneAtPosition(Vector3 position)
    //{
    //    position = SnapToGrid(position);
    //    return stonePositions.ContainsKey(position);
    //}

    //// Lấy các vị trí trong phạm vi xung quanh một điểm
    //public List<Vector3> GetPositionsInRange(Vector3 center, int range)
    //{
    //    List<Vector3> positions = new List<Vector3>();
    //    center = SnapToGrid(center);

    //    for (int x = -range; x <= range; x++)
    //    {
    //        for (int z = -range; z <= range; z++)
    //        {
    //            // Loại bỏ các ô nằm ngoài phạm vi hình tròn
    //            if (Mathf.Abs(x) + Mathf.Abs(z) > range) continue;

    //            Vector3 position = center + new Vector3(x * cellSize, 0, z * cellSize);
    //            if (IsStoneAtPosition(position))
    //            {
    //                positions.Add(position);
    //            }
    //        }
    //    }

    //    return positions;
    //}

    // Chuyển vị trí sang tọa độ grid (snap theo kích thước gem)
    //public Vector3 SnapToGrid(Vector3 position, Vector2Int size)
    //{
    //    float x, y;

    //    // Kiểm tra nếu size.x là chẵn
    //    if (size.x % 2 == 0)
    //    {
    //        x = Mathf.Round(position.x / cellSize) * cellSize;
    //    }
    //    else
    //    {
    //        x = position.x;
    //    }

    //    // Kiểm tra nếu size.y là chẵn
    //    if (size.y % 2 == 0)
    //    {
    //        y = Mathf.Round(position.y / cellSize) * cellSize;
    //    }
    //    else
    //    {
    //        y = position.y;
    //    }

    //    return new Vector3(x, y, position.z);
    //}

    // Chuyển vị trí sang tọa độ grid (snap mặc định nếu không có kích thước gem)
    public Vector3 SnapToGrid(Vector3 position)
    {
        return new Vector3(
            position.x, // Làm tròn xuống x
            position.y,                                   // Giữ nguyên y
            Mathf.Floor(position.z / cellSize) * cellSize // Làm tròn xuống z
        );
    }


    // Tạo gem ngẫu nhiên tại vị trí stone
    //public void SpawnRandomGem(Vector3 stonePosition)
    //{
    //    // Đảm bảo danh sách gemConfigs không rỗng
    //    if (gemConfigs.Count == 0)
    //    {
    //        Debug.Log("No gems available to spawn.");
    //        return;
    //    }

    //    // Chọn ngẫu nhiên một gem từ danh sách
    //    int randomIndex = Random.Range(0, gemConfigs.Count);
    //    GemConfig selectedGem = gemConfigs[randomIndex];

    //    // Snap vị trí theo kích thước gem
    //    stonePosition = SnapToGrid(stonePosition, selectedGem.size);

    //    // Tạo gem tại vị trí đã làm tròn
    //    Instantiate(selectedGem.gemPrefab, stonePosition, Quaternion.identity);
    //    Debug.Log($"Spawned gem: {selectedGem.gemPrefab.name} at {stonePosition}");

    //    // Giảm số lượng gem
    //    selectedGem.count--;

    //    // Nếu số lượng gem giảm về 0, xóa khỏi danh sách
    //    if (selectedGem.count <= 0)
    //    {
    //        gemConfigs.RemoveAt(randomIndex);
    //        Debug.Log($"Gem {selectedGem.gemPrefab.name} removed from the list.");
    //    }

    //    // Loại bỏ vị trí stone
    //    RemoveStonePosition(stonePosition);
    //}
    public void SpawnRandomGem(Vector3 stonePosition)
    {
        if (gemConfigs.Count == 0)
        {
            Debug.Log("No gems available to spawn.");
            return;
        }

        int randomIndex = Random.Range(0, gemConfigs.Count);
        GemConfig selectedGem = gemConfigs[randomIndex];
        Debug.Log($"Selected Gem: {selectedGem.gemPrefab.name}");

        List<Vector3> validPositions = new List<Vector3>();

        // Snap stone position to grid
        stonePosition = SnapToGrid(stonePosition);

        // Kiểm tra trục X
        bool isXValid = true;
        for (int offsetX = 0; offsetX < selectedGem.size.x; offsetX++)
        {
            Vector3 forwardX = stonePosition + new Vector3(offsetX * cellSize, 0, 0);
            if (allStonePositions.Contains(forwardX))
            {
                validPositions.Add(forwardX);
                Debug.Log($"Added position to validPositions (X-axis): {forwardX}"); // Log khi thêm vào
            }
            else
            {
                // Kiểm tra ngược lại
                Vector3 backwardX = stonePosition - new Vector3(offsetX * cellSize, 0, 0);
                if (allStonePositions.Contains(backwardX))
                {
                    validPositions.Add(backwardX);
                    Debug.Log($"Added position to validPositions (X-axis): {backwardX}"); // Log khi thêm vào
                }
                else
                {
                    isXValid = false;
                    break;
                }
            }
        }

        // Nếu trục X không hợp lệ, dừng kiểm tra
        if (!isXValid)
        {
            Debug.Log("Cannot spawn gem: Invalid positions on X-axis.");
            return;
        }

        // Kiểm tra trục Y cho tất cả các vị trí hợp lệ trên trục X
        bool isYValid = true;
        List<Vector3> allYPositions = new List<Vector3>();

        foreach (Vector3 xPos in validPositions)
        {
            for (int offsetY = 0; offsetY < selectedGem.size.y; offsetY++)
            {
                Vector3 forwardY = xPos + new Vector3(0, offsetY * cellSize, 0);
                if (allStonePositions.Contains(forwardY))
                {
                    allYPositions.Add(forwardY);
                    Debug.Log($"Added position to allYPositions (Y-axis): {forwardY}"); // Log khi thêm vào
                }
                else
                {
                    // Kiểm tra ngược lại
                    Vector3 backwardY = xPos - new Vector3(0, offsetY * cellSize, 0);
                    if (allStonePositions.Contains(backwardY))
                    {
                        allYPositions.Add(backwardY);
                        Debug.Log($"Added position to allYPositions (Y-axis): {backwardY}"); // Log khi thêm vào
                    }
                    else
                    {
                        isYValid = false;
                        break;
                    }
                }
            }

            if (!isYValid)
            {
                break;
            }
        }

        // Nếu trục Y không hợp lệ, dừng kiểm tra
        if (!isYValid || allYPositions.Count != selectedGem.size.x * selectedGem.size.y)
        {
            Debug.Log("Cannot spawn gem: Invalid positions on Y-axis.");
            return;
        }

        // Tính vị trí trung tâm của gem dựa trên các ô hợp lệ
        Vector3 centerPosition = Vector3.zero;
        foreach (Vector3 pos in allYPositions)
        {
            centerPosition += pos;
        }
        centerPosition /= allYPositions.Count;

        // Snap lại vị trí trung tâm vào grid
        centerPosition = SnapToGrid(centerPosition);
        centerPosition.z = selectedGem.gemPrefab.transform.position.z;
        // Tạo gem tại vị trí trung tâm
        Instantiate(selectedGem.gemPrefab, centerPosition, Quaternion.identity);
        Debug.Log($"Spawned gem: {selectedGem.gemPrefab.name} at {centerPosition}");

        if (selectedGem != null)
        {
            selectedGem.count--;

            if (selectedGem.count <= 0)
            {
                // Đảm bảo index hợp lệ trước khi xóa
                if (randomIndex >= 0 && randomIndex < gemConfigs.Count)
                {
                    gemConfigs.RemoveAt(randomIndex);
                    Debug.Log($"Gem {selectedGem.gemPrefab.name} removed from the list.");
                }
                else
                {
                    Debug.Log("Invalid index for removing gem.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Selected gem is null.");
        }

        // Xóa tất cả các vị trí đã sử dụng
        foreach (Vector3 pos in allYPositions)
        {
            RemoveStonePosition(pos);
        }
    }


}
