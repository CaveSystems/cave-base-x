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
    /// Provides a dictionary for <see cref="Base64"/> implementations
    /// </summary>
    public sealed class Base64Dictionary 
    {
        #region private implementation
        char[] m_Characters = new char[64];
        int[] m_Values = new int[128];
        int m_Count = 0;

        private Base64Dictionary(Base64Dictionary cloneData)
        {
            m_Characters = (char[])cloneData.m_Characters.Clone();
            m_Values = (int[])cloneData.m_Values.Clone();
            m_Count = cloneData.m_Count;
        }
        #endregion

        /// <summary>
        /// Creates a new empty <see cref="Base64Dictionary"/>
        /// </summary>
        public Base64Dictionary(string charset)
        {
            if (charset == null)
            {
                throw new ArgumentNullException("charset");
            }

            if (charset.Length != 64)
            {
                throw new ArgumentOutOfRangeException(nameof(charset), "Charset of 64 7 bit ascii characters expected!");
            }

            foreach (char c in charset)
            {
                if ((c < 1) || (c > 127))
                {
                    throw new InvalidDataException(string.Format("Invalid character 0x{0}!", ((int)c).ToString("x")));
                }

                m_Characters[m_Count] = c;
                m_Values[c] = ++m_Count;
            }
        }

        /// <summary>
        /// Obtains the value for the specified character
        /// </summary>
        /// <param name="c">The <see cref="char"/> to look up</param>
        /// <returns>Returns the value (index) for the char</returns>
        /// <exception cref="ArgumentException">Thrown if the dictionary was not jet completed</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the symbol could not be found</exception>
        public int this[char c]
        {
            get
            {
                if (m_Count != 64)
                {
                    throw new ArgumentException("Dictionary does not contain 64 key valid combinations!");
                }

                int result = m_Values[c] - 1;
                if (result < 0)
                {
                    throw new KeyNotFoundException(string.Format("Invalid symbol '{0}'!", c));
                }

                return result;
            }
        }

        /// <summary>
        /// Obtains the character for the specified value
        /// </summary>
        /// <param name="value">The value to look up</param>
        /// <returns>Returns the character for the value</returns>
        public char this[int value]
        {
            get
            {
                return m_Characters[value];
            }
        }

        #region ICloneable Member

        /// <summary>
        /// Clones the <see cref="Base64Dictionary"/>
        /// </summary>
        /// <returns>Returns a copy</returns>
        public Base64Dictionary Clone()
        {
            return new Base64Dictionary(this);
        }

        #endregion
    }
}
