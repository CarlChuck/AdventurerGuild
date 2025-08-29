using System;
using UnityEngine;

/// <summary>
/// Component that provides unique identification for GameObjects in the save system
/// </summary>
public class UniqueId : MonoBehaviour
{
    [SerializeField] private string id = "";
    
    public string Id 
    { 
        get 
        {
            if (string.IsNullOrEmpty(id))
            {
                GenerateNewId();
            }
            return id;
        }
        private set 
        {
            id = value;
        }
    }
    
    private void Awake()
    {
        // Generate ID if not already set
        if (string.IsNullOrEmpty(id))
        {
            GenerateNewId();
        }
    }
    
    private void GenerateNewId()
    {
        // Generate unique ID using GUID and timestamp
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        string guid = Guid.NewGuid().ToString("N")[..8]; // First 8 characters of GUID
        Id = $"{timestamp}_{guid}";
    }
    
    /// <summary>
    /// Force generation of a new unique ID (useful for duplicated objects)
    /// </summary>
    public void RegenerateId()
    {
        GenerateNewId();
    }
    
    /// <summary>
    /// Set a specific ID (used during save loading)
    /// </summary>
    public void SetId(string newId)
    {
        if (!string.IsNullOrEmpty(newId))
        {
            Id = newId;
        }
    }
}