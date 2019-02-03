//******************************************************************************
//
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.
//
// This code is licensed under the MIT License (MIT).
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//******************************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using System.Threading;
using System;
using System.Diagnostics;

namespace CalculatorTest
{
    [TestClass]
    public class ScenarioStandard : CalculatorSession
    {
        private static WindowsElement header;
        private static WindowsElement calculatorResult;

        [TestMethod]
        public void Addition()
        {
         // Find the buttons by their names using FindElementByName and click them in sequence to peform 2 + 3 = 5
            session.FindElementByName("Two").Click();
            session.FindElementByName("Plus").Click();
            session.FindElementByName("Three").Click();
            session.FindElementByName("Equals").Click();
            Assert.AreEqual("5", GetCalculatorResultText());
        }

        [TestMethod]
        public void Subtraction()
        {
         // Find the buttons by their accessibility ids using FindElementByAccessibilityId and click them in sequence to perform 2-3 = -1 
            session.FindElementByAccessibilityId("num2Button").Click();
            session.FindElementByAccessibilityId("minusButton").Click();
            session.FindElementByAccessibilityId("num3Button").Click();
            session.FindElementByAccessibilityId("equalButton").Click();
            Assert.AreEqual("-1", GetCalculatorResultText());
        }

        [TestMethod]
        public void Division()
        {
        // Find the buttons by their accessibility ids using FindElementByAccessibilityId & click them in sequence to perform 3/2 = 1.5 
            session.FindElementByAccessibilityId("num3Button").Click();
            session.FindElementByAccessibilityId("divideButton").Click();
            session.FindElementByAccessibilityId("num2Button").Click();
            session.FindElementByAccessibilityId("equalButton").Click();
            Assert.AreEqual("1.5", GetCalculatorResultText());
        }

        [TestMethod]
        public void Multiplication()
        {
        // Find the buttons by their accessibility ids using FindElementByAccessibilityId & click them in sequence to perform 9*9 = 81 
            session.FindElementByAccessibilityId("num9Button").Click();
            session.FindElementByAccessibilityId("multiplyButton").Click();
            session.FindElementByAccessibilityId("num9Button").Click();
            session.FindElementByAccessibilityId("equalButton").Click();
            Assert.AreEqual("81", GetCalculatorResultText());
        }

       
        [TestMethod]
        public void ClearWrongDigit()
        {
        // Find the buttons by their accessibility ids using FindElementByAccessibilityId and clear wrongly entered digit 2 to 4
            session.FindElementByAccessibilityId("num2Button").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            session.FindElementByAccessibilityId("clearButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            session.FindElementByAccessibilityId("num4Button").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.AreEqual("4", GetCalculatorResultText());
        }

        [TestMethod]
        public void StoreNumber()
        {
        // Find the buttons by their accessibility ids using FindElementByAccessibilityId to store and recall the numbers from memory
            session.FindElementByAccessibilityId("num9Button").Click();
            session.FindElementByAccessibilityId("num8Button").Click();
            session.FindElementByAccessibilityId("num7Button").Click();
            session.FindElementByAccessibilityId("num6Button").Click();
            session.FindElementByAccessibilityId("num5Button").Click();
            session.FindElementByName("Memory Store").Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            session.FindElementByAccessibilityId("clearButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            session.FindElementByName("Memory Recall").Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Assert.AreEqual("98,765", GetCalculatorResultText());
        }

        [TestMethod]
        public void Reset()
        {
        // Find the buttons by their accessibility ids using FindElementByAccessibilityId and clear entry
            session.FindElementByAccessibilityId("num9Button").Click();
            session.FindElementByAccessibilityId("num8Button").Click();
            session.FindElementByAccessibilityId("num7Button").Click();
            session.FindElementByAccessibilityId("num6Button").Click();
            session.FindElementByAccessibilityId("num5Button").Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            session.FindElementByName("Clear entry").Click();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Assert.AreEqual("0", GetCalculatorResultText());
            
        }
        [TestMethod]
        public void StoreOperations()
        {
        // Find the buttons by their accessibility ids using FindElementByName and store the history of operations performed
            session.FindElementByName("Two").Click();
            session.FindElementByName("Plus").Click();
            session.FindElementByName("Three").Click();
            session.FindElementByName("Equals").Click();          
            session.FindElementByName("History").Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.AreEqual("5", GetCalculatorResultText());
            

        }

       

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Create session to launch a Calculator window
            Setup(context);

            // Identify calculator mode by locating the header
            try
            {
                header = session.FindElementByAccessibilityId("Header");
            }
            catch
            {
                header = session.FindElementByAccessibilityId("ContentPresenter");
            }            

            // Ensure that calculator is in standard mode
            if (!header.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                session.FindElementByAccessibilityId("NavButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var splitViewPane = session.FindElementByClassName("SplitViewPane");
                splitViewPane.FindElementByName("Standard Calculator").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(header.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase));
            }

            // Locate the calculatorResult element
            calculatorResult = session.FindElementByAccessibilityId("CalculatorResults");
            Assert.IsNotNull(calculatorResult);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestInitialize]
        public void Clear()
        {
            session.FindElementByName("Clear").Click();
            Assert.AreEqual("0", GetCalculatorResultText());
        }

        private string GetCalculatorResultText()
        {
            return calculatorResult.Text.Replace("Display is", string.Empty).Trim();
        }
    }
}
