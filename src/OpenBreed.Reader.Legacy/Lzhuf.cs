/*
    C# translation of lzhuf.c

    Original:
        lzhuf.c
        written by Haruyasu Yoshizaki 11/20/1988
        some minor changes 4/6/1989
        comments translated by Haruhiko Okumura 4/7/1989

    Original copyright:
        Copyright (C) 1989 by Haruyasu Yoshizaki,
        Haruhiko Okumura, and contributors.

    The original license terms apply.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenBreed.Reader.Legacy
{
    public sealed class Lzhuf
    {
        #region Private Fields

        private const int N = 4096;
        private const int F = 60;
        private const int Threshold = 2;

        private const int NChar = 256 - Threshold + F; // 314
        private const int T = NChar * 2 - 1;           // 627
        private const int R = T - 1;                   // 626
        private const int MaxFreq = 0x8000;

        private static readonly byte[] d_code = new byte[256]
        {
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01,
        0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01,
        0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02,
        0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02,
        0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
        0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
        0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
        0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
        0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
        0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09,
        0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A,
        0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B,
        0x0C, 0x0C, 0x0C, 0x0C, 0x0D, 0x0D, 0x0D, 0x0D,
        0x0E, 0x0E, 0x0E, 0x0E, 0x0F, 0x0F, 0x0F, 0x0F,
        0x10, 0x10, 0x10, 0x10, 0x11, 0x11, 0x11, 0x11,
        0x12, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13,
        0x14, 0x14, 0x14, 0x14, 0x15, 0x15, 0x15, 0x15,
        0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17,
        0x18, 0x18, 0x19, 0x19, 0x1A, 0x1A, 0x1B, 0x1B,
        0x1C, 0x1C, 0x1D, 0x1D, 0x1E, 0x1E, 0x1F, 0x1F,
        0x20, 0x20, 0x21, 0x21, 0x22, 0x22, 0x23, 0x23,
        0x24, 0x24, 0x25, 0x25, 0x26, 0x26, 0x27, 0x27,
        0x28, 0x28, 0x29, 0x29, 0x2A, 0x2A, 0x2B, 0x2B,
        0x2C, 0x2C, 0x2D, 0x2D, 0x2E, 0x2E, 0x2F, 0x2F,
        0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
        0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
        };

        private static readonly byte[] d_len = new byte[256]
        {
        0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
        0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
        0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
        0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
        0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
        0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
        0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
        0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
        0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
        0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
        0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
        0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
        0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
        0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
        0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
        0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
        0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
        0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
        0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
        };

        private readonly ushort[] freq = new ushort[T + 1];
        private readonly int[] prnt = new int[T + NChar];
        private readonly int[] son = new int[T];

        private readonly byte[] text_buf = new byte[N + F - 1];
        private Stream _input = null!;
        private int _getBuf;
        private int _getLen;

        #endregion Private Fields

        #region Public Methods

        public void Decode(Stream input, Stream output)
        {
            _input = input;

            using var reader = new BinaryReader(input, Encoding.ASCII, leaveOpen: true);

            uint textSize = reader.ReadUInt32();
            if (textSize == 0)
                return;

            _getBuf = 0;
            _getLen = 0;

            StartHuff();

            // Initialize the sliding window with spaces.
            Array.Fill(text_buf, (byte)' ', 0, N - F);

            int r = N - F;
            uint count = 0;

            while (count < textSize)
            {
                int c = DecodeChar();

                if (c < 256)
                {
                    output.WriteByte((byte)c);

                    text_buf[r] = (byte)c;
                    r = (r + 1) & (N - 1);

                    count++;
                }
                else
                {
                    int i = (r - DecodePosition() - 1) & (N - 1);
                    int length = c - 255 + Threshold;

                    for (int k = 0; k < length && count < textSize; k++)
                    {
                        byte value = text_buf[(i + k) & (N - 1)];

                        output.WriteByte(value);

                        text_buf[r] = value;
                        r = (r + 1) & (N - 1);

                        count++;
                    }
                }

                // Optional progress callback could be invoked here.
                // Progress?.Invoke(count, textSize);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Read one bit from the compressed stream.
        /// Returns 0 or 1.
        /// </summary>
        private int GetBit()
        {
            while (_getLen <= 8)
            {
                int b = _input.ReadByte();
                if (b < 0)
                    b = 0;

                _getBuf |= (ushort)(b << (8 - _getLen));
                _getLen += 8;
            }

            int bit = (_getBuf & 0x8000) != 0 ? 1 : 0;

            _getBuf <<= 1;
            _getLen--;

            return bit;
        }

        /// <summary>
        /// Read one byte from the compressed stream.
        /// </summary>
        private int GetByte()
        {
            while (_getLen <= 8)
            {
                int b = _input.ReadByte();
                if (b < 0)
                    b = 0;

                _getBuf |= b << (8 - _getLen);
                _getLen += 8;
            }

            int value = _getBuf;
            _getBuf <<= 8;
            _getLen -= 8;

            return (value >> 8) & 0xFF;
        }

        private void StartHuff()
        {
            int i, j;

            for (i = 0; i < NChar; i++)
            {
                freq[i] = 1;
                son[i] = (short)(i + T);
                prnt[i + T] = (short)i;
            }

            i = 0;
            j = NChar;

            while (j <= R)
            {
                freq[j] = (ushort)(freq[i] + freq[i + 1]);
                son[j] = (short)i;

                prnt[i] = (short)j;
                prnt[i + 1] = (short)j;

                i += 2;
                j++;
            }

            freq[T] = 0xFFFF;
            prnt[R] = 0;
        }

        private int DecodeChar()
        {
             int c = son[R];

            // Traverse the Huffman tree from the root to a leaf.
            while (c < T)
            {
                c += GetBit();
                c = son[c];
            }

            c -= T;
            Update(c);

            return c;
        }

        private void Reconstruct()
        {
            int i, j, k;

            // Collect leaf nodes into the first half of the table
            // and halve their frequencies.
            j = 0;

            for (i = 0; i < T; i++)
            {
                if (son[i] >= T)
                {
                    freq[j] = (ushort)((freq[i] + 1) >> 1);
                    son[j] = son[i];
                    j++;
                }
            }

            // Rebuild the internal nodes.
            for (i = 0, j = NChar; j < T; i += 2, j++)
            {
                int right = i + 1;

                ushort f = (ushort)(freq[i] + freq[right]);
                freq[j] = f;

                // Find insertion point.
                k = j - 1;
                while (f < freq[k])
                    k--;

                k++;

                // Equivalent to C's memmove().
                int count = j - k;

                if (count > 0)
                {
                    Array.Copy(freq, k, freq, k + 1, count);
                    Array.Copy(son, k, son, k + 1, count);
                }

                freq[k] = f;
                son[k] = (short)i;
            }

            // Reconnect parent pointers.
            for (i = 0; i < T; i++)
            {
                k = son[i];

                if (k >= T)
                {
                    prnt[k] = (short)i;
                }
                else
                {
                    prnt[k] = (short)i;
                    prnt[k + 1] = (short)i;
                }
            }
        }

        private void Update(int c)
        {
            if (freq[R] == MaxFreq)
            {
                Reconstruct();
            }

            // Move from the leaf to its parent.
            c = prnt[c + T];

            do
            {
                int k = ++freq[c];

                // If frequencies are now out of order, swap nodes.
                int l = c + 1;
                if (k > freq[l])
                {
                    while (k > freq[++l])
                    {
                        // Find insertion point.
                    }

                    l--;

                    freq[c] = (ushort)freq[l];
                    freq[l] = (ushort)k;

                    // Swap children.
                    int i = son[c];
                    int j = son[l];

                    son[l] = (short)i;
                    son[c] = (short)j;

                    // Fix parent links.
                    prnt[i] = (short)l;
                    if (i < T)
                        prnt[i + 1] = (short)l;

                    prnt[j] = (short)c;
                    if (j < T)
                        prnt[j + 1] = (short)c;

                    c = l;
                }

                c = prnt[c];
            } while (c != 0);
        }

        private int DecodePosition()
        {
            int i = GetByte();

            // Decode the upper six bits from the lookup tables.
            int position = d_code[i] << 6;

            int bits = d_len[i] - 2;

            // Read the remaining lower six bits directly.
            while (bits-- > 0)
            {
                i = (i << 1) | GetBit();
            }

            return position | (i & 0x3F);
        }

        #endregion Private Methods
    }
}