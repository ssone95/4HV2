//
// _4H_Application_ModelTest.Horses_Test.HorseManager_Test.cs: 
// Class for unit testing the HorseManager class from _4H_Application
//
// Author(s):
//   Joel Krause (jkjk8080@gmail.com)
//

using System;
using _4H_Application.Models.Horses;
using _4H_Application.Models.Horses.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _4h_Application_ModelTest.Horses_Test
{
    [TestClass]
    public class HorseManager_Tests
    {
        [TestMethod]
        public void HorseManager_GivesMeTheManagerCorrectly ()
        {
            var manager = HorseManager.GetInstance();

            Assert.AreEqual(typeof(HorseManager), manager.GetType());
        }

        [TestMethod]
        [ExpectedException(typeof(HorseNotFoundException))]
        public void HorseManager_ThrowsExpectedErrorWithBadHorseID()
        {
            HorseManager.GetInstance().GetHorseById(-10);
        }

        //[TestMethod]
        //public async void HorseManager_MakesAHorseAndGivesMeTheRightHorse()
        //{
        //    Horse newHorse = await HorseManager.GetInstance().Create();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(TooManyHorsesException))]
        //public async void HorseManager_MaxHorseLimitIsRespected()
        //{
        //    while (HorseManager.GetInstance().Horses.Count < HorseManager.MAX_HORSES + 1)
        //    {
        //        await HorseManager.GetInstance().Create();
        //    }
        //}
    }
}
