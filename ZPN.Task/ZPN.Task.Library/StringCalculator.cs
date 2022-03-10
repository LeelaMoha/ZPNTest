using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZPN.Task.Library
{
    public class StringCalculator
    {
        public decimal Calculate(string input)
        {
            CheckForValidInput(ref input);
            List<string> splittedInputs = SplitInput(input);
            return Evaluate(splittedInputs);
        }

        public void CheckForValidInput(ref string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("input cannot be null or empty");
            }
            input = input.Replace(" ", "");

            Regex regex = new Regex(@"^(\d+[-+\/*])+\d+$");
            if (!regex.IsMatch(input))
            {
                throw new ArgumentException("input is not valid");
            }
        }

        public List<string> SplitInput(string input)
        {
            Regex regex = new Regex(@"(\d+|[-+\/*]){1}");
            return regex.Matches(input).Select(match => match.Value).ToList();
        }


        public decimal Evaluate(List<string> splittedInputs)
        {
            for (int i = 1; i < splittedInputs.Count - 1; i++)
            {
                string mathOperator = splittedInputs[i];
                bool isOperator = mathOperator == "*" || mathOperator == "/";
                if (isOperator)
                {
                    decimal partialResult = DoCalculation(splittedInputs[i - 1], mathOperator, splittedInputs[i + 1]);
                    FillCalculatedWithSpaces(splittedInputs, i, partialResult);
                }
            }
            splittedInputs = splittedInputs.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            for (int i = 1; i < splittedInputs.Count - 1; i++)
            {
                string mathOperator = splittedInputs[i];
                bool isOperator = mathOperator == "+" || mathOperator == "-";
                if (isOperator)
                {
                    decimal partialResult = DoCalculation(splittedInputs[i - 1], mathOperator, splittedInputs[i + 1]);
                    FillCalculatedWithSpaces(splittedInputs, i, partialResult);
                }
            }
            splittedInputs = splittedInputs.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            return decimal.Parse(splittedInputs[0]);
        }

        private void FillCalculatedWithSpaces(List<string> splittedInputs, int index, decimal partialResult)
        {
            splittedInputs[index - 1] = "";
            splittedInputs[index] = "";
            splittedInputs[index + 1] = partialResult.ToString();
        }

        private decimal DoCalculation(string firstString, string operation, string secondString)
        {
            decimal firstNumber = decimal.Parse(firstString);
            decimal secondNumber = decimal.Parse(secondString);
            switch (operation)
            {
                case "+":
                    return checked(firstNumber + secondNumber);
                case "-":
                    return checked(firstNumber - secondNumber);
                case "*":
                    return checked(firstNumber * secondNumber);
                case "/":
                        return firstNumber / secondNumber;                    
                default:
                    throw new InvalidOperationException($"Operation {operation} is not supported");
            }
        }

       
    }
}
