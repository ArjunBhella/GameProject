using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("SCENE INDEX")]
        public int sceneIndex = 1;


        [Header("Character Name")]
        public string characterName = "Character";


        [Header("Time Played")]
        public float secondsPlayed;


        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;


        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;


        [Header("State")]
        public int vitalty;
        public int endurance;


        [Header("Bosses")]
        public SerializableDictionary<int, bool> bossesAwakened; // The int is the bodd ID, the bool is the Awakened Status
        public SerializableDictionary<int, bool> bossesDefeated; // The int is the bodd ID, the bool is the Awakened Status


        public CharacterSaveData()
        {
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
        }




    }
}
