/*
 * Copyright (c) 2025 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

namespace CarterGames.Standalone.NotionData.Editor
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