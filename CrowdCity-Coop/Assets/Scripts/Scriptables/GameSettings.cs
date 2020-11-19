using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public Vector2 xSize, zSize;
        
        public int maxAmountOfPlayers;
        public GameObject playerPrefab, aiPrefab, npcPrefab;
        public List<Color> leaderColors;
        
        public int startAmountOfNpc, maxAmountOfNpc;
        public int followersForCapturing;
        
        [NonSerialized] public PlayerSetting[] players;
        [NonSerialized] public ColorBool[] ColorTable;
        
        public ColorBool FirstAvailableColorBool => ColorTable.First(x => !x.taken);

        public ColorBool RandomColorTable()
        {
            ColorBool[] available = ColorTable.Where(x => !x.taken).ToArray();

            return available[Random.Range(0, available.Length)];
        }

        public int NextAvailableIndex(int i)
        {
            int index = i;
            while (true)
            {
                if (index > ColorTable.Length - 1)
                    index = 0;
                
                if (ColorTable[index].taken)
                {
                    index++;
                    continue;
                }

                return index;
            }
        }
        
        public int PrevAvailableIndex(int i)
        {
            int index = i;
            
            while (true)
            {
                if (index < 0)
                    index = ColorTable.Length - 1;
                
                if (ColorTable[index].taken)
                {
                    index--;
                    continue;
                }

                return index;
            }
        }

        public void InitColorTable()
        {
            ColorTable = new ColorBool[leaderColors.Count];

            for (var i = 0; i < leaderColors.Count; i++)
            {
                ColorTable[i] = new ColorBool(leaderColors[i]);
            }
        }
    }

    [System.Serializable]
    public class ColorBool
    {
        public Color color;
        public bool taken;

        public ColorBool(Color c)
        {
            color = c;
            taken = false;
        }
    }

    public class PlayerSetting
    {
        public Color Color;
    }
}