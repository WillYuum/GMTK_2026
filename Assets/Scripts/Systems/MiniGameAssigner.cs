using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class MiniGameAssigner : MonoBehaviour
{
    [Serializable]
    private class MiniGameEntry
    {
        public MiniGamePanel Panel;
        public List<GameObject> MiniGames = new();
    }

    [SerializeField] private List<MiniGameEntry> _entries = new();

    private readonly Dictionary<MiniGamePanel, List<GameObject>> _miniGames = new();

    private void Awake()
    {
        ValidateEntries();
        BuildDictionary();
    }

    public MiniGame RequestMiniGameFromPanel(MiniGamePanel panel)
    {
        if (panel == null)
        {
            throw new ArgumentNullException(nameof(panel), "MiniGameAssigner: Panel is null.");
        }

        if (!_miniGames.TryGetValue(panel, out List<GameObject> miniGames))
        {
            throw new InvalidOperationException($"MiniGameAssigner: Panel '{panel.name}' is not registered.");
        }

        int index = miniGames.Count == 1 ? 0 : UnityEngine.Random.Range(0, miniGames.Count);
        GameObject prefab = miniGames[index];

        return Instantiate(prefab).GetComponent<MiniGame>();
    }

    private void ValidateEntries()
    {
        HashSet<MiniGamePanel> registeredPanels = new();

        foreach (MiniGameEntry entry in _entries)
        {
            if (entry == null)
            {
                throw new InvalidOperationException("MiniGameAssigner: A MiniGameEntry is null.");
            }

            if (entry.Panel == null)
            {
                throw new InvalidOperationException("MiniGameAssigner: A MiniGamePanel is not assigned.");
            }

            if (!registeredPanels.Add(entry.Panel))
            {
                throw new InvalidOperationException($"MiniGameAssigner: Panel '{entry.Panel.name}' is registered more than once.");
            }

            if (entry.MiniGames == null)
            {
                throw new InvalidOperationException($"MiniGameAssigner: Panel '{entry.Panel.name}' has a null MiniGame list.");
            }

            if (entry.MiniGames.Count == 0)
            {
                throw new InvalidOperationException(
                    $"MiniGameAssigner: Panel '{entry.Panel.name}' has no MiniGames assigned.");
            }

            // foreach (GameObject prefab in entry.MiniGames)
            // {
            //     if (prefab == null)
            //     {
            //         throw new InvalidOperationException(
            //             $"MiniGameAssigner: Panel '{entry.Panel.name}' contains a null prefab.");
            //     }

            //     if (!prefab.TryGetComponent<MiniGame>(out _))
            //     {
            //         throw new MissingComponentException(
            //             $"MiniGameAssigner: Prefab '{prefab.name}' does not contain a MiniGame component.");
            //     }
            // }
        }
    }

    private void BuildDictionary()
    {
        _miniGames.Clear();

        foreach (MiniGameEntry entry in _entries)
        {
            _miniGames.Add(entry.Panel, entry.MiniGames);
        }
    }
}