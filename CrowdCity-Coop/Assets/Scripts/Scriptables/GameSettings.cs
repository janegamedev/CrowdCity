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
        public List<Skin> leaderSkins;
        public List<string> leaderNames = new List<string>()
        {
            "Basher", "Cannonball", "Snowflake", "Jelly", "Genius", "Izzy", "Wheels", "Stretch", "Bird", "Biggie","Dynamite", "Starfall", "Jumbo", "Beast", "Frosty", "Storm", "Shark", "Ducky", "Bigshot", "Cobra"
        };
        
        public int startAmountOfNpc, maxAmountOfNpc;
        public int followersForCapturing;
        
        [NonSerialized] public List<PlayerConfiguration> players = new List<PlayerConfiguration>();
        [NonSerialized] public List<CustomItem<string>> nicknameItems;
        [NonSerialized] public List<CustomItem<Skin>> skinItems;
        
        public CustomItem<Skin> FirstAvailableColorItem => skinItems.First(x => !x.taken);
        public CustomItem<string> FirstAvailableNicknameItem => nicknameItems.First(x => !x.taken);

        public CustomItem<Skin> RandomSkinTable()
        {
            CustomItem<Skin>[] available = skinItems.Where(x => !x.taken).ToArray();

            if (available.Length == 0)
                return null;
            
            return available[Random.Range(0, available.Length)];
        }
        
        public CustomItem<string> RandomNicknameTable()
        {
            CustomItem<string>[] available = nicknameItems.Where(x => !x.taken).ToArray();

            if (available.Length == 0)
                return null;
            
            return available[Random.Range(0, available.Length)];
        }
        
        public void InitData()
        {
            skinItems = new List<CustomItem<Skin>>();

            foreach (Skin leaderColor in leaderSkins)
            {
                skinItems.Add(new CustomItem<Skin>(leaderColor));
            }
            
            nicknameItems = new List<CustomItem<string>>();
            
            foreach (string leaderName in leaderNames)
            {
                nicknameItems.Add(new CustomItem<string>(leaderName));
            }
        }

        public int NextAvailableIndexSkin(int i)
        {
            return NextIndex(skinItems, i);
        }

        public int NextAvailableIndexNickname(int i)
        {
            return NextIndex(nicknameItems, i);
        }
        
        private int NextIndex<T>(List<CustomItem<T>> list, int i)
        {
            int index = i;

            while (true)
            {
                if (index > list.Count - 1)
                    index = 0;
                
                if (list[index].taken)
                {
                    index++;
                    continue;
                }

                return index;
            }
        }
        
        public int PrevAvailableIndexColor(int i)
        {
            return PreviousIndex(skinItems, i);
        }
        
        public int PrevAvailableIndexNickname(int i)
        {
            return PreviousIndex(nicknameItems, i);
        }

        private int PreviousIndex<T>(List<CustomItem<T>> list, int i)
        {
            int index = i;
            
            while (true)
            {
                if (index < 0)
                    index = list.Count - 1;
                
                if (list[index].taken)
                {
                    index--;
                    continue;
                }

                return index;
            }
        }
    }

    [System.Serializable]
    public class CustomItem<T>
    {
        public T value;
        public bool taken;

        public CustomItem(T v)
        {
            value = v;
            taken = false;
        }
    }
}