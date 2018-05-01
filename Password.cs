using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using System.Diagnostics;

namespace NEA_Solution
{
    public class Password
    {
        // Stores the actual password string
        private string text;
        public string Text
        {
            get => text;
            set => text = value;
        }

        // Stores the score given to the password by the Evaluate() method
        private int score;
        public int Score
        {
            get => score;
        }

        // Stores the strength tier given to the password by the Evaluate() method
        private PasswordStrengthTier strengthTier;
        public PasswordStrengthTier StrengthTier
        {
            get => strengthTier;
        }

        // Determines wether the the password meets validation requirements
        private bool IsValid
        {
            get
            {
                // Will match only if the string contains a character other than these
                Regex regex = new Regex("[^!$%^&*()-_=+A-Za-z0-9]");
                if (regex.IsMatch(text))
                {
                    // Creates an error message and displays it
                    MessageDialog errorMessage = new MessageDialog("Password Contains Illegal Characters", "Whoops!");
                    errorMessage.ShowAsync();
                    return false;
                }

                // Test password length
                if (text.Length < 8 || text.Length > 24)
                {
                    // Determines what to tell the user based on the password length
                    string adjective = (text.Length < 8) ? "short" : "long";
                    // Creates an error message and displays it
                    MessageDialog errorMessage = new MessageDialog($"Password is too {adjective}", "Whoops!");
                    errorMessage.ShowAsync();
                    return false;
                }
                
                return true;
            }
        }

        public bool Evaluated = false;

        public void Evaulate()
        {
            if (IsValid)
            {
                // Initialise variables
                bool containsUpperCase = false;
                bool containsLowerCase = false;
                bool containsNumbers = false;
                bool containsSymbols = false;
                
                // Initiate score with length of password
                score = text.Length;
                Debug.WriteLine($"Score: {score}");

                // Check for lowercase letters
                if (text.ToUpper() != text)
                {
                    containsLowerCase = true;
                    score += 5;
                    Debug.WriteLine("Contains Lower");
                    Debug.WriteLine($"Score: {score}");
                }
                // Check for uppercase letters
                if (text.ToLower() != text) {
                    containsUpperCase = true;
                    score += 5;
                    Debug.WriteLine("Contains Upper");
                    Debug.WriteLine($"Score: {score}");
                }
                // Check for numbers using regular expression
                Regex numberRegEx = new Regex("[0-9]");
                if (numberRegEx.IsMatch(text))
                {
                    containsNumbers = true;
                    score += 5;
                    Debug.WriteLine("Contains Num");
                    Debug.WriteLine($"Score: {score}");
                }
                // Checks to see if there are any character which are not letters or numbers
                Regex symbolRegEx = new Regex("[^A-Za-z0-9]");
                if (symbolRegEx.IsMatch(text))
                {
                    containsSymbols = true;
                    score += 5;
                    Debug.WriteLine("Contains Sym");
                    Debug.WriteLine($"Score: {score}");
                }

                // If all types of character are present
                if (containsLowerCase && containsUpperCase && containsNumbers && containsSymbols)
                {
                    Debug.WriteLine("All char. types present");
                    score += 10;
                    Debug.WriteLine($"Score: {score}");
                }

                // If only contains letters
                if (!(containsNumbers || containsSymbols))
                {
                    Debug.WriteLine("Only contains letters");
                    score -= 5;
                    Debug.WriteLine($"Score: {score}");
                }

                // If only contains numbers
                if (!(containsLowerCase || containsUpperCase || containsSymbols))
                {
                    Debug.WriteLine("Only contains numbers");
                    score -= 5;
                    Debug.WriteLine($"Score: {score}");
                }

                // If only contains symbols
                if (!(containsLowerCase || containsUpperCase || containsNumbers))
                {
                    Debug.WriteLine("Only contains symbols");
                    score -= 5;
                    Debug.WriteLine($"Score: {score}");
                }

                // List of List (2D array) to respresent a QUERTY keyboard
                List<List<char>> keyboard = new List<List<char>>() { 
                    new List<char>{ 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' },
                    new List<char>{ 'a','s','d','f','g','h','j','k','l' },
                    new List<char>{ 'z','x','c','v','b','n','m' }
                };

                // Create an array for the password text for easy iteration
                char[] passwordArray = text.ToLower().ToCharArray();
                // Do once for each row of the keyboard
                foreach(List<char> keyRow in keyboard)
                {
                    // Do once for each character in the password, apart from the last two
                    for(int charPosition = 0; charPosition < passwordArray.Length - 2; charPosition++)
                    {
                        // Where does the password character fall in the keyboard?
                        int tripletIndex = keyRow.IndexOf(passwordArray[charPosition]);
                        // Do if the key is on this row of the keyboard and if this isn't the last two keys
                        if (tripletIndex >= 0 && tripletIndex < keyRow.Count - 2)
                        {
                            char[] keyboardTriplet = 
                            {
                                keyRow[tripletIndex],
                                keyRow[tripletIndex + 1],
                                keyRow[tripletIndex + 2]
                            };  // Create an object to store the key and next two consecutive keys


                            char[] passwordTriplet =
                            {
                                passwordArray[charPosition],
                                passwordArray[charPosition + 1],
                                passwordArray[charPosition + 2],
                            };  // Create an object to store the password character and next two consecutive

                            // Testing Code
                            /*for (int i = 0; i < 3; i++) {
                                Debug.WriteLine($"Keyboard: {keyboardTriplet[i].ToString()}");
                                Debug.WriteLine($"Password: {passwordTriplet[i].ToString()}");
                                Debug.WriteLine($"Equal: {passwordTriplet[i] == keyboardTriplet[i]}");
                            }
                            Debug.WriteLine("");*/

                            if (keyboardTriplet.SequenceEqual(passwordTriplet))
                            {
                                // Take away 5 points if there is a match

                                score -= 5;

                                // Testing Code
                                Debug.WriteLine($"Triplet found: {passwordTriplet[0].ToString() + passwordTriplet[1].ToString() + passwordTriplet[2].ToString()}");
                                Debug.WriteLine($"Score: {score}");
                            }
                        }
                    }
                }

                Debug.WriteLine($"Score: {score}");

                // Password is strong if score is greater than 20
                if (score > 20)       
                    strengthTier = PasswordStrengthTier.Strong;
                // Password is weak if score is less than 1
                else if (score < 1)   
                    strengthTier = PasswordStrengthTier.Weak;
                // Password is medium strength if score is between 0 and 21
                else
                    strengthTier = PasswordStrengthTier.Medium;

                Evaluated = true;

            }
        }

        public static Password Generate()
        {
            Password password = new Password();

            // Create an array for each allowed symbol
            char[] allowedChars = { '!', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+' };

            // Create a random number generator
            Random randomGenerator = new Random();

            // Generate the length of the new password according to specification
            int passwordLength = randomGenerator.Next(8,13);

            int charCode;
            for (int i = 0; i < passwordLength; i++)
            {
                // Decide what type of character to add to the password
                switch(randomGenerator.Next(0, 4))
                {
                    case 0:
                        // Get an uppercase letter
                        charCode = randomGenerator.Next(65, 91);
                        password.text += char.ConvertFromUtf32(charCode);
                        break;
                    case 1:
                        // Get a lowercase letter
                        charCode = randomGenerator.Next(97, 123);
                        password.text += char.ConvertFromUtf32(charCode);
                        break;
                    case 2:
                        // Get a number
                        int number = randomGenerator.Next(0, 10);
                        password.text += number.ToString();
                        break;
                    case 3:
                        // Work get a lowercase letter
                        int index = randomGenerator.Next(0, 12);
                        password.text += allowedChars[index];
                        break;
                }
            }

            Debug.WriteLine(password.text);

            // Calculate the score of the new password
            password.Evaulate();

            // If the password is strong, return the password
            if (password.strengthTier == PasswordStrengthTier.Strong)
                return password;
            // Recursively generate a new password and return it if not strong
            else
                return Generate();  
        }

    }

    public enum PasswordStrengthTier
    {
        Weak,
        Medium,
        Strong
    }

}
