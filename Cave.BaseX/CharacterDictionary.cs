#region CopyRight 2018
/*
    Copyright (c) 2003-2018 Andreas Rohleder (andreas@rohleder.cc)
    All rights reserved
*/
#endregion
#region License LGPL-3
/*
    This program/library/sourcecode is free software; you can redistribute it
    and/or modify it under the terms of the GNU Lesser General Public License
    version 3 as published by the Free Software Foundation subsequent called
    the License.

    You may not use this program/library/sourcecode except in compliance
    with the License. The License is included in the LICENSE file
    found at the installation directory or the distribution package.

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be included
    in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion
#region Authors & Contributors
/*
   Author:
     Andreas Rohleder <andreas@rohleder.cc>

   Contributors:
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;

namespace Cave
{
    /// <summary>
    /// Provides a ascii character dictionary (this is used for example at the <see cref="Base64"/> implementation)
    /// </summary>
    public sealed class CharacterDictionary
    {
        #region private implementation
        char[] m_Characters;
        int[] m_Values = new int[128];

        private CharacterDictionary(CharacterDictionary cloneData)
        {
            m_Characters = (char[])cloneData.m_Characters.Clone();
            m_Values = (int[])cloneData.m_Values.Clone();
        }
        #endregion

        /// <summary>Gets the length.</summary>
        /// <value>The length.</value>
        public int Length
        {
            get { return m_Characters.Length; }
        }

        /// <summary>
        /// Creates a new empty <see cref="CharacterDictionary"/>
        /// </summary>
        public CharacterDictionary(string charset)
        {
            if (charset == null)
            {
                throw new ArgumentNullException("charset");
            }

            for (int i = 0; i < 128; i++)
            {
                m_Values[i] = -1;
            }

            m_Characters = charset.ToCharArray();
            for(int i = 0; i < m_Characters.Length; i++)
            {
                m_Values[m_Characters[i]] = i;
            }
        }

        /// <summary>
        /// Obtains the value for the specified character
        /// </summary>
        /// <param name="character">The <see cref="char"/> to look up</param>
        /// <returns>Returns the value (index) for the char</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the symbol could not be found</exception>
        public int GetValue(char character)
        {
            int result = m_Values[character];
            if (result < 0)
            {
                throw new KeyNotFoundException(string.Format("Invalid symbol '{0}'!", character));
            }

            return result;
        }

        /// <summary>Tries to get the value for the given character. If no values is available defaultValue will be returned.</summary>
        /// <param name="character">The character.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public int TryGetValue(char character, int defaultValue)
        {
            if (character < 0 || character >= m_Values.Length)
            {
                return defaultValue;
            }

            int result = m_Values[character];
            if (result < 0)
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Obtains the character for the specified value
        /// </summary>
        /// <param name="value">The value to look up</param>
        /// <returns>Returns the character for the value</returns>
        public char GetCharacter(int value)
        {
            return m_Characters[value];
        }

        /// <summary>
        /// Clones the <see cref="CharacterDictionary"/>
        /// </summary>
        /// <returns>Returns a copy</returns>
        public CharacterDictionary Clone()
        {
            return new CharacterDictionary(this);
        }
    }
}
