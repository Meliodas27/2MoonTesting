using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Drawing.Imaging;
using _2MoonTesting.Pages;

namespace _2moonTestProject.Tests
{
    [TestFixture("chrome")]
    //[TestFixture("firefox")]
    //[TestFixture("edge")]
    //[TestFixture("safari")]
    public class LoginTest : IDisposable
    {

        private IWebDriver driver;
        private LoginPage loginPage;
        private ForgotPasswordPage forgotPasswordPage;
        private readonly string browser;

        private ExtentReports extent;
        private ExtentSparkReporter sparkReporter;
        private ExtentTest test;


        public LoginTest(string browser)
        {
            this.browser = browser;
            sparkReporter = new ExtentSparkReporter($"../../Reports/2MOONTestReport_{DateTime.Now:dd_MM_yyyy_hh_mm tt}.html");
            extent = new ExtentReports();
            extent.AttachReporter(sparkReporter);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SetUp]
        public void Setup()
        {
            driver = GetWebDriver(browser);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://staging.backtester.2moon.trade/login");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            loginPage = new LoginPage(driver);
            forgotPasswordPage = new ForgotPasswordPage(driver);

        }

        [TearDown]

        public void TearDown()
        {

            Dispose();

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                extent?.Flush();
                driver?.Quit();
            }

        }





        private static IWebDriver GetWebDriver(string browser)
        {
            return browser.ToLower() switch
            {
                "chrome" => new ChromeDriver(),
                "firefox" => new FirefoxDriver(),
                "edge" => new EdgeDriver(),
                "safari" => new SafariDriver(),
                _ => throw new NotSupportedException($"El navegador '{browser}' no se encuentra en el listado"),
            };
        }

        private void TakeScreenshot(IWebDriver driver, ExtentTest test)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            var screenshotAsBase64 = screenshot.AsBase64EncodedString;
            test.AddScreenCaptureFromBase64String(screenshotAsBase64);
        }




        [Test]
        [Description("Verify on entering invalid Email and valid password, the user can’t login")]
        public void LoginInvalidEmail()
        {
            var description = TestContext.CurrentContext.Test.Properties.Get("Description").ToString();
            test = extent.CreateTest(description);
            string Email = "pruebas2@gmail.com";
            string Password = "S2?C9j8H$_25";
            loginPage.Login(Email, Password);
            bool isToastElementPresent = loginPage.FindToastElement();
            test.Log(Status.Info, $"Login attempt with Email: {Email} and Password: {Password}");
            if (isToastElementPresent)
            {
                test.Pass("Test passed");
                TakeScreenshot(driver, test);
            }
            else
            {
                test.Fail("Failed Test");
                TakeScreenshot(driver, test);
            }
            Assert.That(isToastElementPresent, Is.EqualTo(true));
        }




        [Test]
        [Description("Verify on entering valid Email and Invalid password, the user can’t login")]
        public void LoginInvalidPassword()
        {
            var description = TestContext.CurrentContext.Test.Properties.Get("Description").ToString();
            test = extent.CreateTest(description);
            string Email = "schrodinger.meliodas@gmail.com";
            string Password = "tempss";
            loginPage.Login(Email, Password);
            bool isToastElementPresent = loginPage.FindToastElement();
            test.Log(Status.Info, $"Login attempt with Email: {Email} and Password: {Password}");
            if (isToastElementPresent)
            {
                test.Pass("Test passed");
                TakeScreenshot(driver, test);
            }
            else
            {
                test.Fail("Failed Test");
                TakeScreenshot(driver, test);
            }
            Assert.That(isToastElementPresent, Is.EqualTo(true));
            Thread.Sleep(2000);

        }


        [Test]
        [Description("Verify on entering invalid Email and password, the user can’t login")]

        public void LoginInvalidEmailAndPassword()
        {
            var description = TestContext.CurrentContext.Test.Properties.Get("Description").ToString();
            test = extent.CreateTest(description);
            string Email = "prueba2@prueba.com";
            string Password = "tempss";
            loginPage.Login(Email, Password);
            bool isToastElementPresent = loginPage.FindToastElement();
            test.Log(Status.Info, $"Login attempt with Email: {Email} and Password: {Password}");
            if (isToastElementPresent)
            {
                test.Pass("Test passed");
                TakeScreenshot(driver, test);
            }
            else
            {
                test.Fail("Failed Test");
                TakeScreenshot(driver, test);
            }
            Assert.That(isToastElementPresent, Is.EqualTo(true));
            Thread.Sleep(2000);

        }


        [Test]
        [Description("Verify on entering valid Email and password, the user can login")]
        public void LoginValidCredentials()
        {
            var description = TestContext.CurrentContext.Test.Properties.Get("Description").ToString();
            test = extent.CreateTest(description);
            string Email = "schrodinger.meliodas@gmail.com";
            string Password = "S2?C9j8H$_25";
            string NewUrl = "https://staging.backtester.2moon.trade/dashboard/strategies";
            loginPage.Login(Email, Password, true);
            Thread.Sleep(7000);

            test.Log(Status.Info, $"Login attempt with Email: {Email} and Password: {Password}");
            if (driver.Url.Equals(NewUrl))
            {
                test.Pass("Test passed");
                TakeScreenshot(driver, test);
            }
            else
            {
                test.Fail("Failed Test");
                TakeScreenshot(driver, test);
            }
            Assert.That(driver.Url, Is.EqualTo(NewUrl));
        }

        [Test]
        [Description("Check Reset Password for existing user Test")]

        public void ResetPassword()
        {
            string NewUrl = "https://staging.backtester.2moon.trade/forgot-password";
            var description = TestContext.CurrentContext.Test.Properties.Get("Description").ToString();
            test = extent.CreateTest(description);
            string Email = "alv.juan.sis@gmail.com";

            if (!driver.Url.Equals(NewUrl))
            {
                driver.Navigate().GoToUrl(NewUrl);
                Thread.Sleep(3000);
            }
            else
            {
                loginPage.ForgotLinkClick();
            }
            forgotPasswordPage.ResetPassword(Email);
            Thread.Sleep(3000);
            test.Log(Status.Info, $"Reset password for this Email : {Email}");

            if (driver.Url.Contains("reset-password"))
            {
                test.Pass("Test passed");
                TakeScreenshot(driver, test);

            }
            else
            {
                test.Fail("Failed Test");
                TakeScreenshot(driver, test);
            }
            Assert.That(driver.Url, Does.Contain("reset-password"));

        }

        [Test]
        [Description("Check Reset not Exist User Password Test")]

        public void ResetPasswordNonExistingUser()
        {


        }
    }
}
