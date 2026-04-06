using UnityEngine;

public class MyTask_CritSystem : MonoBehaviour
{
    public int totalHits = 0;
    public int critHits = 0;
    public float targetRate = 0.3f; // 30%

    public bool RollCrit()
    {
        totalHits++;

        // 현재까지의 실제 치명타 확률 계산 (이번 공격 직전까지의 데이터)
        float currentRate = (totalHits <= 1) ? 0f : (float)critHits / (totalHits - 1);

        // [보정 로직 1] 확률이 너무 낮으면 (목표의 0.8배 미만) 강제로 발생
        if (totalHits > 5 && currentRate < targetRate * 0.8f)
        {
            critHits++;
            Debug.Log("치명타 강제 발생 (보정)");
            return true;
        }

        // [보정 로직 2] 확률이 너무 높으면 (목표의 1.2배 초과) 강제로 일반 공격
        if (totalHits > 5 && currentRate > targetRate * 1.2f)
        {
            Debug.Log("일반 공격 강제 발생 (보정)");
            return false;
        }

        // [기본 로직] 랜덤 주사위
        if (Random.value < targetRate)
        {
            critHits++;
            return true;
        }

        return false;
    }
}