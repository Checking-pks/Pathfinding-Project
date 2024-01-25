/// ksPark
///
/// 랜덤 필드 제작

using UnityEngine;

public class RandomFieldMaker : FieldMaker
{
    protected override void Init()
    {
        parent = new GameObject(GetType().Name);
        parent.transform.parent = transform;
        base.Init();
    }

    override protected void FieldMake()
    {
        if (floor == null)
        {
            Debug.LogError($"{floorName}을 찾을 수 없습니다.");
            return;
        }

        m_fieldSizeRow = floor.transform.localScale.x * 5.0f;
        m_fieldSizeCol = floor.transform.localScale.z * 5.0f;
        m_blockSizeRow = floor.transform.localScale.x / m_row * 10f;
        m_blockSizeCol = floor.transform.localScale.z / m_col * 10f;

        
        for (int i=0; i<m_row; i++)
        {
            for (int j=0; j<m_col; j++)
            {
                fieldList[i, j] = Random.Range(0, 2) == 1;
                if (i == 0 || i == m_row - 1) fieldList[i, j] = true;
                if (j == 0 || j == m_col - 1) fieldList[i, j] = true;
                if (i == 1 && j == m_col - 2) fieldList[i, j] = false;
            }
        }
    }
}
