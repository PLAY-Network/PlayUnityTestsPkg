using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RGN.Extensions;
using RGN.Modules.EmailSignIn;
using UnityEngine.TestTools;

namespace RGN.Tests
{
    [TestFixture]
    public abstract class BaseTests
    {
        private bool oneTimeSetup;
        private TestsEnvironment testEnvironment;

        protected virtual List<IRGNModule> Modules { get; } = new List<IRGNModule>();

        protected virtual IEnumerator SetUp()
        {
            yield return null;
        }
        protected virtual IEnumerator OneTimeSetUp()
        {
            yield return null;
        }

        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            yield return UnityOneTimeSetUp();
            yield return SetUp();
        }
        [OneTimeTearDown]
        public void UnityTearDown()
        {
            testEnvironment.Dispose();
            testEnvironment = null;
        }

        private IEnumerator UnityOneTimeSetUp()
        {
            if (!oneTimeSetup)
            {
                var dependencies = new Impl.Firebase.Dependencies();
                var modules = new List<IRGNModule>(Modules) { new EmailSignInModule() };

                testEnvironment = new TestsEnvironment();

                var setupEnvironmentTask = testEnvironment.SetupEnvironment(dependencies, modules);

                yield return setupEnvironmentTask.AsIEnumeratorReturnNull();
                yield return LoginAsNormalTester();

                yield return OneTimeSetUp();

                oneTimeSetup = true;
            }
        }

        protected IEnumerator LoginAsAdminTester()
        {
            yield return testEnvironment.SetupTestAccount(true).AsIEnumeratorReturnNull();
        }
        protected IEnumerator LoginAsNormalTester()
        {
            yield return testEnvironment.SetupTestAccount(false).AsIEnumeratorReturnNull();
        }
    }
}
