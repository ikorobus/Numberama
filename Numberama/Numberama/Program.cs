// Cynthia Tristán Álvarez
// María Solórzano Gómez
using System;
using System.Diagnostics;
using System.Threading;

namespace Numberama
{
    internal class Program
    {
        // Constantes
        const int MAX_NUM = 400; // número máximo de eltos en la secuencia    
        const bool DEBUG = true;


        static void Main(string[] args)
        {
            // Variables
            int[] nums = new int[MAX_NUM];
            int cont = 0, // número de dígitos de la secuencia = primera posición libre en la secuencia
                act = 0, // posición del cursor en la secuencia
                sel = -1; // posición de la casilla seleccionada; -1 si no hay selección

            Console.CursorVisible = true;

            Render(nums, cont, act, sel);

            while (true)
            {
                char ch = LeeInput();
                if (ch != ' ')
                {
                    ProcesaInput(ch, nums, ref cont, act, sel);
                    Render(nums, cont, act, sel);
                }
                Thread.Sleep(100);
            }
        }
        // Métodos
        static void Render(int[] nums, int cont, int act, int sel)
        {
            if (DEBUG) // Datos de control
            {
                Console.WriteLine("nums: " + string.Join("", nums));
                Console.WriteLine("cont: " + cont);
                Console.WriteLine("act: " + act);
                Console.WriteLine("sel: " + sel);
            }

         
            for (int i = 1; i <= cont; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                if (nums[i] == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write(" ");
                }
                else
                Console.Write(nums[i]);

                if (i % 9 == 0)
                    Console.Write("\n");
            }
        }

        static void Genera(int[] nums, ref int cont, int cant)
        {
            for (int i = 1; i <= cant; i++)
            {
                if (i < 10)
                { 
                    nums[cont] = i; 
                    cont++; 
                }
                else if (i > 10)
                {
                    nums[cont] = i / 10;
                    cont++;
                    nums[cont] = i % 10;
                    cont++;
                }


            }
            
        }

        static char LeeInput()
        {
            char ch = ' ';
            if (Console.KeyAvailable)
            {
                string dir = Console.ReadKey(true).Key.ToString();
                if (dir == "A" || dir == "LeftArrow") ch = 'l'; // izquierda
                else if (dir == "D" || dir == "RightArrow") ch = 'r'; // derecha
                else if (dir == "W" || dir == "UpArrow") ch = 'u'; // arriba
                else if (dir == "S" || dir == "DownArrow") ch = 'd'; //abajo                  
                else if (dir == "X" || dir == "Spacebar") ch = 'x'; // marcar   
                else if (dir == "G" || dir == "Intro") ch = 'g'; // generar
                else if (dir == "P") ch = 'p'; // pista 
                else if (dir == "Q" || dir == "Escape") ch = 'q'; // salir
                                                                  // limpiamos buffer
                while (Console.KeyAvailable) Console.ReadKey();
            }
            return ch;
        }

        static void ProcesaInput(char ch, int[] nums, ref int cont, int act, int sel)
        {
            if (ch == 'g')
                Genera(nums, ref cont, 18);

        }

        static bool Contiguos(int[] nums, int cont, int act, int sel)
        {
            return false; // para que no de error
        }

        static bool EliminaPar(int[] nums, int cont, int act, int sel)
        {
            return false; 
        }

        static bool Terminado(int[] nums, int cont)
        {
            return false;
        }

        static void Guarda(int[] nums, int cont)
        {

        }

        static void Lee(int[] nums, int cont)
        {

        }

        // Extensiones opcionales: LimpiaFila(nums,cont,pos), Pista, Deshacer jugadas.

    }
}
