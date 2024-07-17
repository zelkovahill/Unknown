using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace SG
{
    public class CharacterStatsManager : MonoBehaviour
    {
        private CharacterManager characterManager;

        [Header("Stmaina Regeneration")]
        [SerializeField] private float staminaRegenerationAmount = 2;
        private float staminaRegenTimer = 0;
        private float staminaTickTimer = 0;
        [SerializeField] private float staminaRegenerationDelay = 2f;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }


        public virtual void RegenerateStamina()
        {
            if (!characterManager.IsOwner)
            {
                return;
            }

            if (characterManager.characterNetworkManager.isSprinting.Value)
            {
                return;
            }

            if (characterManager.isPerformingAction)
            {
                return;
            }

            staminaRegenTimer += Time.deltaTime;

            if (staminaRegenTimer >= staminaRegenerationDelay)
            {
                if (characterManager.characterNetworkManager.currentStamina.Value < characterManager.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1f)
                    {
                        staminaTickTimer = 0;
                        characterManager.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                    }
                }

            }

        }


        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);

        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {

            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenTimer = staminaRegenerationDelay;
            }

        }

    }

}


