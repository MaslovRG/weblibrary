using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Gateway.Controllers;
using System.Collections.Generic;

namespace WebLibraryTests
{
    [TestClass]
    public class GatewayUnitTests
    {
        [TestMethod]
        public void GWTest1()
        {
            GatewayController gwc = new GatewayController();

            ActionResult<IEnumerable<string>> result = gwc.Get();
            IEnumerator<string> rl = result.Value.GetEnumerator();

            rl.MoveNext(); 
            Assert.AreEqual("value1", rl.Current);
            rl.MoveNext(); 
            Assert.AreEqual("value2", rl.Current); 
        }
    }


}
