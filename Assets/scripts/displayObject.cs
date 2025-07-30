using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayObject : MonoBehaviour
{
    // 수정된 스크립터블 오브젝트 타입으로 변경
    public stackobject stackedPrefabObject;
    public Vector3 rotation;
    public Vector3 offset = new Vector3(0, 0.05f, 0);
    public int orderInLayer = 0;

    public List<GameObject> partList; // 생성된 게임 오브젝트들을 저장

    void GenerateStack()
    {
        // "Parts" 부모 오브젝트는 동일하게 유지
        GameObject parts = new GameObject("Parts");
        parts.transform.parent = transform;
        parts.transform.localPosition = Vector3.zero;

        // 수정: 애니메이션이 적용된 프리팹을 인스턴스화
        for (int i = 0; i < stackedPrefabObject.stack.Count; i++)
        {
            // 프리팹을 인스턴스화하고 부모를 "Parts"로 설정
            GameObject stackPart = Instantiate(stackedPrefabObject.stack[i], parts.transform);
            stackPart.name = "AnimatedPart_" + i; // 이름 설정
            stackPart.transform.localPosition = Vector3.zero; // 초기 위치는 부모 기준 (0,0,0)

            // 인스턴스화된 프리팹에 SpriteRenderer가 있는지 확인 (필수)
            SpriteRenderer sp = stackPart.GetComponent<SpriteRenderer>();
            if (sp == null)
            {
                Debug.LogWarning($"Animated part {i} ({stackPart.name}) does not have a SpriteRenderer component. This might cause issues with sorting order.");
            }

            partList.Add(stackPart);
        }
    }

    void Start()
    {
        // partList 초기화
        if (partList == null)
        {
            partList = new List<GameObject>();
        }
        GenerateStack();
    }

    void DrawStack() // 메서드 이름 변경 (선택 사항)
    {
        int currentSortingOrder = orderInLayer; // 현재 Sorting Order 추적
        Vector3 currentOffsetPosition = Vector3.zero; // 현재 위치 오프셋 추적

        foreach (GameObject part in partList)
        {
            // 각 파트의 로컬 위치와 회전 설정
            part.transform.localPosition = currentOffsetPosition;
            part.transform.localRotation = Quaternion.Euler(rotation);

            // SpriteRenderer가 있는 경우에만 Sorting Order 적용
            SpriteRenderer sp = part.GetComponent<SpriteRenderer>();
            if (sp != null)
            {
                sp.sortingOrder = currentSortingOrder;
            }

            // 다음 파트를 위한 값 업데이트
            currentOffsetPosition += offset;
            currentSortingOrder += 1;
        }
    }

    void Update()
    {
        DrawStack(); // DrawStack 호출
    }
}