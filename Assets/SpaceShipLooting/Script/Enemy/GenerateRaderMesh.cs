//using UnityEngine;

//[RequireComponent(typeof(Mesh))]
//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]

//public class GenerateRadarMesh : MonoBehaviour
//{
//    [Header("레이더 설정")]
//    [SerializeField] private float radius = 10f; // 반지름
//    [SerializeField][Range(3, 100)] private int segments = 10; // 세그먼트 수 (최소값 3)
//    [SerializeField][Range(10f, 360f)] private float angle = 100f; // 각도
//    [SerializeField][Range(0.01f, 1f)] private float thickness = 0.05f; // 두께 (최소값 0.01)

//    [Header("메쉬 설정")]
//    [SerializeField] private Material radarMaterial; // 레이더 표시용 머티리얼
//    [SerializeField] private bool useCustomNormals = false; // 커스텀 노멀 계산 여부
//    [SerializeField] private bool isTrigger = true; // 콜라이더를 트리거로 설정 여부

//    private Mesh radarMesh;
//    private MeshCollider meshCollider;
//    private MeshFilter meshFilter;

//    private void Start()
//    {
//        // (DetectionType type, float range) = transform.parent.GetComponent<MonsterMovement>().GetRaderInfo();
//        RaderSettingStart(radius, 100f, 0.01f, true);
//    }


//    public void RaderSettingStart(float radius, float angle, float thickness, bool trigger)
//    {
//        this.radius = Mathf.Clamp(radius, 2, 20);
//        this.angle = Mathf.Clamp(angle, 10, 360);
//        this.thickness = Mathf.Clamp(thickness, 0.01f, 1);
//        this.isTrigger = trigger;
//        GenerateMesh();
//    }


//    /// <summary>
//    /// 레이더 메쉬 생성
//    /// </summary>
//    private void GenerateMesh()
//    {
//        radarMesh = GetComponent<Mesh>();
//        if (radarMesh != null) radarMesh.Clear();
//        radarMesh = new Mesh();
//        radarMesh.name = "Arc Radar Mesh";

//        meshFilter = GetComponent<MeshFilter>();
//        if (meshFilter != null && radarMesh != null)
//        {
//            meshFilter.sharedMesh = radarMesh;
//        }
//        if (radarMaterial != null)
//        {
//            GetComponent<MeshRenderer>().material = radarMaterial;
//        }
//        else
//        {
//            Debug.LogWarning("메터리얼이 없습니다. 메터리얼을 추가해주세요.");
//            GetComponent<MeshRenderer>().enabled = false;
//        }

//        int vertexCount = (segments + 1) * 2 + 2; // 두께를 위해 두 면의 버텍스를 포함
//        int triangleCount = segments * 12; // 앞면, 뒷면, 두께 연결 면을 위한 삼각형 개수
//        Vector3[] vertices = new Vector3[vertexCount];
//        int[] triangles = new int[triangleCount];

//        float angleStep = angle / segments;

//        // 중심 버텍스 추가 (앞면과 뒷면)
//        vertices[0] = new Vector3(0, thickness / 2, 0); // 앞면 중심
//        vertices[vertexCount / 2] = new Vector3(0, -thickness / 2, 0); // 뒷면 중심

//        // 각 버텍스를 앞면과 뒷면에 추가
//        for (int i = 0; i <= segments; i++)
//        {
//            float currentAngle = -angle / 2 + i * angleStep;
//            float radian = Mathf.Deg2Rad * currentAngle;

//            // 앞면 버텍스
//            vertices[i + 1] = new Vector3(Mathf.Sin(radian), thickness / 2, Mathf.Cos(radian)) * radius;
//            // 뒷면 버텍스
//            vertices[i + 1 + vertexCount / 2] = new Vector3(Mathf.Sin(radian), -thickness / 2, Mathf.Cos(radian)) * radius;
//        }

//        // 삼각형 구성 (앞면, 뒷면, 두께 연결)
//        int triIndex = 0;

//        // 앞면 삼각형 (반시계 방향)
//        for (int i = 0; i < segments; i++)
//        {
//            triangles[triIndex++] = 0;
//            triangles[triIndex++] = i + 1;
//            triangles[triIndex++] = i + 2;
//        }

//        // 뒷면 삼각형 (반대 방향으로 정의, 시계 방향)
//        for (int i = 0; i < segments; i++)
//        {
//            triangles[triIndex++] = vertexCount / 2;
//            triangles[triIndex++] = i + 2 + vertexCount / 2;
//            triangles[triIndex++] = i + 1 + vertexCount / 2;
//        }

//        // 두께 연결 삼각형
//        for (int i = 0; i < segments; i++)
//        {
//            int start1 = i + 1;
//            int start2 = i + 1 + vertexCount / 2;

//            triangles[triIndex++] = start1;
//            triangles[triIndex++] = start2;
//            triangles[triIndex++] = start1 + 1;

//            triangles[triIndex++] = start2;
//            triangles[triIndex++] = start2 + 1;
//            triangles[triIndex++] = start1 + 1;
//        }

//        // 메쉬 설정
//        radarMesh.vertices = vertices;
//        radarMesh.triangles = triangles;

//        if (useCustomNormals)
//        {
//            ApplyCustomNormals(radarMesh);
//        }
//        else
//        {
//            radarMesh.RecalculateNormals();
//        }

//        radarMesh.RecalculateBounds();

//        // 콜라이더 추가
//        AddCollider();
//    }

//    /// <summary>
//    /// 커스텀 노멀 적용
//    /// </summary>
//    /// <param name="mesh">노멀을 적용할 메쉬</param>
//    private void ApplyCustomNormals(Mesh mesh)
//    {
//        Vector3[] normals = mesh.normals;
//        for (int i = 0; i < normals.Length; i++)
//        {
//            normals[i] = Vector3.Normalize(normals[i]);
//        }
//        mesh.normals = normals;
//    }

//    /// <summary>
//    /// 메쉬 콜라이더 추가 및 설정
//    /// </summary>
//    private void AddCollider()
//    {
//        meshCollider = GetComponent<MeshCollider>();
//        if (meshCollider == null)
//            meshCollider = gameObject.AddComponent<MeshCollider>();

//        meshCollider.sharedMesh = null; // 기존 메쉬 초기화
//        meshCollider.sharedMesh = radarMesh; // 새로 생성된 메쉬 설정
//        meshCollider.convex = true;
//        meshCollider.isTrigger = isTrigger;

//    }

//    //private void OnDrawGizmos()
//    //{
//    //    Gizmos.color = Color.yellow; // 외곽선 색상

//    //    if (segments < 3 || radius <= 0 || angle <= 0) return;

//    //    // 외곽선 그리기
//    //    float angleStep = angle / segments;
//    //    Vector3 prevPoint = Vector3.zero;

//    //    for (int i = 0; i <= segments; i++)
//    //    {
//    //        // 현재 각도 계산
//    //        float currentAngle = -angle / 2 + i * angleStep;
//    //        float radian = Mathf.Deg2Rad * currentAngle;

//    //        // 현재 점 계산
//    //        Vector3 currentPoint = transform.position + new Vector3(
//    //            Mathf.Sin(radian) * radius,
//    //            0,
//    //            Mathf.Cos(radian) * radius
//    //        );

//    //        // 이전 점과 현재 점을 연결
//    //        if (i > 0)
//    //        {
//    //            Gizmos.DrawLine(prevPoint, currentPoint);
//    //        }

//    //        prevPoint = currentPoint;
//    //    }

//    //    // 두께를 표시하기 위해 외곽선 끝부분 연결
//    //    Vector3 startPoint = transform.position + new Vector3(
//    //        Mathf.Sin(-angle / 2 * Mathf.Deg2Rad) * radius,
//    //        0,
//    //        Mathf.Cos(-angle / 2 * Mathf.Deg2Rad) * radius
//    //    );

//    //    Vector3 endPoint = transform.position + new Vector3(
//    //        Mathf.Sin(angle / 2 * Mathf.Deg2Rad) * radius,
//    //        0,
//    //        Mathf.Cos(angle / 2 * Mathf.Deg2Rad) * radius
//    //    );

//    //    Gizmos.DrawLine(startPoint, endPoint);
//    //}

//}
