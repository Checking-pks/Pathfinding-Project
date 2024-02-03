/// ksPark
///
/// 필드 제작 스크립트

using UnityEngine;

public class FieldMaker : MonoBehaviour
{
    public bool[,] fieldList;
    public GameObject[,] blockList;
    protected string floorName = "Floor";
    protected GameObject floor;
    protected string playerName = "Player";
    protected GameObject player;
    protected GameObject parent;

    protected float m_fieldSizeRow, m_fieldSizeCol;
    protected float m_blockSizeRow, m_blockSizeCol;

    public GameObject wall;
    [Range(10, 100)]
    public int m_row, m_col;
    private int lastRow=0, lastCol=0;
    public bool m_makeField = true;
    public KeyCode m_key = KeyCode.F;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        DisableAnotherFieldMaker();
        SetField();
    }

    private void OnDisable()
    {
        DisableField();
    }

    private void Update()
    {
        if (lastRow != m_row || lastCol != m_col)
        {
            lastRow = m_row; lastCol = m_col;
            m_makeField = true;
        }

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
        floor  = transform.Find(floorName)? .gameObject;
        player = transform.Find(playerName)?.gameObject;

        DataInspection();

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

    private bool DataInspection()
    {
        if (floor == null)
        {
            Debug.LogError($"이 오브젝트의 하위 {floorName}을 찾을 수 없습니다.");
            return false;
        }

        if (player == null)
        {
            Debug.LogError($"이 오브젝트의 하위 {playerName}을 찾을 수 없습니다.");
            return false;
        }

        if (wall == null)
        {
            Debug.LogError($"Wall Prefab을 찾을 수 없습니다.");
            return false;
        }

        return true;
    }

    private void DisableAnotherFieldMaker()
    {
        FieldMaker[] fieldMakers = GetComponents<FieldMaker>();
        for (int i = 0;i < fieldMakers.Length;i++)
        {
            if (fieldMakers[i] != this)
                fieldMakers[i].enabled = false;
        }
    }

    virtual protected void SetField()
    {
        if (!DataInspection()) return;

        // 플레이어 배치
        player.transform.position =
            Vector3.right * ((1 + .5f) * m_blockSizeRow - m_fieldSizeRow) +
            Vector3.forward * ((m_col - 2 + .5f) * m_blockSizeCol - m_fieldSizeCol);

        player.transform.localScale =
            Vector3.right * m_blockSizeRow * .75f +
            Vector3.up +
            Vector3.forward * m_blockSizeCol * .75f;

        // 필드 배치
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

    virtual protected void DisableField()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                blockList[i, j].SetActive(false);
            }
        }
    }
}
