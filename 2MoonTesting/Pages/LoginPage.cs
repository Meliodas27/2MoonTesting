using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2MoonTesting.Pages
{
    public class LoginPage(IWebDriver driver)
    {
        private readonly IWebDriver driver = driver;
        private readonly WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

        private IWebElement PasswordField => driver.FindElement(By.XPath("//input[@placeholder='Password']"));

        private IWebElement EmailField => driver.FindElement(By.XPath("//input[@placeholder='Email']"));
        private IWebElement RememberCheck => driver.FindElement(By.XPath("//input[@type='checkbox']"));
        private IWebElement LoginButton => driver.FindElement(By.CssSelector("button[type='submit']"));
        private IWebElement ForgotLink => driver.FindElement(By.XPath("//a[normalize-space()='Forgot Password']"));
        private IWebElement RegisterLink => driver.FindElement(By.XPath("(//a[normalize-space()=\"Don't have an account? Register\"])[1]"));

        private IWebElement SignGoogleButton => driver.FindElement(By.XPath("//span[@class='text-gray-500']"));

        public bool FindToastElement()
        {


            var ToastElement = wait.Until<IWebElement>(driver =>
            {
                try
                {
                    var ToastElementDisplayed = driver.FindElement(By.XPath("//div[@class='styles_toast__j2h6b']"));
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

        public void ForgotLinkClick()
        {
            ForgotLink.Click();


        }

        public void Login(string email, string password, bool Check=false)
        {
            EmailField.SendKeys(email);
            PasswordField.SendKeys(password);
            if(Check) RememberCheck.Click();
            LoginButton.Submit();
            
        }
    }
}
