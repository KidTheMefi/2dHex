using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public class LandGeneration : ILandGeneration
{
    private IHexStorage _hexStorage;
    
    public LandGeneration(IHexStorage hexStorage)
    {
        _hexStorage = hexStorage;
    }

    #region LandGeneration

    public List<Vector2Int> CreateContinentLand(Vector2Int center, int startScale, int minTilesNumber, List<Vector2Int> unavailableHexes = null)
     {
         List<Vector2Int> continentTilesCoordinate = CreateContinentPart(center, startScale);

         int cycleCount = 0;
         int maxCycleCount = minTilesNumber / 5;
         while (continentTilesCoordinate.Count<minTilesNumber)
         {
             var continentAdd = CreateContinentPart(continentTilesCoordinate[Random.Range(0, continentTilesCoordinate.Count)], 2);

             if (cycleCount == maxCycleCount)
             {
                 break;
             }
             
             foreach (var axial in continentAdd)
             {
                 if (unavailableHexes == null)
                 {
                     if (!continentTilesCoordinate.Contains(axial))
                     {
                         continentTilesCoordinate.Add(axial);
                     }
                 }
                 else
                 {
                     if (!continentTilesCoordinate.Contains(axial) && !unavailableHexes.Contains(axial))
                     {
                         continentTilesCoordinate.Add(axial);
                     }
                 }
                 
                 if (continentTilesCoordinate.Count == minTilesNumber)
                 {
                     break;
                 }
             }
             cycleCount++;
         }
         return continentTilesCoordinate;
     }


     private List<Vector2Int> CreateContinentPart(Vector2Int center, int startScale)
    {
        List<Vector2Int> continentTilesCoordinate =  new List<Vector2Int>();

        for (int i = 0; i < 8; i++) // magic . TODO: smth with that
        {
            Vector2Int nextCenter = RandomAxialAtRadius(center, Random.Range(startScale, startScale+3));
            int nextScaleMin = HexUtils.AxialDistance(center, nextCenter) - startScale+1;
            int nextScale = Random.Range(nextScaleMin, nextScaleMin + 1);

            foreach (var axial in CreatePieceOffLand(nextCenter,nextScale))
            {
                if (!continentTilesCoordinate.Contains(axial)&& _hexStorage.HexAtAxialCoordinateExist(axial))
                {
                    continentTilesCoordinate.Add(axial);
                }
            }
        }
        return continentTilesCoordinate;
    }
     
     private Vector2Int RandomAxialAtRadius(Vector2Int center, int radius)
     {
         List<Vector2Int> axialAtRadius = new List<Vector2Int>();
         foreach (var axial in HexUtils.GetAxialRingWithRadius(center, radius))
         {
             if (_hexStorage.HexAtAxialCoordinateExist(axial))
             {
                 axialAtRadius.Add(axial);
             }
         }

         return axialAtRadius[Random.Range(0, axialAtRadius.Count)];
     }

    private List<Vector2Int> CreatePieceOffLand(Vector2Int center, int startScale)
    {
        List<Vector2Int> landCoordinates = HexUtils.GetAxialAreaAtRange(center, startScale);
        
        foreach (var axial in HexUtils.GetAxialAreaAtRange(RandomAxialAtRadius(center, startScale), startScale - 1))
        {
            landCoordinates.Remove(axial);
        }
        return landCoordinates;
    }
    
    public List<Vector2Int> CreateLandTypeAtContinent(List<Vector2Int> continent, int minTilesNumber)
    {
        //not single responsibility !!!!
        List<Vector2Int> continentNewLandType = new List<Vector2Int>();
        while (continentNewLandType.Count<minTilesNumber)
        {
            foreach (var axial in CreatePieceOfLandType(continent[Random.Range(0, continent.Count)]))
            {
                if (continent.Contains(axial))
                {
                    continentNewLandType.Add(axial);
                    continent.Remove(axial); // look at this dude!
                }
                
                if (continentNewLandType.Count == minTilesNumber)
                {
                    break;
                }
            }
        }
        return continentNewLandType;
    }
    
    private List<Vector2Int> CreatePieceOfLandType(Vector2Int center)
    {
        List<Vector2Int> landCoordinates = HexUtils.GetAxialAreaAtRange(center, 1);
   
        for (int i = 0; i < 3; i++)
        {
            landCoordinates.RemoveAt(Random.Range(0,landCoordinates.Count));
        }
        return landCoordinates;
    }

  #endregion
}
