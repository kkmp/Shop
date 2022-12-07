using NUnit.Framework;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    internal static class DbResultExtension
    {
        public static void EnsureFailMessage<T>(this DbResult<T> result, string expectedMessage)
        {
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.AreEqual(expectedMessage, result.Message);
        }
    }
}
