using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dtp11_MUD_ava
{
    /// <summary>
    /// class LockKey represents a key with which you can
    /// open a door between two rooms
    /// </summary>
    public class LockKey
    {
        public int[] adjRooms = new int[2];
        /// <summary>
        /// Constructor, ascertains that adjRooms come in ascending order
        /// </summary>
        /// <param name="room1">lesser room</param>
        /// <param name="room2">greater room number</param>
        public LockKey(int room1, int room2)
        {
            if (room1 < room2)
            {
                adjRooms[0] = room1;
                adjRooms[1] = room2;
            }
            else
            {
                adjRooms[0] = room2;
                adjRooms[1] = room1;
            }
        }
        public bool Opens(int room1, int room2)
        {
            if (room1 < room2)
            {
                return room1 == adjRooms[0] && room2 == adjRooms[1];
            }
            else
            {
                return room1 == adjRooms[1] && room2 == adjRooms[0];
            }
        }
    }
    public class Room
    {
        // Constants and static members:
        public const int North = 0;
        public const int East = 1;
        public const int South = 2;
        public const int West = 3;
        public const int NoDoor = -1;
        public static string CardinalDirection(int dir)
        {
            switch (dir)
            {
                case 0: return "norr";
                case 1: return "öster";
                case 2: return "söder";
                case 3: return "väster";
                default: return "okänd riktning";
            }
        }

        // Object attributes:
        public int number;
        public string roomname = "";
        public string story = "";
        public List<string> imageFile = new List<string>();
        public int[] adjacent = new int[4]; // adjacent[Room.North] etc.
        public bool[] isOpen = new bool[4]; // isOpen[Room.North] etc.

        // Stuff in this room:
        public List<LockKey> keys = new List<LockKey>();
        public Room(int num, string name)
        {
            number = num; roomname = name;
        }
        public void SetStory(string theStory)
        {
            story = theStory;
        }
        public void SetImage(string theImage)
        {
            imageFile.Add(theImage);
        }
        private void setNextRoom(int direction, int roomNumber)
        {
            if (roomNumber < 0)
            {
                adjacent[direction] = -roomNumber;
                isOpen[direction] = false;
            }
            else
            {
                adjacent[direction] = roomNumber;
                isOpen[direction] = true;
            }
        }
        public void SetDirections(int N, int E, int S, int W)
        {
            setNextRoom(North, N);
            setNextRoom(East, E);
            setNextRoom(South, S);
            setNextRoom(West, W);
        }
        private int GetNextRoom(int direction)
        {
            int N = adjacent[direction];
            // Refuse if there is no such door:
            if (N == Room.NoDoor) return number;
            if (!isOpen[direction]) return number;
            return adjacent[direction];
        }
        public int GetNorth() => GetNextRoom(North);
        public int GetEast() => GetNextRoom(East);
        public int GetSouth() => GetNextRoom(South);
        public int GetWest() => GetNextRoom(West);
        public void HasKey(int room1, int room2)
        {
            keys.Add(new LockKey(room1, room2));
        }
        public List<LockKey> getKeys(List<LockKey> otherKeys)
        {
            foreach (LockKey lc in keys)
            {
                otherKeys.Add(lc);
            }
            return otherKeys;
        }
        public void Unlock(List<LockKey> keys)
        {
            foreach (LockKey lc in keys)
            {
                for (int i = North; i <= West; i++)
                {
                    if (lc.Opens(number, adjacent[i]))
                    {
                        isOpen[i] = true;
                    }
                }
            }
        }
    }
}
