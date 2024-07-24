using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // process instant effect (take damage, heal)

        // process Timed effect (poison , build ups)

        // process static effect (adding / removing buffs form talismans ect)

        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }

    }
}
