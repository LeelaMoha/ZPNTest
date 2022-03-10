using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ZPN.Task.Library;

namespace StringEvaluator.UnitTests
{
    public class StringCalculatorTests
    {

        [TestCase("4+5*2", 4 + 5 * 2)]
        [TestCase("4+5/2", 4 + 5.0 / 2.0)]
        [TestCase("4+5/2-1", 4 + 5.0 / 2.0 - 1)]
        [TestCase("4/4", 4 / 4)]              
        public void Test_Results(string input, decimal expectedResult)
        {
            StringCalculator calculator = new StringCalculator();
            decimal result = calculator.Calculate(input);
            Assert.That(expectedResult, Is.EqualTo(result).Within(0.00000001));
        }

        [TestCase("1/0")]
        [TestCase("0/0")]
        [TestCase("1+2/0*3")]
        [TestCase("1+2-3/0")]
        public void Test_When_Divide_By_Zero(string input)
        {
            StringCalculator calculator = new StringCalculator();
            void action() => calculator.Calculate(input);
            Exception ex = Assert.Throws<DivideByZeroException>(() => action());
        }

        [TestCase("923627272828883838838873737373333+10")]
        public void Big_input_Should_Trow_Error(string input)
        {
            StringCalculator calculator = new StringCalculator();
            void action() => calculator.Calculate(input);
            Exception ex = Assert.Throws<OverflowException>(() => action());
        }

        [Test]
        public void Null_Input_Should_ThrowError()
        {
            StringCalculator calculator = new StringCalculator(); 
            Exception ex = Assert.Throws<ArgumentException>(() => calculator.Calculate(null as string));
            Assert.That(ex.Message, Is.EqualTo("input cannot be null or empty"));
        }

        [Test]
        public void Empty_Input_Should_ThrowError()
        {            
            StringCalculator calculator = new StringCalculator();            
            Exception ex = Assert.Throws<ArgumentException>(() => calculator.Calculate(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("input cannot be null or empty"));
        }

        [TestCase("abc")]
        [TestCase("1+2+-b")]
        [TestCase("a+2-c")]       
        public void Invalid_Input_Should_ThrowError(string input)
        {           
            StringCalculator calculator = new StringCalculator();            
            Exception ex = Assert.Throws<ArgumentException>(() => calculator.Calculate(input));
            Assert.That(ex.Message, Is.EqualTo("input is not valid"));
        }

        [TestCase(" 1+ 1 *2", "1+1*2")]       
        public void Invalid_Spaces_Input_Should_ThrowError(string input, string inputWithoutSpaces)
        {           
            StringCalculator calculator = new StringCalculator();
            calculator.CheckForValidInput(ref input);
            Assert.AreEqual(input, inputWithoutSpaces);
        }

      
    }
}