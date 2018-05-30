using System;
using System.Collections.Generic;
using DataModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            JsonModel jsonModel = new JsonModel()
            {
                BjModel = new BjModel()
                {
                    ID = "baby96me",
                    Nic = "nic"
                },
                UserModels = new List<UserModel>()
                {
                    new UserModel()
                    {
                        ID = "mms218",
                        Nic = "testnic"
                    }
                }
            };

            //var re = Get(jsonModel);
            //test = RunAsync(jsonModel);


        }
    }
}
