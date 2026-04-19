/*
 * Notion Data (0.x)
 * Copyright (c) Carter Games
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version. 
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. 
 *
 * You should have received a copy of the GNU General Public License along with this program.
 * If not, see <https://www.gnu.org/licenses/>. 
 */

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Handles basic validation for the secret keys used in Notion API calls.
    /// </summary>
    public static class NotionSecretKeyValidator
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string SecretAPIKeyPrefix = "secret_";
        private const string NtnKeyPrefix = "ntn_";
        private const int MaxKeyLenght = 50;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Returns if the api key entered is in the valid format for notion or not.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>If the key is valid or not.</returns>
        public static bool IsKeyValid(string key)
        {
            return 
                !string.IsNullOrEmpty(key) &&
                PrefixValid(key) && 
                LenghtValid(key);
        }


        /// <summary>
        /// Validates the prefix of the key used.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>If the prefix is valid.</returns>
        private static bool PrefixValid(string key)
        {
            return key.Contains(SecretAPIKeyPrefix) || key.Contains(NtnKeyPrefix);
        }
        

        /// <summary>
        /// Validates the lenght of the key to max of 50 characters.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>If the lenght is valid.</returns>
        private static bool LenghtValid(string key)
        {
            if (key.Length != MaxKeyLenght) return false;

            if (key.Contains(SecretAPIKeyPrefix))
            {
                return key.Replace(SecretAPIKeyPrefix, string.Empty).Length ==
                       (MaxKeyLenght - SecretAPIKeyPrefix.Length);
            }

            if (key.Contains(NtnKeyPrefix))
            {
                return key.Replace(NtnKeyPrefix, string.Empty).Length ==
                       (MaxKeyLenght - NtnKeyPrefix.Length);
            }

            return false;
        }
    }
}