using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class HexMapSaved
    {
        [SerializeField]
        private Vector2Int _mapResolution;
        [SerializeField]
        private List<Hex> _hexes;
        
        public List<Hex> Hexes => _hexes;
        public Vector2Int MapResolution=> _mapResolution;
        
        [SerializeField]
        private List<Continent> _allContinents;
        public List<Continent> AllContinents => _allContinents;
        
        
        public void SaveMap(Hex[,] hexMap, Vector2Int mapResolution)
        {
            _mapResolution = mapResolution;
            Debug.Log("Saved Map");
            
            _hexes = new List<Hex>();
            foreach (var hex in hexMap)
            {
                _hexes.Add(hex);
            }
        }

        public void SaveContinents(List<Continent> continents)
        {
            Debug.Log("Saved Continents");
            _allContinents = continents;
        }

        public static HexMapSaved GetSaveFromJson()
        {
            string json = File.ReadAllText(Application.dataPath + "/Save/SavedMap.json");
            HexMapSaved hexMapSaved = JsonUtility.FromJson<HexMapSaved>(json);
            return hexMapSaved;
        }

        public static void SaveToJson(HexMapSaved hexMapSaved)
        {
            foreach (var continent in hexMapSaved._allContinents)
            {
                Debug.Log(continent.BiomType.ToString());
            }

            string json = JsonUtility.ToJson(hexMapSaved, true);
            File.WriteAllText(Application.dataPath + "/Save/SavedMap.json", json);
        }
    }
}