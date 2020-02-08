using System;

namespace mastermind
{
    class Mastermind
    {
        static void printBoard(int[,] guesses, int num_options, string[,] status, string[] emojis, bool choice) {
            string delim;
            Console.WriteLine("-------------");
            for (int i = 0; i < guesses.GetLength(0); i++) {
                delim = "";
                for (int j = 0; j < guesses.GetLength(1); j++) {
                    if (guesses[i, j] == 0) {
                        Console.Write(delim + delim + "o");
                    } else {
                        Console.Write(delim + emojis[guesses[i, j]]);
                    }
                    delim = " ";
                }
                Console.Write(" ");
                for (int j = 0; j < guesses.GetLength(1); j++) {
                    Console.Write(status[i, j]);
                }
                Console.WriteLine();
            }
            delim = "";
            if (choice) {
                Console.WriteLine("-- choices --");
                for (int i = 1; i < num_options; i++) {
                    Console.Write(delim + i.ToString() + ". " + emojis[i]);
                    delim = ", ";
                }
                Console.WriteLine();
            }
        }
        static bool isValidInput(string input, int num_options, int sequence_len) {
            if (input == null) {
                Console.WriteLine();
                Console.WriteLine("Goodbye!");
                System.Environment.Exit(0);
                return false;
            }
            string[] sequence = input.Split(" ");
            int parse;
            if (sequence.GetLength(0) != sequence_len) {
                Console.WriteLine("Invalid sequence length.");
                return false;
            }
            for (int i = 0; i < sequence.GetLength(0); i++) {
                if (int.TryParse(sequence[i], out parse) == false) {
                    Console.WriteLine("Invalid number.");
                    return false;
                }
                if (parse <= 0 || parse >= num_options) {
                    Console.WriteLine("Invalid choice.");
                    return false;
                }
            }
            return true;
        }
        static void getStatus(int[,] guesses, int[] answer, string[,] status, int sequence_len, int i) {
            
            for (int a = 0; a < sequence_len; a++)
            {
                if (guesses[i, a] == answer[a])
                    status[i, a] = "black";
                else 
                    for (int b = 0; b < sequence_len; b++)
                    {
                        if (guesses[i, b] == answer[a])
                        {
                            status[i, b] = "white";
                        }
                    }
            }
        }
        static bool checkWin(string[,] status, int i) {
            for (int j = 0; j < status.GetLength(1); j++) {
                if (status[i, j] != "black") {
                    return false;
                }
            } 
            return true;
        }
        static void printWinMessage() {
            Console.WriteLine("=============");
            Console.WriteLine("You Win!");
            Console.WriteLine("=============");
        }
        static void printLoseMessage(int[] answer, string[] emojis) {
            Console.WriteLine("=============");
            Console.WriteLine("You Lose!");
            Console.Write("The Answer Was:");
            for (int i = 0; i < answer.GetLength(0); i++) {
                Console.Write(" " + emojis[answer[i]]);
            }
            Console.WriteLine();
            Console.WriteLine("=============");
        }
        static void Main(string[] args)
        {
            const int num_options = 6;
            const int sequence_len = 4;
            const int tries = 12;
            string[] emojis = {"", "\U0001F431", "\U0001F427", "\U0001F428", "\U0001F435", "\U0001F439"};
            int[,] guesses = new int[tries, sequence_len];
            string[,] status = new string[tries, sequence_len];
            int[] answer = new int[sequence_len];
            string[]guess;
            string input;
            var rand = new Random();

            /* Creates the answer sequence to guess. */
            for (int i = 0; i < sequence_len; i++) {
                answer[i] = rand.Next(1, num_options);
            }

            /* Starts the game loop. */
            for (int i = 0; i < tries; i ++) {
                /* Prints the board */
                printBoard(guesses, num_options, status, emojis, true);

                /* Gets and validates input */
                do
                {
                    Console.WriteLine("Enter a sequence separated by spaces: ");
                    Console.Write("$ ");
                    input = Console.ReadLine();
                } while (isValidInput(input, num_options, sequence_len) == false);

                /* Records the current guess into guesses. */
                guess = input.Split(" ");
                for (int j = 0; j < sequence_len; j++) {
                    guesses[i, j] = int.Parse(guess[j]);
                }

                /* Gets the status of the current guess. */
                getStatus(guesses, answer, status, sequence_len, i);

                /* Checks if the user won. */
                if (checkWin(status, i)) {
                    printBoard(guesses, num_options, status, emojis, false);
                    printWinMessage();
                    return;
                }
            }
            printBoard(guesses, num_options, status, emojis, false);
            printLoseMessage(answer, emojis);
            return;
        }
    }
}
