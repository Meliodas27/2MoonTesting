﻿using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2MoonTesting.Pages
{
    public class ForgotPasswordPage(IWebDriver driver)
    {
        private readonly IWebDriver driver = driver;
        private readonly WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

        private IWebElement ResetButton => driver.FindElement(By.CssSelector("button[type='submit']"));
        private IWebElement EmailField => driver.FindElement(By.XPath("//input[@placeholder='Email']"));
        private IWebElement LoginLink => driver.FindElement(By.XPath("//a[normalize-space()='Have you already remember your password? Log In']"));


        public void ResetPassword(string email)
        {
            //wait.Until()
            EmailField.SendKeys(email);
            ResetButton.Submit();


        }


        public bool FindToastElement()
        {


            var ToastElement = wait.Until<IWebElement>(driver =>
            {
                try
                {
                    var ToastElementDisplayed = driver.FindElement(By.XPath("//span[@class='text-xs']"));
                    if (ToastElementDisplayed.Displayed)
                    {
                        return ToastElementDisplayed;
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
            if (ToastElement != null) return true;
            return false;

        }


    }
}
