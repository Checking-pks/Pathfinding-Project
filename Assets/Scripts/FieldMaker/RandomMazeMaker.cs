/// ksPark
///
/// 랜덤 미로 제작

using UnityEngine;
using System.Collections.Generic;

public class RandomMazeMaker : FieldMaker
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

        for (int i = 0; i < m_row; i++)
            for (int j = 0; j < m_col; j++)
                fieldList[i, j] = true;

        (int, int)[] moveList = { (2, 0), (-2, 0), (0, 2), (0, -2) };
        Stack<(int, int)> bucket = new Stack<(int, int)>();
        bucket.Push((1, m_col - 2));

        while (bucket.Count > 0)
        {
            var now = bucket.Peek();
            List<(int, int)> nextList = new List<(int, int)>();

            for (int i = 0; i < moveList.Length; i++)
            {
                var nextPos = (now.Item1 + moveList[i].Item1, now.Item2 + moveList[i].Item2);

                if (nextPos.Item1 <= 0 || nextPos.Item1 >= m_row - 1) continue;
                if (nextPos.Item2 <= 0 || nextPos.Item2 >= m_col - 1) continue;
                if (fieldList[nextPos.Item1, nextPos.Item2] == false) continue;

                nextList.Add(nextPos);
            }

            if (nextList.Count == 0)
            {
                bucket.Pop();
                continue;
            }

            var next = nextList[Random.Range(0, nextList.Count)];
            fieldList[(now.Item1 + next.Item1) / 2, (now.Item2 + next.Item2) / 2] = false;
            fieldList[next.Item1, next.Item2] = false;

            bucket.Push(next);
        }
    }
}