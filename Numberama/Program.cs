// Cynthia Tristán Álvarez
// María Solórzano Gómez
using System;
using System.Threading;

namespace Numberama
{
    internal class Program
    {
        // Constantes
        const int MAX_NUM = 400, // número máximo de eltos en la secuencia    
                  MODO = 1, // modo de juego (cant)
                  ANCHO = 9; // cantidad de columnas / anchura de filas
        const bool DEBUG = true;

        // Método Main
        static void Main(string[] args)
        {
            // Variables
            int[] nums = new int[MAX_NUM];
            int cont = 0, // número de dígitos de la secuencia = primera posición libre en la secuencia
                act = MAX_NUM - 1, // posición del cursor en la secuencia
                sel = -1; // posición de la casilla seleccionada; -1 si no hay selección

            Console.CursorVisible = false;

            while (cont < MAX_NUM)
            Genera(nums, ref cont, MODO);
            
            Render(nums, cont, act, sel);

            while (!Terminado(nums, cont))
            {
                char ch = LeeInput();
                if (ch != ' ')
                {
                    ProcesaInput(ch, nums, ref cont, ref act, ref sel);
                    Render(nums, cont, act, sel);
                }
                Thread.Sleep(100);
            }
            Console.Write("El juego ha finalizado.");
            while (true);
        }

        // Métodos
        static void Render(int[] nums, int cont, int act, int sel)
        {
            Console.Clear();

            if (DEBUG) // Datos de control
            {
                Console.WriteLine("nums: " + string.Join("", nums));
                Console.WriteLine("cont: " + cont);
                Console.WriteLine("act: " + act);
                Console.WriteLine("sel: " + sel);
            }

            for (int i = 0; i < cont; i++)
            {
                if (nums[i] == 0)
                {
                    if (i == act)
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    else
                        Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("  ");
                }
                else
                {
                    if (i == act)
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    else if (i == sel)
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" " + nums[i]);
                }

                Console.BackgroundColor = ConsoleColor.Black;

                if ((i + 1) % ANCHO == 0)
                    Console.Write("\n");
            }

            Console.WriteLine("");
        }

        static void Genera(int[] nums, ref int cont, int cant) // Añade cant a nums desde cont
        {
            if (cont <= (MAX_NUM - cant)) // solo si cant cabe
            {
                for (int i = 1; i <= cant; i++)
                {
                    if (i < 10) // Menores de 10 
                    {
                        nums[cont] = i;
                        cont++;
                    }
                    else if (i > 10 && !(i % 10 == 0)) // Mayores de 10 y no son múltiplos
                    {
                        nums[cont] = i / 10;
                        cont++;
                        nums[cont] = i % 10;
                        cont++;
                    }
                }
            }
        }

        static char LeeInput() // Lectura del input a ch
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

        static void ProcesaInput(char ch, int[] nums, ref int cont, ref int act, ref int sel) // Diferentes comportamientos según ch (switch?)
        {
            switch (ch)
            {
                case 'l':
                    if (act > 0) 
                        act--;
                    return;
                case 'r':
                    if (act < (cont - 1)) 
                        act++;
                    return;
                case 'u':
                    if (act >= ANCHO) 
                        act -= ANCHO;
                    return;
                case 'd':
                    if (act < (cont - ANCHO)) 
                        act += ANCHO;
                    return;
                case 'x':
                    if (nums[act] != 0) // tal cual el enunciado pag. 4 
                    {
                        if (sel == -1)
                            sel = act;
                        else if (EliminaPar(nums, cont, act, sel))
                        {
                            nums[act] = 0;
                            nums[sel] = 0;
                            LimpiaFilas(nums, ref cont, ref sel);
                            LimpiaFilas(nums, ref cont, ref act);
                            sel = -1;
                        }
                        else 
                            sel = act;
                    }
                    return;
                case 'g':
                    Genera(nums, ref cont, MODO); 
                    return;
                case 'q':
                    return;
                default:
                    return;
            }
        }

        static bool Contiguos(int[] nums, int cont, int act, int sel)
        {
            int menor = Math.Min(act, sel),
                mayor = Math.Max(act, sel);

            if (menor == mayor) // no puede ser la misma casilla
                return false;
            if (mayor == menor + 1 || mayor == menor + ANCHO) // inmediatamente adyacente horizontal o vertical
                return true;

            int i = menor + 1; // búsqueda contiguos con 0s horizontales
            while ((i + 1) < cont && nums[i] == 0) // mientras los siguientes sean 0 y no se salgan de rango
                i++;
            if (i == mayor) // solo si al acabar ha llegado al mayor
                return true;

            int j = menor + ANCHO; // búsqueda contiguos con 0s verticales
            while ((j + ANCHO) < cont && nums[j] == 0)
                j += ANCHO;
            if (j == mayor)
                return true;

            return false; // si ninguna de las condiciones se ha cumplido
        }

        static bool EliminaPar(int[] nums, int cont, int act, int sel)
        {
            if (Contiguos(nums, cont, act, sel) && (nums[act] == nums[sel] || nums[act] + nums[sel] == 10))
                return true;
            else
                return false;
        }

        static bool Terminado(int[] nums, int cont) // true si todo nums[i] es igual a 0
        {
            int i = 0; // parecido a contiguos horizontales
            while (i < cont)
            {
                if (nums[i] == 0)
                    i++;
                else
                    return false;
            }
            return true;
        }

        static void Guarda(int[] nums, int cont)
        {

        }

        static void Lee(int[] nums, int cont)
        {

        }

        // Extensiones opcionales: LimpiaFila(nums,cont,pos), Pista, Deshacer jugadas.

        static void LimpiaFilas(int[] nums, ref int cont, ref int pos)
        {
            int linea = pos - (pos % ANCHO),
                i = linea;

            if (i + ANCHO < MAX_NUM)
                while (nums[i] == 0 && i < linea + ANCHO)
                    i++;
            else
                while (nums[i] == 0 && i < MAX_NUM - 1)
                    i++;

            if (i == linea + ANCHO || i == MAX_NUM - 1)
            {
                for (int j = linea; j < cont; j++)
                {
                    if (j + ANCHO >= MAX_NUM)
                        nums[j] = 0;
                    else
                        nums[j] = nums[j + ANCHO];
                }
                cont -= Math.Min(ANCHO, cont - linea);
                pos -= ANCHO;
            }
        }
    }
}
