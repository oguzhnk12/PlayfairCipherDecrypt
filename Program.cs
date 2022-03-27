using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayfairCipher_decrypt_
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\t\t\t***** PLAYFAIR CIPHER METHOD *****\n\t\t\t  ***** DECRYPT PROGRAM *****");   
            string path = @"../../../ciphertext.txt";
            Boolean exist = File.Exists(path);
            string ciphertext;
            char[] cipher = new char[999];
            if (exist)
            {
                ciphertext = File.ReadAllText(path);
                cipher = OmitJ(ciphertext.ToCharArray());
                ciphertext = new string(cipher);
                if (ciphertext.All(c => Char.IsLetter(c)))
                {
                    if (!(ciphertext.Length < 6))
                    {
                        cipher = OmitJ(ciphertext.ToUpper().ToCharArray());
                        if (cipher.Length % 2 != 0)
                            cipher = cipher.Take(cipher.Length - 1).ToArray();
                    }
                    else
                    {
                        Console.WriteLine("Ciphertext length should not be less than 15.");
                        Console.WriteLine("Please press any key for exit...");
                        Console.ReadKey();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Ciphertext should  be contain only letters.");
                    Console.WriteLine("Please press any key for exit...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("File does not exist.");
                Console.WriteLine("Please press any key for exit...");
                Console.ReadKey();
                return;
            }
            Boolean validate = false;
            string keyword = "";
            do
            {
                Console.WriteLine("Please enter the keyword.\n");
                    keyword = Console.ReadLine();
                if (keyword.Length > 0)
                {
                    if (keyword.All(c => Char.IsLetter(c)))
                        validate = true;
                    else
                        Console.WriteLine("Keyword should contain only letters, please try again.");
                }
                else
                {
                    Console.WriteLine("You typed nothing, please try again.");
                }
            } while(!validate);
            char[] keywordArray = OmitJ(keyword.ToUpper().ToCharArray());
            keywordArray = RemoveDuplication(keywordArray);
            //Alphabet array creating
            char[] alphabet = new char[26];
            int index = 0;
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                if (letter !='J')
                {
                    alphabet[index] = letter;
                    index++;
                }
            }
            char[] matrixLetter = keywordArray.Concat(alphabet).ToArray();
            matrixLetter = RemoveDuplication(matrixLetter);
            char[,] matrix = new char[5,5];
            index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = matrixLetter[index];
                    index++;
                }
            }
            string plaintext = PlayfairCipher(matrix, cipher);
            Console.WriteLine("\n\nKeyword: \t" + keyword);
            Console.WriteLine("Ciphertext: \t" + ciphertext);
            Console.WriteLine("Plaintext: \t" + plaintext + "\n");
            Console.WriteLine("Please press any key for exit...");
            Console.ReadKey();
        }
        public static char[] RemoveDuplication(char[] charArray)
        {
            int check = 0;
            for (int i = 0; i < charArray.Length; i++)
            {
                for (int j = i+1; j < charArray.Length; j++)
                {
                    if (charArray[i] == charArray[j])
                        check++;
                    if (check == 2)
                    {
                        check = 0;
                        charArray = charArray.Except(new char[j]).ToArray();
                    }
                }
            }
            return charArray;
        }
        public static char[] OmitJ(char[] charArray)
        {
            char space = (char)32;
            for (int c = 0; c < charArray.Length; c++)
            {
                if (charArray[c] == space)
                    charArray = charArray.Where((source, index) => index != c).ToArray();
                if  ((charArray[c] == 'J') || (charArray[c] == 'İ'))
                    charArray[c] = 'I';
            }
            return charArray;
        }
        public static string PlayfairCipher(char[,] matrix, char[] cipher)
        {
            string plaintextStr;
            char[] plaintext = new char[cipher.Length];
            int index = 0;
            int check = 0;
            while (index < cipher.Length)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (matrix[i, j] == cipher[index])
                        {
                            for (int a = 0; a < 5; a++)
                            {
                                for (int b = 0; b < 5; b++)
                                {
                                    if (matrix[a, b] == cipher[index + 1])
                                    {

                                        if ((a == i) && (b == j))
                                        {
                                            if ((a == 0) && (b == 0))
                                            {
                                                plaintext[index] = plaintext[index + 1] = matrix[4, 4];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                            else if (a == 0)
                                            {
                                                plaintext[index] = plaintext[index + 1] = matrix[4, b - 1];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                            else if (b == 0)
                                            {
                                                plaintext[index] = plaintext[index + 1] = matrix[a - 1, 4];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                            else
                                            {
                                                plaintext[index] = plaintext[index + 1] = matrix[a - 1, b - 1];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                        }

                                        else if (a == i)
                                        {
                                            if (j == 0)
                                            {
                                                plaintext[index] = matrix[i, 4];
                                                plaintext[index + 1] = matrix[a, b - 1];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                            else if (b == 0)
                                            {
                                                plaintext[index] = matrix[i, j - 1];
                                                plaintext[index + 1] = matrix[a, 4];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                            else
                                            {
                                                plaintext[index] = matrix[i, j - 1];
                                                plaintext[index + 1] = matrix[a, b - 1];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                        }
                                        else if (b == j)
                                        {
                                            if (i == 0)
                                            {
                                                plaintext[index] = matrix[4, j];
                                                plaintext[index + 1] = matrix[a - 1, b];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                            else if (a == 0)
                                            {
                                                plaintext[index] = matrix[i - 1, j];
                                                plaintext[index + 1] = matrix[4, b];
                                                index = index + 2;
                                                check = 1;
                                                break;
                                            }
                                            else
                                            {
                                                plaintext[index] = matrix[i - 1, j];
                                                plaintext[index + 1] = matrix[a - 1, b];
                                                check = 1;
                                                index = index + 2;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            plaintext[index] = matrix[i, b];
                                            plaintext[index + 1] = matrix[a, j];
                                            index = index + 2;
                                            check = 1;
                                            break;
                                        }

                                    }
                                }
                                if (check == 1)
                                    break;

                            }
                        }
                        if (check == 1)
                            break;
                    }
                    if (check == 1)
                        break;
                }
                check = 0;
            }

            plaintextStr = new string(plaintext);
            return plaintextStr;
        }

    }
}
