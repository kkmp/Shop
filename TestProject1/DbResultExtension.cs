using NUnit.Framework;
using Shop.Data;

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

        public static void EnsureSuccessMessage<T>(this DbResult<T> result, string expectedMessage)
        {
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.AreEqual(expectedMessage, result.Message);
        }
    }
}
