/// ksPark
///
/// 필드 제작 스크립트

using UnityEngine;

public class FieldMaker : MonoBehaviour
{
    protected bool[,] fieldList;
    protected GameObject[,] blockList;
    protected string floorName = "Floor";
    protected GameObject floor;
    protected GameObject parent;

    protected float m_fieldSizeRow, m_fieldSizeCol;
    protected float m_blockSizeRow, m_blockSizeCol;

    public GameObject wall;
    [Range(10, 100)]
    public int m_row, m_col;
    public bool m_makeField = true;
    public KeyCode m_key = KeyCode.F;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (Input.GetKey(m_key))
            m_makeField = true;

        if (m_makeField)
        {
            FieldMake();
            SetField();
            m_makeField = false;
        }
    }

    virtual protected void Init()
    {
        floor = transform.Find(floorName)?.gameObject;

        if (floor == null)
        {
            Debug.LogError($"이 오브젝트의 하위 {floorName}을 찾을 수 없습니다.");
            return;
        }

        if (wall == null)
        {
            Debug.LogError($"Wall Prefab을 찾을 수 없습니다.");
            return;
        }

        fieldList = new bool[100, 100];
        blockList = new GameObject[100, 100];
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                blockList[i, j] = Instantiate(wall, parent.transform);
                blockList[i, j].SetActive(false);
            }
        }
    }

    virtual protected void FieldMake() { }

    virtual protected void SetField()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                if (i >= m_row || j >= m_col || !fieldList[i, j])
                {
                    blockList[i, j].SetActive(false);
                    continue;
                }

                blockList[i, j].SetActive(fieldList[i, j]);
                blockList[i, j].transform.position =
                    Vector3.right * ((i + .5f) * m_blockSizeRow - m_fieldSizeRow) +
                    Vector3.forward * ((j + .5f) * m_blockSizeCol - m_fieldSizeCol);
                blockList[i, j].transform.localScale =
                    Vector3.right * m_blockSizeRow +
                    Vector3.up +
                    Vector3.forward * m_blockSizeCol;
            }
        }
    }
}
