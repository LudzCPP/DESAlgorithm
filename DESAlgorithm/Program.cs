using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DESAlgorithm
{
    class Program
    {
        private static int[] KeyPermutPC1 = new int[]{
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4};

        private static int[] KeyShifts = new int[]
                {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};

        private static int[] KeyPermutPC2 = new int[]{
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32};

        private static int[] InitPermut = new int[]{
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7};

        private static int[] ESelectionTable = new int[]{
            32, 1, 2, 3, 4, 5,
            4, 5, 6, 7, 8, 9,
            8, 9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32, 1};

        private static int[][] SBoxes = new int[][]{
            new int[]{
                    14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7,
                    0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8,
                    4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0,
                    15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13},
            new int[]{
                    15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10,
                    3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5,
                    0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15,
                    13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9},
            new int[]{
                    10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8,
                    13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1,
                    13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7,
                    1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12},
            new int[]{
                    7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15,
                    13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9,
                    10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4,
                    3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14},
            new int[]{
                    2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9,
                    14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6,
                    4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14,
                    11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3},
            new int[]{
                    12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11,
                    10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8,
                    9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6,
                    4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13},
            new int[]{
                    4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1,
                    13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6,
                    1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2,
                    6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12},
            new int[]{
                    13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7,
                    1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2,
                    7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8,
                    2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}};

        private static int[] PBlockPermut = new int[]{
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25};

        private static int[] ReversedInitialPermut = new int[]{
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25};

        public static int intArrayToInt(int[] intArray)
        {
            int result = 0;
            for (int i = intArray.Length - 1; i >= 0; i--)
            {
                result += intArray[i] * (int)Math.Pow(2, intArray.Length - 1 - i);
            }
            return result;
        }

        public static int[] IntTointArray(int Value, int NumOfints)
        {
            int[] intArray = new int[NumOfints]; // 32 bajty dla liczby 32-bitowej
            for (int i = intArray.Length - 1; i >= 0; i--)
            {
                intArray[i] = (int)(Value & 0x01);
                Value >>= 1;
            }
            return intArray;
        }

        private static int[] Permut(int[] Input, int[] PermutTab)
        {
            int[] Result = new int[PermutTab.Length];

            for (int i = 0; i < PermutTab.Length; i++)
            {
                Result[i] = Input[PermutTab[i] - 1];
            }

            return Result;
        }

        public static int[][] StringToBinaryTable(string inputString)
        {


            int[][] binaryTable = new int[inputString.Length][];

            for (int i = 0; i < inputString.Length; i++)
            {
                int asciiValue = inputString[i];
                string binaryString = Convert.ToString(asciiValue, 2).PadLeft(8, '0');
                binaryTable[i] = new int[binaryString.Length];
                for (int j = 0; j < binaryString.Length; j++)
                {
                    binaryTable[i][j] = binaryString[j] - '0';
                }
            }

            return binaryTable;
        }

        public static void Show2DTable(int[][] binaryTable)
        {
            for (int i = 0; i < binaryTable.Length; i++)
            {
                for (int j = 0; j < binaryTable[i].Length; j++)
                {
                    Console.Write(binaryTable[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void ShowTable(int[] table)
        {
            foreach (int i in table)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
        }

        public static int[][] PadAndFlattenBinaryTable(int[][] binaryTable)
        {
            int rows = (int)Math.Ceiling((double)binaryTable.Length * 8 / 64);
            int[][] paddedTable = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                paddedTable[i] = new int[64];
                for (int j = 0; j < 64; j++)
                {
                    int row = j / 8;
                    int col = j % 8;
                    int index = i * 8 + row;
                    int[] currentRow = index < binaryTable.Length ? binaryTable[index] : new int[8];

                    if (col < currentRow.Length)
                    {
                        paddedTable[i][j] = currentRow[col];
                    }
                    else
                    {
                        paddedTable[i][j] = 0;
                    }
                }
            }

            return paddedTable;
        }



        public static (int[], int[]) ShiftLeft(int[] inputC, int[] inputD, int shiftCount)
        {
            int length1 = inputC.Length;
            int length2 = inputD.Length;
            int[] shiftedArray1 = new int[length1];
            int[] shiftedArray2 = new int[length2];

            for (int i = 0; i < length1; i++)
            {
                int newIndex = (i - shiftCount + length1) % length1;
                shiftedArray1[newIndex] = inputC[i];
            }

            for (int i = 0; i < length2; i++)
            {
                int newIndex = (i - shiftCount + length2) % length2;
                shiftedArray2[newIndex] = inputD[i];
            }

            //ShowTable(shiftedArray);
            return (shiftedArray1, shiftedArray2);
        }

        public static int[] XOR(int[] inputA, int[] inputB)
        {
            int[] result = new int[inputA.Length];

            for (int i = 0; i < inputA.Length; i++)
            {
                result[i] = inputA[i] ^ inputB[i];
            }


            return result;
        }

        public static int[] Fun(int[] inputA, int[] inputB)
        {
            int[] result = new int[8];
            inputA = Permut(inputA, ESelectionTable);
            //ShowTable(inputA);

            int[] xor = XOR(inputA, inputB);

            int[][] Groups = Group(xor);

            for (int i = 0; i < 8; i++)
            {
                int[] row = new int[2];
                int[] col = new int[4];
                row[0] = Groups[i][0];
                row[1] = Groups[i][5];

                for (int j = 0; j < 4; j++)
                {
                    col[j] = Groups[i][j + 1];
                }

                Groups[i] = DecimalToBinaryIntArray(SBoxes[i][16 * BinaryIntArrayToDecimal(row) + BinaryIntArrayToDecimal(col)]);
                result = Flatten2DArray(Groups);

                result = Permut(result, PBlockPermut);
            }


            return result;
        }

        public static int[][] Group(int[] inputA)
        {
            int[][] result = new int[8][];

            for (int i = 0; i < 8; i++)
            {
                result[i] = new int[6];
                for (int j = 0; j < 6; j++)
                {
                    result[i][j] = inputA[i * 6 + j];
                }
            }

            return result;
        }


        public static int[] Flatten2DArray(int[][] twoDArray)
        {
            int rows = twoDArray.Length;
            int cols = twoDArray[0].Length;
            int[] oneDArray = new int[rows * cols];
            int index = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    oneDArray[index++] = twoDArray[i][j];
                }
            }

            return oneDArray;
        }


        public static int BinaryIntArrayToDecimal(int[] binaryArray)
        {
            int decimalNumber = 0;
            int length = binaryArray.Length;

            for (int i = 0; i < length; i++)
            {
                int currentBit = binaryArray[length - 1 - i];

                if (currentBit == 1)
                {
                    decimalNumber += (int)Math.Pow(2, i);
                }
            }

            return decimalNumber;
        }

        public static int[] DecimalToBinaryIntArray(int decimalNumber)
        {
            int[] binaryArray = new int[4];

            for (int i = 0; i < 4; i++)
            {
                binaryArray[3 - i] = decimalNumber % 2;
                decimalNumber /= 2;
            }

            return binaryArray;
        }


        public static int[] Encrypt(int[] message, string key)
        {
            /* int[] xd = new int[]{0,0,0,1,0,0,1,1,0,0,1,1,0,1,0,0,0,1,0,1,0,1,1,1,0,1,1,1,1,0,0,1,1,0,0,1,1,0,1,1,
                 1,0,1,1,1,1,0,0,1,1,0,1,1,1,1,1,1,1,1,1,0,0,0,1 };

             int[] msg = new int[]{0,0,0,0,0,0,0,1,0,0,1,0,0,0,1,1,0,1,0,0,0,1,0,1,0,1,1,0,0,1,1,1,1,0,0,0,1,0,0,1,
                 1,0,1,0,1,0,1,1,1,1,0,0,1,1,0,1,1,1,1,0,1,1,1,1 };*/


            int[][] keyxd = StringToBinaryTable(key);
            int[] msg = message;
            int[] xd = Flatten2DArray(keyxd);



            int[][] Keys = new int[16][];

            int[] xd1 = Permut(xd, KeyPermutPC1);
            int[] C = xd1.Take(xd1.Length / 2).ToArray();
            int[] D = xd1.Skip(xd1.Length / 2).ToArray();


            for (int i = 0; i < 16; i++)
            {
                (C, D) = ShiftLeft(C, D, KeyShifts[i]);
                Keys[i] = Permut(C.Concat(D).ToArray(), KeyPermutPC2);
            }


            //ShowTable(ShiftedArrayC.Concat(ShiftedArrayD).ToArray());
            //ShowTable(ShiftedArrayD);

            //Console.WriteLine();
            /*for (int i = 0; i < 16; i++)
            {
                ShowTable(Keys[i]);
            }*/

            msg = Permut(msg, InitPermut);
            int[] L = msg.Take(msg.Length / 2).ToArray();
            int[] R = msg.Skip(msg.Length / 2).ToArray();
            //ShowTable(L);
            //ShowTable(R);

            int[][] Ls = new int[17][];
            int[][] Rs = new int[17][];

            Ls[0] = L;
            Rs[0] = R;


            for (int i = 1; i < 17; i++)
            {
                Ls[i] = Rs[i - 1];
                Rs[i] = XOR(Ls[i - 1], Fun(Rs[i - 1], Keys[i - 1]));
            }

            int[] result = Rs[16].Concat(Ls[16]).ToArray();
            result = Permut(result, ReversedInitialPermut);

            return result;
        }

        public static string BinaryToAscii(int[] binaryArray)
        {
            byte[] byteArray = new byte[binaryArray.Length / 8];

            for (int i = 0; i < binaryArray.Length; i += 8)
            {
                byte b = 0;
                for (int j = 0; j < 8; j++)
                {
                    b = (byte)((b << 1) | binaryArray[i + j]);
                }
                byteArray[i / 8] = b;
            }

            return Encoding.ASCII.GetString(byteArray);
        }

        public static string BinaryToHex(int[] binaryArray)
        {
            StringBuilder hexBuilder = new StringBuilder(binaryArray.Length / 4);

            for (int i = 0; i < binaryArray.Length; i += 4)
            {
                int nibble = binaryArray[i] << 3 | binaryArray[i + 1] << 2 | binaryArray[i + 2] << 1 | binaryArray[i + 3];
                hexBuilder.Append(nibble.ToString("X"));
            }

            return hexBuilder.ToString();
        }




        static void Main(string[] args)
        {
            String message = "AB12CD34AV";
            String key = "AL1029AS";
            string fileText = File.ReadAllText("C:\\Users\\kacpe\\Downloads\\PoliClinic - Inf. ws. badań studentów.pdf");
            byte[] fileBytes = File.ReadAllBytes("C:\\Users\\kacpe\\Downloads\\PoliClinic - Inf. ws. badań studentów.pdf");

            int[][] messagexd = StringToBinaryTable(fileText);

            int[][] msgPad = PadAndFlattenBinaryTable(messagexd);
            /*Show2DTable(messagexd);
            Console.WriteLine();
            Show2DTable(msgPad);*/
            foreach (int[] x in msgPad)
            {
                Console.Write(BinaryToHex(Encrypt(x, key)));
            }


        }
    }
}