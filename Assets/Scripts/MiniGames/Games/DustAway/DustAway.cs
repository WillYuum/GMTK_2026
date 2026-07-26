using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DustAway : MiniGame
{
    [Header("Dust")]
    [SerializeField] private GameObject dustPrefab;
    [SerializeField] private int dustCount = 10;
    [SerializeField] private Transform _spawnedDustHolder;
    [SerializeField] private Vector2 spawnPadding = new(0.5f, 0.5f);

    private readonly List<GameObject> spawnedDust = new();

    public override void OnInitialize()
    {
        SpawnDust();
    }

    public override void OnStart()
    {
    }

    public override void OnUpdate()
    {
        Mouse currentMouse = Mouse.current;
        if (!currentMouse.leftButton.isPressed)
        {
            return;
        }

        Vector2 mouse = Camera.main.ScreenToWorldPoint(currentMouse.position.ReadValue());

        RaycastHit2D[] hits = Physics2D.CircleCastAll(mouse, 0.1f, Vector2.zero, 0f);
        if (hits.Length == 0)
        {
            return;
        }

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out Dust dust))
            {
                GameObject dustGO = hit.collider.gameObject;
                AudioManager.Instance.PlaySFX("Dust");

                spawnedDust.Remove(dustGO);
                Destroy(dustGO);
            }
        }

        if (spawnedDust.Count == 0)
        {
            TriggerFinishedGame(true);
        }
    }

    public override void OnEnd()
    {
        foreach (var dust in spawnedDust)
        {
            if (dust != null)
            {
                Destroy(dust);
            }
        }

        spawnedDust.Clear();
    }

    private void SpawnDust()
    {
        spawnedDust.Clear();

        float minX = BackPanelRect.xMin + spawnPadding.x;
        float maxX = BackPanelRect.xMax - spawnPadding.x;
        float minY = BackPanelRect.yMin + spawnPadding.y;
        float maxY = BackPanelRect.yMax - spawnPadding.y;

        for (int i = 0; i < dustCount; i++)
        {
            Vector3 localPos = new(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                _spawnedDustHolder.position.z);

            GameObject dust = Instantiate(dustPrefab);

            spawnedDust.Add(dust);

            dust.transform.SetParent(_spawnedDustHolder, false);
            dust.transform.position = localPos;
        }
    }
}