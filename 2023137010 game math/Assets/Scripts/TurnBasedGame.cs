using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // UI TextMeshPro를 사용하기 위해 추가

public class TurnBasedGame : MonoBehaviour
{
    [Header("확률 및 능력치 설정")]
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    [Header("UI 텍스트 연결")]
    [SerializeField] TextMeshProUGUI battleResultText; // 전투 결과 텍스트
    [SerializeField] TextMeshProUGUI itemResultText;   // 획득 아이템 텍스트

    // 게임 진행 상태 변수
    int turn = 0;
    bool rareItemObtained = false;
    float currentRareDropChance = 0.00f; // 기본 레어 확률 (0%에서 시작, 턴마다 증가)

    // 통계 기록용 변수들 (UI 출력용)
    int totalEnemiesEncountered = 0;
    int totalEnemiesDefeated = 0;
    int totalAttacksAttempted = 0;
    int totalAttacksHit = 0;
    int totalCrits = 0;
    float maxDamage = 0f;
    float minDamage = float.MaxValue;

    int potionCount = 0;
    int goldCount = 0;
    int normalWeaponCount = 0;
    int rareWeaponCount = 0;
    int normalArmorCount = 0;
    int rareArmorCount = 0;

    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };

    public void StartSimulation()
    {
        // 시뮬레이션 시작 시 모든 통계 및 상태 초기화
        turn = 0;
        rareItemObtained = false;
        currentRareDropChance = 0.05f; // 첫 턴의 레어 확률을 5%로 시작

        totalEnemiesEncountered = 0;
        totalEnemiesDefeated = 0;
        totalAttacksAttempted = 0;
        totalAttacksHit = 0;
        totalCrits = 0;
        maxDamage = 0f;
        minDamage = float.MaxValue;

        potionCount = 0;
        goldCount = 0;
        normalWeaponCount = 0;
        rareWeaponCount = 0;
        normalArmorCount = 0;
        rareArmorCount = 0;

        Debug.Log("=== 시뮬레이션 시작 ===");

        // 레어 아이템이 나올 때까지 무한 반복
        while (!rareItemObtained)
        {
            SimulateTurn();
            turn++;

            if (!rareItemObtained)
            {
                // 턴이 끝날 때마다 레어 아이템 획득 확률 5%씩 상승 (최대 100% 보정)
                currentRareDropChance += 0.05f;
            }
        }

        Debug.Log($"레어 아이템 {turn} 턴에 획득!");

        // 모든 턴이 종료되면 UI 및 콘솔에 최종 결과 출력
        PrintAndUpdateResults();
    }

    void SimulateTurn()
    {
        Debug.Log($"--- Turn {turn + 1} (현재 레어 확률: {currentRareDropChance * 100:F0}%) ---");

        // 푸아송 샘플링: 적 등장 수
        int enemyCount = SamplePoisson(poissonLambda);
        totalEnemiesEncountered += enemyCount;
        Debug.Log($"적 등장 : {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            // 이항 샘플링: 명중 횟수
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);

            // 통계: 총 시도한 공격 횟수와 성공한 명중 횟수 누적
            totalAttacksAttempted += maxHitsPerTurn;
            totalAttacksHit += hits;

            float totalDamage = 0f;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);

                // 베르누이 분포 샘플링: 확률 기반 치명타 발생
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    totalCrits++; // 통계: 치명타 횟수 누적
                    Debug.Log($" 크리티컬 hit! {damage:F1}");
                }
                else
                {
                    Debug.Log($" 일반 hit! {damage:F1}");
                }

                // 통계: 최대/최소 데미지 갱신
                if (damage > maxDamage) maxDamage = damage;
                if (damage < minDamage) minDamage = damage;

                totalDamage += damage;
            }

            if (totalDamage >= enemyHP)
            {
                totalEnemiesDefeated++; // 통계: 처치한 적 누적
                Debug.Log($"적 {i + 1} 처치! (데미지: {totalDamage:F1})");

                // 균등 분포 샘플링: 보상 결정
                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];
                Debug.Log($"보상: {reward}");

                // 보상 카운트 및 레어 아이템 체크 로직
                if (reward == "Potion") potionCount++;
                else if (reward == "Gold") goldCount++;
                else if (reward == "Weapon")
                {
                    if (Random.value < currentRareDropChance)
                    {
                        rareItemObtained = true;
                        rareWeaponCount++;
                        Debug.Log("레어 무기 획득!");
                    }
                    else normalWeaponCount++;
                }
                else if (reward == "Armor")
                {
                    if (Random.value < currentRareDropChance)
                    {
                        rareItemObtained = true;
                        rareArmorCount++;
                        Debug.Log("레어 방어구 획득!");
                    }
                    else normalArmorCount++;
                }
            }
        }
    }

    // 결과 데이터를 정리하여 UI 텍스트와 콘솔창에 출력하는 함수
    void PrintAndUpdateResults()
    {
        // 0으로 나누기 방지를 포함한 퍼센트 계산
        float hitRatePercentage = totalAttacksAttempted > 0 ? ((float)totalAttacksHit / totalAttacksAttempted) * 100f : 0f;
        float critRatePercentage = totalAttacksHit > 0 ? ((float)totalCrits / totalAttacksHit) * 100f : 0f;

        // 공격을 한 번도 못 맞췄을 경우 최소 데미지 예외 처리
        if (minDamage == float.MaxValue) minDamage = 0f;

        // 출력할 문자열 포맷팅
        string battleResult = $"총 진행 턴 수 : {turn}\n" +
                              $"발생한 적 : {totalEnemiesEncountered}\n" +
                              $"처치한 적 : {totalEnemiesDefeated}\n" +
                              $"공격 명중 결과 : {hitRatePercentage:F2}%\n" +
                              $"발생한 치명타율 결과 : {critRatePercentage:F2}%\n" +
                              $"최대 데미지 : {maxDamage:F2}\n" +
                              $"최소 데미지 : {minDamage:F2}";

        string itemResult = $"포션 : {potionCount}개\n" +
                            $"골드 : {goldCount}개\n" +
                            $"무기 - 일반 : {normalWeaponCount}개\n" +
                            $"무기 - 레어 : {rareWeaponCount}개\n" +
                            $"방어구 - 일반 : {normalArmorCount}개\n" +
                            $"방어구 - 레어 : {rareArmorCount}개";

        // 화면 UI Text 갱신
        if (battleResultText != null) battleResultText.text = battleResult;
        if (itemResultText != null) itemResultText.text = itemResult;

        // 콘솔 창에도 한눈에 보이게 출력
        Debug.Log("\n[최종 전투 결과]\n" + battleResult);
        Debug.Log("\n[최종 획득 아이템]\n" + itemResult);
    }

    // --- 분포 샘플 함수들 ---
    int SamplePoisson(float lambda)
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}